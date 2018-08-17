using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public static Console Instance { get; set; }    //控制台的单例
    public float coldTime = 60f;        //冷却时间
    public Vector3 endPos;          //河流上升的最大高度
    public float speed = 1f;        //河流上升速度

    private Transform river;       //地底河流
    private bool canInteract = false;
    private bool isUp = false;
    private bool isDown = false;
    private bool isColdDown = false;
    private Vector3 startPos;
    
    private float startTime;

    // Use this for initialization
    void Start ()
    {
        Instance = this;
        river = GameObject.Find("Map").transform.Find("River");
        startPos = river.position;
    }
	
	// Update is called once per frame
	void Update () 
	{
	    if (isUp)
	    {
	        river.position = Vector3.Lerp(startPos, endPos, (Time.time - startTime) * 0.1f * speed);
            print(river.position.y);
	        if (endPos.y - river.position.y < 0.00001f)
	        {
	            isUp = false;
	            isDown = true;
	            startTime = Time.time;
	        }
        }
	    if (isDown)
	    {
	        float posY = Mathf.Lerp(endPos.y, startPos.y, (Time.time - startTime) * 0.2f * speed);
	        river.position = new Vector3(0, posY, 0);
	        //river.position = Vector3.Lerp(endPos, startPos, Time.time * 0.1f);
	        //if (river.position.y - startPos.y < 0.00001f)
	        //    isDown = false;
	    }
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Enemy")
            canInteract = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Enemy")
            canInteract = false;
    }

    public void ConsoleInteract()
    {
        if (canInteract && !isColdDown)
        {
            isUp = true;
            startTime = Time.time;
            StartCoroutine(ColdDown());
        }
    }

    IEnumerator ColdDown()
    {
        isColdDown = true;
        yield return new WaitForSeconds(coldTime);
        isColdDown = false;
    }
}
