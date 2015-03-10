using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public Animator mainPanaleAnim;
    public int numberOfChapther = 5;

    public Button leftButton;
    public Button rightButton;

    public int[] numberOfLevels;
	// Use this for initialization
	void Start () 
    {
        //print(PlayerPrefs.GetInt("ChapterReached", 1));
        //if (PlayerPrefs.GetInt("ChapterReached", 1) == 1)
        //{
        //    leftButton.interactable = false;
        //}
        //else if (PlayerPrefs.GetInt("ChapterReached", 1) == numberOfChapther)
        //{
        //    mainPanaleAnim.SetInteger("state", PlayerPrefs.GetInt("ChapterReached", 1));
        //    rightButton.interactable = false;
        //}
        //else
        //{
        //    mainPanaleAnim.SetInteger("state", PlayerPrefs.GetInt("ChapterReached", 1));
        //}

        Application.targetFrameRate = 60;
        AudioListener.pause = false;
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void loadLevel(int levelIndex)
    {
        int chapterIndex;
        print("loadLevel");
        PlayerPrefs.SetInt("SelectedLevelIndex", levelIndex);
        chapterIndex = mainPanaleAnim.GetInteger("state");
        chapterIndex+=2;
      //  Application.LoadLevel(chapterIndex);
        AutoFade.LoadLevel(chapterIndex, 0.3f, 0.6f, Color.black);
    }

    public void MainPanleShiftState(bool shiftRight)
    {
        print("MainPanleShiftState");
        int state;
        if (shiftRight)
        {
            state =  mainPanaleAnim.GetInteger("state");
            if (state+1 < numberOfChapther)
                state++;
            mainPanaleAnim.SetInteger("state", state);
        }
        else 
        {
            state = mainPanaleAnim.GetInteger("state");
            if (state > 0)
                state--;
            mainPanaleAnim.SetInteger("state", state);
        } 
        
        if (numberOfChapther == state+1)
            {
                rightButton.interactable = false;
                leftButton.interactable = true;
            }
        else if (0 == state)
        {
            rightButton.interactable = true;
            leftButton.interactable = false;
        }
        else
        {
            rightButton.interactable = true;
            leftButton.interactable = true;
        }

           
    }
}
