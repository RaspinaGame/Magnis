using UnityEngine;
using System.Collections;

public class FacebookShareButton : MonoBehaviour {

    string FacebookShareLink = "https://www.facebook.com/sharer/sharer.php?u=http://raspina.co/";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        Application.OpenURL(FacebookShareLink);
    }
}
