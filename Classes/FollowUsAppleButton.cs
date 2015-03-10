using UnityEngine;
using System.Collections;

public class FollowUsAppleButton : MonoBehaviour {

    string AppleStoreLink = "http://store.apple.com/us";

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
        Application.OpenURL(AppleStoreLink);
    }
}
