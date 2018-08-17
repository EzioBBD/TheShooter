using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{
	public bool destroyOnAwake;			
	public float awakeDestroyDelay;		
	public bool findChild = false;		
	public string namedChild;			

    public int damage = 80;

	void Start ()
	{
	    Destroy(gameObject, 0.1f);
	}
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Enemy")
            col.transform.root.GetComponent<Health>().TakeDamage(damage);
    }
}
