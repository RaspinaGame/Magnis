using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public  float MovmentSpeed = 0.2f; 
	public 	float Xmax= 7, Xmin = -7;
	public 	float Ymax = 1000, Ymin = -1000;
    public LineRenderer anchorLine;
    public GameObject fingerHead;
    public GameObject anchorCenter;
    public GameObject anchorHead;
	
	private string DTouchWorldPosition;
	private string DFinalPosition;

	private Vector3 DesiredPosition;
	// Use this for initialization
	void Start () 
	{
        anchorCenter.transform.position = GetBasePosition();
	}

	Vector3 GetBasePosition()
	{
	
		//Camera MyCamera = FindObjectOfType<Camera> ();
		//return MyCamera.transform.position;
		//return transform.parent.position;
		return new Vector3(0,0,-2);
	}
	Vector3 GetInput()
	{
		if (Application.isEditor)
		{
			return Input.mousePosition ;
		}
		else if ( Input.touchCount > 0)
		{
			return Input.GetTouch(0).position ;
		}
		return new Vector3 (-1000, -1000, -1000);
	}
	void ProcessTouch(Vector3 InputPosition)
	{
		Vector3 FinalPosition, TouchWorldPosition, BasePosition;
		if ( InputPosition != new Vector3 (-1000, -1000, -1000) )
		{
            
			TouchWorldPosition = Camera.main.ScreenToWorldPoint(InputPosition);
            TouchWorldPosition.x = Mathf.Clamp(TouchWorldPosition.x, Xmin, Xmax);
            TouchWorldPosition.y = Mathf.Clamp(TouchWorldPosition.y, Ymin, Ymax);
            anchorLine.SetPosition(1, new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1));
            fingerHead.transform.position = new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1);
			BasePosition = GetBasePosition();
			TouchWorldPosition.z = -2;

			//FinalPosition = BasePosition -  TouchWorldPosition - transform.position;

			FinalPosition = BasePosition + BasePosition - TouchWorldPosition;
		//	FinalPosition.x = Mathf.Clamp (FinalPosition.x, Xmin, Xmax);
		//	FinalPosition.y = Mathf.Clamp (FinalPosition.y, Ymin, Ymax);

			//transform.Translate(FinalPosition.x , FinalPosition.y , 0,Space.World);
			DesiredPosition = FinalPosition;


			/// debug
			//		print (transform.parent.name + TouchWorldPosition  + BasePosition.ToString() + FinalPosition.ToString());
			DTouchWorldPosition = TouchWorldPosition.ToString();
			DFinalPosition = FinalPosition.ToString();
		}
		//
	}
	// Update is called once per frame
	void Update ()
	{
		if ( !GameInfo.IsGamePaused() )
		{
			ProcessTouch ( GetInput() );
			//print ( transform.position + " " + DesiredPosition + " " + MovmentSpeed );
         //   rigidbody2D.position = Vector3.Lerp(rigidbody2D.position, DesiredPosition, 0.1f);
            anchorLine.SetPosition(0, new Vector3(DesiredPosition.x, DesiredPosition.y, -1));
            anchorHead.transform.position = new Vector3(DesiredPosition.x, DesiredPosition.y, -1);
		}
		if (Input.GetKey (KeyCode.Escape)) 
			Application.LoadLevel (0);
	}

    void FixedUpdate()
    {
        if (!GameInfo.IsGamePaused())
        {
            MoveToDesiredPosition(DesiredPosition);
        }
 
    }

    void MoveToDesiredPosition(Vector3 inDesiredPosition ) 
    {
        Vector2 newposition = Vector3.Lerp(rigidbody2D.position, inDesiredPosition, MovmentSpeed);
       // rigidbody2D.position = Vector3.Lerp(rigidbody2D.position, inDesiredPosition, 0.1f);
        rigidbody2D.MovePosition(newposition);//= Vector3.Lerp(rigidbody2D.position, inDesiredPosition, 0.1f);
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "LevelSplitter") 
		{
			GameInfo.LevelFinished();
		}
        //else
        //{
				
        //}
		//foreach (ContactPoint2D contact in collision.contacts) 
		//{
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LevelSplitter")
        {
            GameInfo.LevelFinished();
        }
        else
        {
            GameInfo.PauseGame();
            Invoke("GameOver", 1.5f);
        }

    }



	void GameOver()
	{
		GameInfo.GameOver();
	}

	// Debug Part

	void OnGUI()
	{
		GUI.color = Color.white;
		
		GUIStyle _style = GUI.skin.GetStyle ("Label");
		_style.alignment = TextAnchor.UpperLeft;
		_style.fontSize = 20;
		GUI.Label (new Rect (20, 20, 200, 200), DTouchWorldPosition, _style);
		//print ("DTouchWorldPosition" + DTouchWorldPosition);
		_style.alignment = TextAnchor.MiddleCenter;
		GUI.Label (new Rect (0, 20, 200, 200), "DFinalPosition" + DFinalPosition, _style);
		//print ("DFinalPosition" + DFinalPosition);
		//_style.alignment = TextAnchor.MiddleCenter;
		//GUI.Label (new Rect (Screen.width - 320, 20, 200, 200), "LB(1):" + Input.touches.GetLowerBound (1) + " UB(0):" + Input.touches.GetUpperBound (1), _style);
	}
}
