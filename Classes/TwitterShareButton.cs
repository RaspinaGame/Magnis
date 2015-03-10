using UnityEngine;
using System.Collections;

public class TwitterShareButton : MonoBehaviour {

    string TwitterShareLink = "https://twitter.com/home?status=http://www.etarmies.com/";

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
        Application.OpenURL(TwitterShareLink);
    }
}
