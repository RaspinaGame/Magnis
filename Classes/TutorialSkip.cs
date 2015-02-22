using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialSkip : MonoBehaviour {

    public string levelName;
    public float doubleTapTime = 0.2f;
    bool bIsTaped;
    Text skipText;
    float tapTimeStamp;


	// Use this for initialization
	void Start () {

        tapTimeStamp = 0; 
        skipText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (bIsTaped)
            {
                if (tapTimeStamp > Time.time)
                {
                    AutoFade.LoadLevel(levelName, 2f, 0.6f, Color.black);
                }
                else
                {
                    tapTimeStamp = Time.time + doubleTapTime;
                }
            }
            else
            {
                skipText.enabled = true;
                bIsTaped = true;
            }
            
        }
	
	}
}
