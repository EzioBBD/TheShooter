using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer line;  //枪口激光渲染器
    public float length = 20;   //激光长度

    // Use this for initialization
    void Start ()
	{
	    line = GetComponent<LineRenderer>();
	    line.SetVertexCount(2);//设置两点
	    line.SetColors(Color.red, Color.red); //设置直线颜色
    }
	 
	// Update is called once per frame
	void Update () 
	{
	    RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + transform.right.normalized * length);
        
	    line.SetPosition(0, transform.position);
	    if (hit)
	        line.SetPosition(1, hit.point);
        else
	        line.SetPosition(1, transform.position + transform.right.normalized * length);
    }
}
