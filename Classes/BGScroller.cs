using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeY ;

    public Vector3 startPosition;

	// Use this for initialization
	void Awake()
	{
        startPosition = transform.position;
	}
	
	void Start() 
    {
        //tileSizeY = (10.24f + 153.6f) / 2;
	}
	
	// Update is called once per frame
	void Update () 
    {
        

	}

    public void Scroll()
    {
        transform.position = new Vector3(startPosition.x, Mathf.Repeat(Time.time * scrollSpeed, tileSizeY /2f ) + startPosition.y, startPosition.z);
    }

    
}
