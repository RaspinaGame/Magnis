using UnityEngine;
using System.Collections;

public class Tut_FingerHeadAnimator : MonoBehaviour {

    public bool hold;
	Animator anim;
	// Use this for initialization
	void Start () 
    {
	    anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (hold)
        {
            anim.SetBool("hold",true); }

        else 
        { 
            anim.SetBool("hold", false);
        }
       
	}
}
