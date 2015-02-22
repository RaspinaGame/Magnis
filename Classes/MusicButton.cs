using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour {

    public Sprite musicSprite;
    public Sprite noMusicSprite;

    Image buttonImage;
    AudioSource bgMusic;

	// Use this for initialization
	void Start () {
	
        buttonImage = GetComponent<Image>();
        if (GameInfo.IsLevelControllerPresent())
        {
            bgMusic = GameObject.FindGameObjectWithTag("LevelController").GetComponent<AudioSource>();
        }
        else
        {
            bgMusic = GameObject.FindGameObjectWithTag("bgmusic").GetComponent<AudioSource>();
        }

        bgMusic.ignoreListenerVolume = true;

        if (PlayerPrefs.GetInt("music", 1) == 1)
        {
            buttonImage.sprite = musicSprite;
            bgMusic.mute = false;
        }
        else 
        {
            buttonImage.sprite = noMusicSprite;
            bgMusic.mute = true;
        }

      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void OnClick()
    {
        if (PlayerPrefs.GetInt("music", 1) == 1)
        {
            PlayerPrefs.SetInt("music", 0);
            buttonImage.sprite = noMusicSprite;
            bgMusic.mute = true;
        }
        else
        {
            PlayerPrefs.SetInt("music", 1);
            buttonImage.sprite = musicSprite;
            bgMusic.mute = false;
        }

        PlayerPrefs.Save();
    }
}
