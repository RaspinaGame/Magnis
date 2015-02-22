using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeY ;

    public Vector3 startPosition;

    Transform cashTransform;

	// Use this for initialization
	void Awake()
	{
        cashTransform = transform;
        startPosition = cashTransform.localPosition;
	}
	
	void Start() 
    {
        //tileSizeY = (10.24f + 153.6f) / 2;
	}
	
	// Update is called once per frame
	void Update () 
    {
        

	}
    float RepeatTime;
    public void Scroll()
    {
        RepeatTime += Time.deltaTime;
      //  RepeatTime += Time.smoothDeltaTime;
        cashTransform.localPosition = new Vector3(startPosition.x, Mathf.Repeat(RepeatTime * scrollSpeed, tileSizeY / 2f) + startPosition.y, startPosition.z);
    }

    public void ScrollBack()
    {
        RepeatTime -= Time.deltaTime * 2;
      //  RepeatTime -= Time.smoothDeltaTime * 2;
        cashTransform.localPosition = new Vector3(startPosition.x, Mathf.Repeat(RepeatTime * scrollSpeed, tileSizeY / 2f) + startPosition.y, startPosition.z);
    }

    
}
