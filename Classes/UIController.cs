using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{
    public Animator mainPanaleAnim;
    public int numberOfChapther = 3;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void loadLevel(int levelIndex)
    {
        int chapterIndex;
        print("loadLevel");
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
        chapterIndex = mainPanaleAnim.GetInteger("state");
        chapterIndex++;
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
            if (state+1 <= numberOfChapther)
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
    }
}
