using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	
	public GameObject StartPoint;
	public GameObject EndPoint;
	public GameObject []BackGrounds;
    public GameObject obstacle;
	private MoverComponent[] MoverChildren;

	private int LevelIndex = 0;
	private bool isPaused;
	public bool IsPaused
	{
		get
		{
			return isPaused;
		}
	}
	// Use this for initialization
	void Start () {

		GameInfo.SetLevelController (this);
		//direction.Normalize();
        MoverChildren = obstacle.transform.GetComponentsInChildren<MoverComponent>();
		Resetlevel ();
		//ChildPositions = new ArrayList ();	
		//foreach (MoverComponent CHPos in MoverChildren) 
		//{
		//	ChildPositions.Add( CHPos );
		//}		 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( !IsPaused )
		{
			if (MoverChildren.GetLength (0) != 0)
			{
				MoverChildren [LevelIndex].Move ();
			}
			foreach( GameObject BG in BackGrounds )
			{
                BGScroller MCBG = BG.GetComponent<BGScroller>();
                MCBG.Scroll();
			}
		}
		//else
		//{
			//transform.Translate (Direction  * Time.deltaTime);
		//}
		//foreach(MoverComponent MoverChild in MoverChildren  )
		//{
		//	MoverChild.Move();
		//}
		//transform.Translate (direction * Movementspeed * Time.deltaTime);
	}
	public void LevelFinished()
	{
		Debug.Log ("LevelFinished" + LevelIndex);
		MoverChildren [LevelIndex].LevelFinished (EndPoint.transform.position);
		LevelIndex = ( LevelIndex + 1 ) % ( MoverChildren.Length + 1 );
		Resetlevel();
	}
	//void OnMouseDown()
	//{
		//GUI.color = Color.white;
		
	//	GUIStyle _style = GUI.skin.GetStyle ("Label");
	//	_style.alignment = TextAnchor.LowerRight;
//		_style.fontSize = 20;
//		GUI.Label (new Rect (20, 20, 200, 200), "On MouseDown", _style);
//	}
	public void Resetlevel()
	{
		ResumeGame ();
		MoverChildren [LevelIndex].ResetLevel(StartPoint.transform.position);
	}

	public void PauseGame()
	{
		isPaused = true;
	}
	public void ResumeGame()
	{
		isPaused = false;
	}

}
