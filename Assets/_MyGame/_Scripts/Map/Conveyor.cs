using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float changeTime =0.8f;
    private bool isRight = true;
    private bool isMoving = false;
    private Transform player;

	// Use this for initialization
	void Start ()
	{
	    player = GameObject.Find("character1").transform;
	    StartCoroutine(ChangeDir());
	}
	
	// Update is called once per frame
	void Update () 
	{
	    if (isMoving)
	    {
	        if (isRight)
	            player.position = player.position + Vector3.right * Time.deltaTime;
	        else
	            player.position = player.position + Vector3.left * Time.deltaTime;
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            isMoving = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            isMoving = false;
        }
    }

    IEnumerator ChangeDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeTime);
            isRight = !isRight;
        }
    }
}
