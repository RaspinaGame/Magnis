using UnityEngine;
using System.Collections;

public class SuicideController : MonoBehaviour {

    public Animator PanleAnim;
    PlayerController playerController;
	// Use this for initialization
	void Start () 
    {
        playerController = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Suicide()
    {
        GameInfo.ResumeGame();
        PanleAnim.SetInteger("Pause", 0);
        Time.timeScale = 1;
        playerController.Die("Suicide");
        
    }
}
