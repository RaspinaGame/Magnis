﻿using UnityEngine;
using System.Collections;

public class FollowUsFacebookButton : MonoBehaviour {

    string FacebookLink = "https://www.facebook.com/";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Application.OpenURL(FacebookLink);
    }
}
