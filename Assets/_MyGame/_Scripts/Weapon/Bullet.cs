using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;    //爆炸粒子效果
    public int damage = 40;         //子弹伤害
    public GameObject fireEffect;   //灼烧的粒子效果
    public GameObject iceEffect;    //冰冻的粒子效果
    
    //子弹类型
    public enum BulletType
    {
        Normal,
        Fire,
        Ice
    }
    public BulletType bulletType = BulletType.Normal;

	// Use this for initialization
	void Start () 
	{
		Destroy(gameObject, 2);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            OnExplode();
            Destroy(gameObject);
        }

        //无视和自身的碰撞
        if (col.tag == "Player" || col.tag == "Enemy")
        {
            OnExplode();
            Destroy(gameObject);
            switch (bulletType)
            {
                case BulletType.Fire:
                    col.transform.root.GetComponent<Health>().FireDebuff(damage, 5);
                    GameObject fireParticle = Instantiate(fireEffect, col.transform.position, Quaternion.identity);
                    fireParticle.transform.parent = col.transform;
                    Destroy(fireParticle, 5);
                    break;
                case BulletType.Ice:
                    col.transform.root.GetComponent<Health>().IceDebuff(damage, 3);
                    GameObject iceParticle = Instantiate(iceEffect, col.transform.position, Quaternion.identity);
                    iceParticle.transform.parent = col.transform;
                    Destroy(iceParticle, 3);
                    break;
                default:
                    col.transform.root.GetComponent<Health>().TakeDamage(damage);
                    break;
            }
        }
        else if (col.tag == "Carton")
        {
            col.GetComponent<Carton>().TakeDamage(damage);
            if (bulletType == BulletType.Fire)
            {
                GameObject fireParticle = Instantiate(fireEffect, col.transform.position, Quaternion.identity);
                fireParticle.transform.parent = col.transform;
                Destroy(fireParticle, 5);
            }

        }
    }

    void OnExplode()
    {
        //以随机的旋转量实例化一个爆炸效果
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Instantiate(explosion, transform.position, randomRotation);
    }
}
