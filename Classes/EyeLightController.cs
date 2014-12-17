using UnityEngine;
using System.Collections;

public class EyeLightController : MonoBehaviour {

    public float intensity = 1;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Color tempColor;

        tempColor = renderer.material.color;
        tempColor.a = intensity;
        renderer.material.color = tempColor;
	
	}
}
