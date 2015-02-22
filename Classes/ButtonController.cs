using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour 
{
    
    public int levelIndex;
    public int chapterIndex;
    bool bIsDemoVersion = false;
    public Image[] starImages;

    Button button;
    UIController uIController;
    Text text;

    ButtonController[] buttonControllers;

    public Sprite solidStarSprite;
 //   public Sprite emptyStarSprite;

    //public int[3] numberOfLevels;

	// Use this for initialization
	void Start () 
    {

        uIController = FindObjectOfType<UIController>();
        buttonControllers = transform.parent.GetComponentsInChildren<ButtonController>();
            //GetComponentsInParent<ButtonController>();

        for (int i = 0; i < buttonControllers.Length; i++)
        {
            if (buttonControllers[i] == this)
            {
                levelIndex = i;
                break;
            }
        }

        if (transform.parent.gameObject.name == "chapter1")
        {
            chapterIndex = 2;
        }
        else if (transform.parent.gameObject.name == "chapter2")
        {
            chapterIndex = 3;
        }
        else if (transform.parent.gameObject.name == "chapter3")
        {
            chapterIndex = 4;
        }
        else if (transform.parent.gameObject.name == "chapter4")
        {
            chapterIndex = 5;
        }
        else if (transform.parent.gameObject.name == "chapter5")
        {
            chapterIndex = 6;
        }


        RectTransform rectTransform = GetComponent<RectTransform>();

        text = GetComponentInChildren<Text>();
        if (levelIndex < 9)
        {
            text.text = "0" + (levelIndex + 1);
        }
        else 
        {
            text.text = "" + (levelIndex + 1);
        }

        
        button = GetComponent<Button>();

     //       print("ChapterReached :" + PlayerPrefs.GetInt("ChapterReached", 0));
     //   print("LevelReached : " + PlayerPrefs.GetInt("LevelReached", 0));
        if (PlayerPrefs.GetInt("ChapterReached", 2) < chapterIndex  && !bIsDemoVersion)
        {
            
            button.interactable = false;
            rectTransform.sizeDelta = new Vector2(100, 100);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        }
        else if (PlayerPrefs.GetInt("ChapterReached", 2) == chapterIndex && PlayerPrefs.GetInt("LevelReached", 0) < levelIndex && !bIsDemoVersion)
        {
            //button = GetComponent<Button>();
            button.interactable = false;
            rectTransform.sizeDelta = new Vector2(100, 100);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        }
        else 
        {
           // button = GetComponent<Button>();
            button.onClick.AddListener(() => { Onclick();});
            rectTransform.sizeDelta = new Vector2(120,120);
            ShowStars();
           // Button.ButtonClickedEvent.
        }
        // 

        if (uIController.numberOfLevels[chapterIndex - 2] <= levelIndex)
        {
            text.text = "-";
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
       
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void Onclick()
    {
        if (chapterIndex == 2 && levelIndex == 0)
        {
            AutoFade.LoadLevel("Intro",0.3f,2f,Color.black);
        }
        else 
        {
            uIController.loadLevel(levelIndex); 
        }
        
    }

    public void ShowStars()
    {
        
      //  starImages = GetComponentsInChildren<Image>();

        string s = "";
        s += chapterIndex;
        s += levelIndex;
        int saveStars = PlayerPrefs.GetInt(s, 0);

        for (int i = 0; i < starImages.Length; i++)
        {
            if (saveStars > i)
            {
                starImages[i].sprite = solidStarSprite;
            }
            //else
            //{
            //    starImages[i].sprite = emptyStarSprite;
            //}

            starImages[i].enabled = true;
        }
       
    }
}
