using UnityEngine;
using System.Collections;

public class ExitController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Exit()
    {
        Time.timeScale = 1;
       // Application.LoadLevel("MainMenu");
      //  AudioListener.pause = false;
        AutoFade.LoadLevel("MainMenu", 0.4f, 0.6f, Color.black);
        
    }
}
