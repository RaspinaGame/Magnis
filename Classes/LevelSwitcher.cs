using UnityEngine;
using System.Collections;

public class LevelSwitcher : MonoBehaviour {

    PlayerController player;

	// Use this for initialization
	void Start () {

        player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "LevelSplitter")
        {
            //other.
            //collider2D.
            player.LevelFinished();
      //      GameInfo.LevelFinished();
        }
        else if (other.gameObject.name == "ShowStars")
        {
            player.ShowStarsReached();
            //other.
            //collider2D.
          //  GameInfo.LevelFinished();
        }

    }
}
