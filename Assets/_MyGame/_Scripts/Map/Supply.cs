using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : MonoBehaviour
{
    public float respawnTime = 40f;     //重新生成时间
    public int bullet = 50;
    public int HP = 50;

    private bool isActive = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isActive && (col.tag == "Player" || col.tag == "Enemy"))
        {
            col.GetComponent<PropManager>().normalBullet += bullet;
            col.GetComponent<PropManager>().UpdateDisplay();
            col.GetComponent<Health>().TakeDamage(-HP);
            GetComponent<SpriteRenderer>().enabled = false;
            isActive = false;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        GetComponent<SpriteRenderer>().enabled = true;
        isActive = true;
    }
}
