using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseController : MonoBehaviour 
{

    public Animator PanleAnim;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameInfo.IsPlayerDead())
            {
                if (GameInfo.IsGamePaused())
                {
                   //  GameInfo.ResumeGame();
                   // PanleAnim.SetInteger("Pause", 0);
                    // Time.timeScale = 1;
                }
                else
                {
                    if (GameInfo.IsRollingBack())
                    {
                        //
                    }
                    else
                    {
                        GameInfo.PauseGame();
                        PanleAnim.SetInteger("Pause", 1);
                        Invoke("ZeroTimeScale",0.15f);
                        
                    }
                     
                }
            }
        }
    }

    void ZeroTimeScale()
    {
        Time.timeScale = 0;
    }

    public void OnMouseClick()
    {
        if (!GameInfo.IsPlayerDead())
        {
            if (GameInfo.IsGamePaused())
            {
                GameInfo.ResumeGame();
                PanleAnim.SetInteger("Pause", 0);
                Time.timeScale = 1;
            }
            else
            {
                if (GameInfo.IsRollingBack())
                {
                    //
                }
                else
                {
          //          GameInfo.PauseGame();
          //          PanleAnim.SetInteger("Pause", 1);
                }
            }
        }
    }



}
