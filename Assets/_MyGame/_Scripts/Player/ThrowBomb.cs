using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    public GameObject bombPref;        //炸弹预制体
    public AudioClip bombsAway;         //投掷音效
    public float timer = 0f;            //蓄力计时器
    public float maxTime = 1.5f;        //最大蓄力时间
    public float throwForce = 1f;       //投掷力

    private PropManager propManager;    //道具管理类的引用

    void Start()
    {
        propManager = GetComponent<PropManager>();
    }

    public void Charge()
    {
        if (propManager.specialBomb > 0 || propManager.normalBomb > 0)
        {
            timer += Time.deltaTime;
            if (timer >= maxTime)
                timer = maxTime;
        }
    }

    public void Throw(Vector2 targetDir)
    {
        if (bombPref.GetComponent<Thrown>().isSpecial)
        {
            if (propManager.specialBomb < 1)
                return;
            --propManager.specialBomb;
            propManager.UpdateDisplay();
        }
        else
        {
            if (propManager.normalBomb < 1)
                return;
            --propManager.normalBomb;
            propManager.UpdateDisplay();
        }

        //播放音效
        AudioSource.PlayClipAtPoint(bombsAway, transform.position);

        //子弹射向目标位置
        GameObject bomb = Instantiate(bombPref, transform.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().velocity = targetDir.normalized * throwForce * timer;

        //注册炸弹的爆炸的方法到PlayerController的事件中
        if (bomb.GetComponent<Thrown>().isSpecial)
        {
            GetComponent<PlayerController>().detonateFunc += bomb.GetComponent<Thrown>().Explode;
            if (gameObject.tag == "Enemy")
                bomb.GetComponent<Thrown>().isPlayer = false;
        }

        timer = 0;
    }
}
