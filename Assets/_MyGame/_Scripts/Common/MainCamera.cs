using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float moveSpeed = 5f;    //镜头移动速度

    private Transform player;
    private Transform player2;
    private Transform focusPlayer;
    private Vector3 focusPos;
    private bool gameOver = false;

    // Use this for initialization
    void Start ()
	{
	    player = GameObject.Find("Player").transform.Find("groundCheck");
	    player2 = GameObject.Find("Player2").transform.Find("groundCheck");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (gameOver)
	    {
	        focusPos = new Vector3(focusPlayer.position.x, focusPlayer.position.y + 2, -10);
            transform.position = Vector3.Lerp(transform.position, focusPos, 2 * moveSpeed * Time.deltaTime);
	        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3, moveSpeed * Time.deltaTime);
            return;
        }

        //根据双方玩家位置设置摄像机位置和大小，并平滑过渡
        Vector3 playerPos = player.position;
	    Vector3 enemyPos = player2.position;
	    Vector3 offset = playerPos - enemyPos;
	    Vector2 cameraXY = enemyPos + offset / 2;
	    Vector3 targetPos = new Vector3(cameraXY.x, cameraXY.y + 1, -10);
	    transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
	    float targetSize = Mathf.Max(1.2f * Mathf.Abs(offset.x), 2f * Mathf.Abs(offset.y)) * 0.32f;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, moveSpeed * Time.deltaTime);
	    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 12, 30);      //限制size不能过小
    }

    /// <summary>
    /// 摄像机聚焦于获胜方
    /// </summary>
    /// <param name="playerPos"></param>
    public IEnumerator CameraFocus(string tag)
    {
        if (!gameOver)
        {
            if (tag == "Player")
                focusPlayer = player2;
            else
                focusPlayer = player;
        }
        yield return new WaitForSeconds(1);
        gameOver = true;
    }
}
