using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carton : MonoBehaviour
{
    public int HP = 80;  //纸箱的耐久度
    
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            Destroy(gameObject);
    }
}
