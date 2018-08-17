using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject player1;      //玩家1的引用
    public GameObject player2;      //玩家2的引用
    public GameObject UI_BG;
    public GameObject menuBG;

    private bool isStarted = false;
    private bool display = false;
    private bool startTextDisplay = true;
    private bool bgScroll = false;

    private float startTime;
    private Transform background;
    private Text startText;
    
    // Use this for initialization
    void Start ()
    {
        startText = transform.Find("StartText").GetComponent<Text>();
        background = GameObject.Find("Map").transform.Find("BG");
    }
	
	// Update is called once per frame
	void Update () 
	{
	    if (!isStarted)
	    {
	        if (startTextDisplay)
	        {
	            startText.color = Color.Lerp(startText.color, Color.clear, Time.deltaTime * 2);
	            if (startText.color.a < 0.01f)
	                startTextDisplay = false;
	        }  
            else
            {
                startText.color = Color.Lerp(startText.color, Color.white, Time.deltaTime * 2);
                if (startText.color.a > 0.99f)
                    startTextDisplay = true;
            }

	        if (Input.GetButtonDown("Fire1"))
	        {
                transform.Find("StartText").gameObject.SetActive(false);
	            bgScroll = true;
	            isStarted = true;
	        }

            if (Input.GetButtonDown("Cancel"))
                Application.Quit();
	    }
	    else if (Input.GetButtonDown("Cancel"))
	    {
	        if (display)
	        {
	            menuBG.SetActive(false);
	            Time.timeScale = 1;
	            player1.GetComponent<PlayerController>().enabled = true;
	            player2.GetComponent<PlayerController>().enabled = true;
	        }
	        else
	        {
	            menuBG.SetActive(true);
	            Time.timeScale = 0;
	            player1.GetComponent<PlayerController>().enabled = false;
	            player2.GetComponent<PlayerController>().enabled = false;
	        }
	        display = !display;
	    }

	    if (bgScroll)
	    {
	        background.position = Vector3.Lerp(background.position, new Vector3(0, 26.8f, 0), Time.deltaTime * 3);
	        if (background.position.y > 26.5f)
	        {
                InitGame();
	            bgScroll = false;
            }
	    }
    }

    public void OnStartBtnClick()
    {
        Time.timeScale = 1;
        display = false;
        menuBG.SetActive(false);
        player1.GetComponent<PlayerController>().enabled = true;
        player2.GetComponent<PlayerController>().enabled = true;
    }

    public void OnExitBtnClick()
    {
        Application.Quit();
    }

    void InitGame()
    {
        player1.SetActive(true);
        player2.SetActive(true);
        Camera.main.GetComponent<MainCamera>().enabled = true;
        UI_BG.SetActive(true);
    }
}
