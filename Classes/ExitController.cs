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
        AutoFade.LoadLevel("MainMenu", 0.2f, 0.7f, Color.black);
    }
}
