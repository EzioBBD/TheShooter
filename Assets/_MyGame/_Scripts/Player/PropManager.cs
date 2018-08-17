using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropManager : MonoBehaviour
{
    public int normalBullet = 10;   //普通子弹数量
    public int normalBomb = 5;      //普通炸弹数量
    public int fireBullet = 0;      //火属性子弹数量
    public int iceBullet = 0;       //冰属性子弹数量
    public int specialBomb = 0;     //特殊炸弹数量
    public int supplyNumber = 5;    //每次道具箱可获得子弹或道具数量

    public int respawnTime = 5;     //道具箱重生时间
    public int typeNumber = 3;      //可获得的道具种类个数

    public RectTransform bulletMessage; //子弹信息的UI
    public GameObject[] propPrefabs;    //道具预制体

    private RectTransform normalBulletImg;
    private RectTransform fireBulletImg;
    private RectTransform iceBulletImg;
    private RectTransform normalBombImg;
    private RectTransform specialBombImg;

    private bool isFireBullet = false;  //是否捡到火属性子弹
    private bool isIceBullet = false;   //是否捡到属性子弹
    private bool isSpecialBomb = false; //是否捡到遥控炸弹
    
    private Weapon weapon;          //武器脚本的引用

    // Use this for initialization
    void Start ()
    {
        normalBulletImg = bulletMessage.Find("NormalBullet") as RectTransform;
        fireBulletImg = bulletMessage.Find("FireBullet") as RectTransform;
        iceBulletImg = bulletMessage.Find("IceBullet") as RectTransform;
        normalBombImg = bulletMessage.Find("NormalBomb") as RectTransform;
        specialBombImg = bulletMessage.Find("SpecialBomb") as RectTransform;

        weapon = transform.Find("bone").Find("rh").Find("normal weapon").GetComponent<Weapon>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Propbox")
        {
            int index = Random.Range(0, typeNumber);
            GetProp(index);

            col.gameObject.SetActive(false);
            StartCoroutine(Respawn(col.gameObject));
        }
    }

    //道具箱的重生
    IEnumerator Respawn(GameObject go)
    {
        yield return new WaitForSeconds(respawnTime);
        go.SetActive(true);
    }

    //捡到各种子弹道具的逻辑
    public void GetProp(int index)
    {
        switch (index)
        {
            case 0:     //获得火属性子弹
                fireBullet += supplyNumber;
                iceBullet = 0;
                weapon.bulletPref = propPrefabs[0];
                isFireBullet = true;
                isIceBullet = false;
                iceBulletImg.gameObject.SetActive(false);
                fireBulletImg.gameObject.SetActive(true);
                break;
            case 1:     //获得冰属性子弹
                iceBullet += supplyNumber;
                fireBullet = 0;
                weapon.bulletPref = propPrefabs[1];
                isIceBullet = true;
                isFireBullet = false;
                fireBulletImg.gameObject.SetActive(false);
                iceBulletImg.gameObject.SetActive(true);
                break;
            case 2:     //获得遥控炸弹
                specialBomb += supplyNumber;
                normalBomb = 0;
                GetComponent<ThrowBomb>().bombPref = propPrefabs[2];
                isSpecialBomb = true;
                normalBombImg.gameObject.SetActive(false);
                specialBombImg.gameObject.SetActive(true);
                break;
            case 3:     //获得手雷
                normalBomb += supplyNumber;
                specialBomb = 0;
                GetComponent<ThrowBomb>().bombPref = propPrefabs[4];
                isSpecialBomb = false;
                specialBombImg.gameObject.SetActive(false);
                normalBombImg.gameObject.SetActive(true);
                break;
        }
        UpdateDisplay();
    }

    //物品栏UI显示更新
    public void UpdateDisplay()
    {
        //普通子弹数量更新
        normalBulletImg.Find("BulletText").GetComponent<Text>().text = "X" + normalBullet;

        //冰火属性子弹数量更新
        if (isFireBullet)
        {
            if (fireBullet > 0)
                //print(fireBulletImg.Find("Text"));
                fireBulletImg.Find("FireText").GetComponent<Text>().text = "X" + fireBullet;
            else
            {
                fireBulletImg.gameObject.SetActive(false);
                weapon.bulletPref = propPrefabs[3];
                isFireBullet = false;
            }
        }
        else if (isIceBullet)
        {
            if (iceBullet > 0)
                //print(iceBulletImg.Find("IceText"));
                iceBulletImg.Find("IceText").GetComponent<Text>().text = "X" + iceBullet;
            else
            {
                iceBulletImg.gameObject.SetActive(false);
                weapon.bulletPref = propPrefabs[3];
                isIceBullet = false;
            }
        }

        //投掷物数量更新
        if (isSpecialBomb)
        {
            if (specialBomb > 0)
                specialBombImg.Find("SpecialText").GetComponent<Text>().text = "X" + specialBomb;
            else
            {
                specialBombImg.gameObject.SetActive(false);
                isFireBullet = false;
            }
        }
        else
        {
            if (normalBomb > 0)
                normalBombImg.Find("BombText").GetComponent<Text>().text = "X" + normalBomb;
            else
                normalBombImg.gameObject.SetActive(false);
        }
    }
}
