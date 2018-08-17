using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown : MonoBehaviour
{
    public int damage = 80;    //爆炸中心伤害
    public float radius = 2f;   //爆炸伤害半径
    public float delay = 3f;    //拉环后爆炸延迟
    public GameObject explosion;    //爆炸效果
    public bool isSpecial = false;  //是否是特殊炸弹

    public bool isPlayer = true;   //是玩家1：true，玩家2：false
    
    // Use this for initialization
    void Start () 
	{
        if (!isSpecial)
		    Destroy(gameObject, delay);
	}

    void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    public void Explode()
    {
        if (isPlayer)
            GameObject.Find("Player").GetComponent<PlayerController>().detonateFunc -= Explode;
        else
            GameObject.Find("Player2").GetComponent<PlayerController>().detonateFunc -= Explode;
        Destroy(gameObject);
    }
}
