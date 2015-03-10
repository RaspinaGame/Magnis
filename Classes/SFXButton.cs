using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SFXButton : MonoBehaviour {

    public Sprite sfxSprite;
    public Sprite noSfxSprite;

    Image buttonImage;

    // Use this for initialization
    void Start()
    {

        buttonImage = GetComponent<Image>();

        if (PlayerPrefs.GetInt("sfx", 1) == 1)
        {
            buttonImage.sprite = sfxSprite;
            AudioListener.volume = 1;
        }
        else
        {
            buttonImage.sprite = noSfxSprite;
            AudioListener.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        SaveSystem.DeleteAll();

        if (PlayerPrefs.GetInt("sfx", 1) == 1)
        {
            PlayerPrefs.SetInt("sfx", 0);
            buttonImage.sprite = noSfxSprite;
            AudioListener.volume = 0;
        }
        else
        {
            PlayerPrefs.SetInt("sfx", 1);
            buttonImage.sprite = sfxSprite;
            AudioListener.volume = 1;
        }

        PlayerPrefs.Save();
    }
}
