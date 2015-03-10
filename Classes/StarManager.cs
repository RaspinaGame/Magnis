using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarManager : MonoBehaviour {

    Image[] starImages;
    Text text;

    public Sprite solidStarSprite;
    public Sprite emptyStarSprite;

    Animator anim;

    string[] starMessage = { "Lucky!", "Lucky!", "Good!", "Perfect!" };
    //string star2;
    //string star1;
    //string star0;

	// Use this for initialization
	void Start () {

        starImages = GetComponentsInChildren<Image>();
        text = GetComponentInChildren<Text>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowStars(int stars)
    {
        starImages[0].enabled = true;
        for (int i = 1; i < starImages.Length; i++)
        {
            //if (stars >= i)
            //{
            //    starImages[i].sprite = solidStarSprite;
            //}
            //else 
            //{
            //    starImages[i].sprite = emptyStarSprite;
            //}

            //starImages[i].enabled = true;
        }

        //for (int i = 0; i < texts.Length; i++)
        //{
        //    texts[i].enabled = true;
        //}

        text.text = starMessage[stars];
        text.enabled = true;

        anim.SetInteger("showStars", stars);

      //  Invoke("HideStars", 4);
    }

    public void HideStars()
    {
        //starImages[0].enabled = false;
        //for (int i = 1; i < starImages.Length; i++)
        //{
        //    starImages[i].enabled = false;
        //}

        //for (int i = 0; i < texts.Length; i++)
        //{
        //    texts[i].enabled = false;
        //}

        text.enabled = false;

      //  anim.SetTrigger("show");
        anim.SetInteger("showStars", 0);
    }

}
