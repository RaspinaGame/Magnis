﻿using UnityEngine;
using System.Collections;

public class LevelSwitcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
            GameInfo.LevelFinished();
        }

    }
}
