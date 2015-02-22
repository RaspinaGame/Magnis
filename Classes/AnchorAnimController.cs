using UnityEngine;
using System.Collections;

public class AnchorAnimController : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () 
    {
	    anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.touchCount != 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                anim.SetBool("hold",true);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                anim.SetBool("hold", false);
            }
        }
        else 
        {
            if (Input.GetMouseButtonDown(0) )
            {
                anim.SetBool("hold", true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("hold", false);
            }
        }
       
	}
}
