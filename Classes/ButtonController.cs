using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour 
{
    
    public int levelIndex;
    public int chapterIndex;

    Button button;
    UIController uIController;
    Text text;

    ButtonController[] buttonControllers;

	// Use this for initialization
	void Start () 
    {

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
            chapterIndex = 1;
        }
        else if (transform.parent.gameObject.name == "chapter2")
        {
            chapterIndex = 2;
        }
        else if (transform.parent.gameObject.name == "chapter3")
        {
            chapterIndex = 3;
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
        
        

     //       print("ChapterReached :" + PlayerPrefs.GetInt("ChapterReached", 0));
     //   print("LevelReached : " + PlayerPrefs.GetInt("LevelReached", 0));
        if (PlayerPrefs.GetInt("ChapterReached", 1) < chapterIndex )
        {
            button = GetComponent<Button>();
            button.interactable = false;
            rectTransform.sizeDelta = new Vector2(100, 100);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        }
        else if (PlayerPrefs.GetInt("ChapterReached", 1) == chapterIndex && PlayerPrefs.GetInt("LevelReached", 0) < levelIndex)
        {
            button = GetComponent<Button>();
            button.interactable = false;
            rectTransform.sizeDelta = new Vector2(100, 100);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        }
        else 
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => { Onclick();});
            rectTransform.sizeDelta = new Vector2(120,120);
           // Button.ButtonClickedEvent.
        }
        // 

       
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void Onclick()
    {
        uIController = FindObjectOfType<UIController>();
        uIController.loadLevel(levelIndex);
    }
    

}
