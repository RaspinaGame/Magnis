using UnityEngine;
using System.Collections;

public class EyeLightController : MonoBehaviour {

    public float intensity = 1;

    Renderer eyeLightChild;
	// Use this for initialization
	void Start () 
    {
        if (transform.childCount > 0)
            eyeLightChild = transform.GetChild(0).GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
       // if(eyeLightChild != null)
        //    eyeLightChild.intensity = intensity;

        Color tempColor;
        tempColor = renderer.materials[0].color;
        tempColor.a = intensity;
        renderer.materials[0].color = tempColor;

        if (eyeLightChild != null)
        {
           // eyeLightChild.material.color = tempColor;
            eyeLightChild.materials[0].color = tempColor;
        }
	}
}
