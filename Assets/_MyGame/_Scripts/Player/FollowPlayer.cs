using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	private Vector3 offset;			
	private Transform player;		

	void Start ()
	{
		player = transform.root.Find("groundCheck");
	    offset = transform.position - player.position;
	}

	void Update ()
	{
		transform.position = player.position + offset;
	}
}
