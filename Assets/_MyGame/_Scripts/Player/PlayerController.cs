using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public bool joystick = false;   //是否用手柄操控

    [HideInInspector]
    public int isFacingRight = 1; //是否面向右边
    [HideInInspector]
    public bool isjumping = false;  //是否触发跳跃键

    public float moveForce = 1.0f; //人物移动时加的力
    public float maxSpeed = 10f; //最大移动速度
    public float jumpForce = 1000f; //人物跳跃时加的力
    public Weapon weapon;          //武器脚本引用 

    private Rigidbody2D rb;
    private Animator anim;
    private Transform groundCheck;      //用于检测是否着地的点
    private bool isGrounded = false;    //人物是否着地
    private ThrowBomb throwBomb;    //投掷炸弹脚本引用
    private Vector2 targetDir;       //准星方向

    //游戏按键映射
    private string fire1 = "Fire1";
    private string fire2 = "Fire2";
    private string jump = "Jump";
    private string horizontal = "Horizontal";
    private string detonate = "Detonate";
    private string interact = "Interact";
    public float jumpTimer = 0f;   //跳跃蓄力计时器

    public delegate void Detonate();

    public event Detonate detonateFunc;

    void Awake()
    {
        //如果是手柄模式，修改按键映射
        if (joystick)
        {
            fire1 = "JoyFire1";
            fire2 = "JoyFire2";
            jump = "JoyJump";
            horizontal = "JoyHorizontal";
            detonate = "JoyDetonate";
            interact = "JoyInteract";
        }
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("groundCheck");
        weapon = transform.Find("bone").Find("rh").Find("normal weapon").GetComponent<Weapon>();
        throwBomb = GetComponent<ThrowBomb>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Vector3.zero;
        if (joystick)
            JoystickMode();
        else
            targetPos = MouseMode();

        if (isFacingRight > 0)
            weapon.transform.parent.rotation = Quaternion.FromToRotation(Vector3.right, targetDir.normalized);
        else
            weapon.transform.parent.localRotation = Quaternion.FromToRotation(targetDir, Vector3.left);

        Vector2 bulletPos = weapon.transform.Find("bulletSpawn").position;
        if (joystick)
        {
            Debug.DrawLine(bulletPos, bulletPos + targetDir.normalized * 100, Color.red);
        }
        
        //点击鼠标左键开火
        if (Input.GetButtonDown(fire1))
        {
            if (!joystick)
            {
                targetDir = (Vector2)targetPos - bulletPos;
                if (Vector3.Magnitude(targetPos - transform.position) > 10.5f)
                    weapon.Fire(targetDir);
            }
            else
                weapon.Fire(targetDir);
        }

        //点击鼠标右键投掷炸弹
        //按住蓄力
        if (Input.GetButton(fire2))
            throwBomb.Charge();
        //松开释放
        if (Input.GetButtonUp(fire2))
            throwBomb.Throw(targetDir);

        //射线检测是否碰到地面，若是则可以跳跃
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown(jump) && isGrounded)
        {
            
            anim.SetTrigger("Jump");
            rb.AddForce(new Vector2(0, jumpForce));
            isjumping = true;
        }
        //跳跃蓄力
        if (isjumping)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jumpTimer += Time.deltaTime;
            if (jumpTimer > 0.15f)
            {
                isjumping = false;
            }

        }
        if (Input.GetButtonUp(jump))
        {
            isjumping = false;
            jumpTimer = 0;
        }

        //引爆定时炸弹
        if (Input.GetButtonUp(detonate))
        {
            if (detonateFunc != null)
                detonateFunc();
        }

        //和控制台交互
        if (Input.GetButtonUp(interact))
        {
            Console.Instance.ConsoleInteract();
        }
    }

    void FixedUpdate()
    {
        //控制角色移动
        float h = Input.GetAxisRaw(horizontal);
        anim.SetFloat("Speed", Mathf.Abs(h));
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            rb.AddForce(Vector2.right * moveForce * h);
        else
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
    }

    /// <summary>
    /// 人物精灵转向
    /// </summary>
    void ChangeSpriteDir()
    {

        isFacingRight = -isFacingRight;
        transform.RotateAround(groundCheck.position, Vector3.up, 180);
    }

    void JoystickMode()
    {
        float h = Input.GetAxis("RStickH");

        if (Mathf.Abs(h) > 0.3f && isFacingRight * h < 0)
            ChangeSpriteDir();

        float v = Input.GetAxis("RStickV");
        if (Mathf.Abs(h) > 0.05f && Mathf.Abs(v) > 0.05f)
            targetDir = new Vector2(h, v);
    }

    Vector3 MouseMode()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //控制人物精灵转向
        targetDir = mousePos - groundCheck.position;
        if (targetDir.x * isFacingRight < 0)
            ChangeSpriteDir();

        //枪口方向始终对准准星位置 
        targetDir -= new Vector2(0, 0.76f);
        
        return mousePos;
    }
}
