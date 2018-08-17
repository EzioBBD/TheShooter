using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float elasticForce = 700;    //弹力
    private bool isCompressed = false;    //是否被压缩
    private bool isStretching = false;
    
	// Update is called once per frame
	void Update () 
	{
	    if (isCompressed)
	    {
	        float scaleY = Mathf.Lerp(transform.localScale.y, 0.4f, 20 * Time.deltaTime);
	        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
	        if (scaleY - 0.4 < 0.01f)
	        {
	            isCompressed = false;
	            isStretching = true;
	        }
	    }

	    if (isStretching)
	    {
	        float scaleY = Mathf.Lerp(transform.localScale.y, 1f, 20 * Time.deltaTime);
	        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
            if (1 - scaleY < 0.000001f)
	            isStretching = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, elasticForce));
        isCompressed = true;
    }
}
