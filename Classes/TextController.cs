using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () 
    {
	    text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void ChangeColor(float inAlphaColor)
    {
        if (text != null)
        {
            Color temp = text.color;
            temp.a = inAlphaColor;
            text.color = temp;
        }
    }
}
