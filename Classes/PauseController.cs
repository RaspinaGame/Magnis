using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseController : MonoBehaviour 
{

    public Animator PanleAnim;
    public RectTransform pauseButton;
    public RectTransform canvas;
    public Image pauseButtonImage;

    bool bIsPaused;
    AudioSource bgMusic;

	// Use this for initialization
	void Start () 
    {
//        print(Camera.main.WorldToScreenPoint(new Vector2(10, 0)));
        if (pauseButton != null)
            pauseButton.anchoredPosition = new Vector2(Camera.main.WorldToScreenPoint(new Vector2(10, 0)).x / canvas.localScale.x
                , Camera.main.WorldToScreenPoint(new Vector2(0, 16.5f)).y / canvas.localScale.y);

        bgMusic = GameObject.FindGameObjectWithTag("LevelController").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pause();
        }
    }

    void ZeroTimeScale()
    {
        Time.timeScale = 0;
    }

    public void OnMouseClick()
    {
        Resume();
    }

    void OnApplicationFocus(bool focus)
    {
       // print("OnApplicationFocus" + focus);
        if (!Application.isEditor)
        {
            Pause();
            AudioListener.pause = true;
            bgMusic.Play();
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (!Application.isEditor)
        {
            Pause();
            AudioListener.pause = true;
            bgMusic.Play();
        }
    }

    public void Pause()
    {
        if (GameInfo.IsLevelControllerPresent())
        {

            if (!GameInfo.IsPlayerDead() && !GameInfo.IsGamePaused() && !GameInfo.IsRollingBack())
            {
                GameInfo.PauseGame();
                DoPause();
            }
        }
        else
        {
            if (!bIsPaused)
            {
                DoPause();
            }
        }
    }

    public void Resume()
    {
        if (GameInfo.IsLevelControllerPresent())
        {
            if (GameInfo.IsGamePaused())
            {
                GameInfo.ResumeGame();
                DoResume();
            }
        }
        else
        {
            if (bIsPaused)
            {
                DoResume();
            }
        }
    }

    void DoPause()
    {
        AudioListener.pause = true;
        bgMusic.Play();
        PanleAnim.SetInteger("Pause", 1);
        Invoke("ZeroTimeScale", 0.15f);
        pauseButtonImage.enabled = false;
        bIsPaused = true;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    void DoResume()
    {
        PanleAnim.SetInteger("Pause", 0);
        Time.timeScale = 1;
        pauseButtonImage.enabled = true;
        AudioListener.pause = false;
        bIsPaused = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
