using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPref;  //子弹预制体
    public int speed = 25;          //子弹初速度

    private Animator anim;          //角色动画的引用         
    private AudioSource audio;      //角色音源的引用
    private Transform bulletSpawn;  //初始化子弹的transform

    private PropManager propManager;    //道具管理类的引用

    // Use this for initialization
    void Start ()
	{
	    anim = transform.root.GetComponent<Animator>();
	    audio = transform.root.GetComponent<AudioSource>();
	    bulletSpawn = transform.Find("bulletSpawn");
	    propManager = transform.root.GetComponent<PropManager>();
	}

    /// <summary>
    /// 武器开火
    /// </summary>
    /// <param name="mouseDir"></param>
    public void Fire(Vector2 mouseDir)
    {
        //播放动画和音效
        anim.SetTrigger("Shoot");
        audio.Play();

        switch (bulletPref.GetComponent<Bullet>().bulletType)
        {
            case Bullet.BulletType.Normal:
                if (propManager.normalBullet < 1)
                    return;
                --propManager.normalBullet;
                propManager.UpdateDisplay();
                break;
            case Bullet.BulletType.Fire:
                --propManager.fireBullet;
                propManager.UpdateDisplay();
                break;
            case Bullet.BulletType.Ice:
                --propManager.iceBullet;
                propManager.UpdateDisplay();
                break;
        }

        //初始化子弹
        GameObject bullet = Instantiate(bulletPref, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = mouseDir.normalized * speed;
    }
}
