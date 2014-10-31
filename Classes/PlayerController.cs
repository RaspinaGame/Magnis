using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public  float MovmentSpeed = 0.1f; 
	public 	float Xmax= 7.0f, Xmin = -7;
	public 	float Ymax = 1000, Ymin = -1000;
	
	private string DTouchWorldPosition;
	private string DFinalPosition;

	private Vector3 DesiredPosition;
	// Use this for initialization
	void Start () 
	{
	}

	Vector3 GetBasePosition()
	{
	
		//Camera MyCamera = FindObjectOfType<Camera> ();
		//return MyCamera.transform.position;
		//return transform.parent.position;
		return new Vector3(0,0,0);
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
			BasePosition = GetBasePosition();
			TouchWorldPosition.z = 0;

			//FinalPosition = BasePosition -  TouchWorldPosition - transform.position;

			FinalPosition = BasePosition + BasePosition - TouchWorldPosition;
			FinalPosition.x = Mathf.Clamp (FinalPosition.x, Xmin, Xmax);
			FinalPosition.y = Mathf.Clamp (FinalPosition.y, Ymin, Ymax);

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
			print ( transform.position + " " + DesiredPosition + " " + MovmentSpeed + " " + Xmax);
			transform.position = Vector3.Lerp (transform.position, DesiredPosition, MovmentSpeed);
		}
		if (Input.GetKey (KeyCode.Escape)) 
			Application.LoadLevel (0);
	}



	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.name == "LevelSplitter") 
		{
			GameInfo.LevelFinished();
		}
		else
		{
			GameInfo.PauseGame();
			Invoke("GameOver",1.5f);	
		}
		//foreach (ContactPoint2D contact in collision.contacts) 
		//{
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//}
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
