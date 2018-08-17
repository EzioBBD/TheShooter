using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HP = 100;    //人物生命值
    public bool isHurt = false;     //测试用

    private SpriteRenderer healthBar;   //血条前景UI
    private SpriteRenderer healthBg;    //血条背景UI 
    private Vector3 healthScale;    //生命条的Scale
    private bool HPLerping = false;    //HP正在渐变中
    private bool dead = false;      //是否死亡

    private float moveForce;
    private float jumpForce;

    // Use this for initialization
    void Start ()
    {
        Transform UI_HP = transform.Find("UI_HealthDisplay");
	    healthBar = UI_HP.Find("HealthBar").GetComponent<SpriteRenderer>();
        healthBg = UI_HP.Find("HealthBg").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;

        moveForce = GetComponent<PlayerController>().moveForce;
        jumpForce = GetComponent<PlayerController>().jumpForce;
    }

    void Update()
    {
        if (transform.position.y < -20) //如果掉入河里直接死亡
        {
            TakeDamage(HP);
        }

        //HP减少的渐变效果
        if (HPLerping)
        {
            healthBg.transform.localScale = Vector3.Lerp(healthBg.transform.localScale,
                healthBar.transform.localScale, Time.deltaTime * 1.2f);
            if (Vector3.Distance(healthBg.transform.localScale, healthBar.transform.localScale) < 0.000001)
            {
                if (dead)
                    this.enabled = false;
                HPLerping = false;
            }
        }
    }

    /// <summary>
    /// 玩家受伤
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, 100);

        //角色死亡处理
        if (HP < 0.01f)
        {
            if (!dead)
            {
                if (GetComponent<PlayerController>())
                    GetComponent<PlayerController>().enabled = false;
                GetComponent<Animator>().SetTrigger("Die");
                dead = true;
                Destroy(gameObject,3);
                StartCoroutine(Camera.main.GetComponent<MainCamera>().CameraFocus(tag));
            }
        }

        //更新血条显示
        healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
        HPLerping = true;
        //TODO:播放音效
    }

    /// <summary>
    /// 火属性子弹的灼烧效果
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public void FireDebuff(float damage, int n)
    {
        StartCoroutine(Scorch(damage, n));
    }

    IEnumerator Scorch(float damage, int n)
    {
        while (n-- > 0)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    ///  冰属性子弹的灼烧效果
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="debuffTime"></param>
    public void IceDebuff(float damage, float debuffTime)
    {
        TakeDamage(damage);
        StartCoroutine(SlowDown(debuffTime));
    }

    IEnumerator SlowDown(float debuffTime)
    {
        GetComponent<PlayerController>().moveForce *= 0.8f;
        GetComponent<PlayerController>().jumpForce *= 0.8f;

        yield return new WaitForSeconds(debuffTime);

        GetComponent<PlayerController>().moveForce = moveForce;
        GetComponent<PlayerController>().jumpForce = jumpForce;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "River")
            TakeDamage(HP);
    }
}
