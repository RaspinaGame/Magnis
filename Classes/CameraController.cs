using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization

    public GameObject[] sideBlackQuad;

	void Start () 
    {
        float screenAspect;
        screenAspect = (float)Screen.width / Screen.height;
   //     camera.aspect = 1.875f / 3f;
        if (screenAspect < 1.875f / 3f)
        {
            //saman
            float xFactor = Screen.width / 800f;
            float yFactor = Screen.height / 1280f;
            Camera.main.camera.orthographicSize *= yFactor / xFactor;
            for (int i = 0; i < sideBlackQuad.Length; i++)
            {
                sideBlackQuad[i].SetActive(true);
            }
            

        }
        else
        {
            for (int i = 0; i < sideBlackQuad.Length; i++)
            {
                sideBlackQuad[i].SetActive(false);
            }
        }
      //  Camera.main.rect = new Rect(0, 0,  yFactor/xFactor , 1);
    //    Camera.main.WorldToScreenPoint(new Vector2(10, 0));

    //     Camera.main.WorldToScreenPoint(new Vector2(10, 0)).x / (Screen.width / 800);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //void OnGUI()
    //{
    //    GUI.color = Color.white;

    //    GUIStyle _style = GUI.skin.GetStyle("Label");
    //    _style.alignment = TextAnchor.UpperLeft;
    //    _style.fontSize = 20;


    //    GUI.Label(new Rect(20, 500, 400, 200), "width : " + Screen.width + "height : " + Screen.height);
    //    GUI.Label(new Rect(20, 550, 400, 200), "" + Camera.main.WorldToScreenPoint(new Vector2(10, 0)) );
    //    GUI.Label(new Rect(20, 600, 400, 200), "" + Camera.main.WorldToScreenPoint(new Vector2(-10, 0)));
    //    //print ("DTouchWorldPosition" + DTouchWorldPosition);
    //    //_style.alignment = TextAnchor.MiddleCenter;
    //    //    GUI.Label(new Rect(20, 40, 200, 200), "Contact : " + bIsInContact + bIsContactWithWall);
    //    //  GUI.Label(new Rect(20, 60, 200, 200), "scaleState : " + scaleState);
    //    //print ("DFinalPosition" + DFinalPosition);
    //    //_style.alignment = TextAnchor.MiddleCenter;
    //    //GUI.Label (new Rect (Screen.width - 320, 20, 200, 200), "LB(1):" + Input.touches.GetLowerBound (1) + " UB(0):" + Input.touches.GetUpperBound (1), _style);
    //}
}
