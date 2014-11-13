using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
	
	public GameObject StartPoint;
	public GameObject EndPoint;
	public GameObject[] BackGrounds;

	public MoverComponent[] MoverChildren;
	public List<MoverComponent> MoverLevel;

    public GameObject BossManager;

	public int LevelIndex = 0;
    private bool isRollingBack;
    public bool IsRollingBack
    {
        get
        {
            return isRollingBack;
        }
    }
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

		MoverLevel = new List<MoverComponent> ();
		MoverChildren = transform.GetComponentsInChildren<MoverComponent> ();
		foreach (MoverComponent MC in MoverChildren)
		{
			MoverComponent[] TempMoverComponent;
		
			TempMoverComponent = MC.gameObject.GetComponentsInParent<MoverComponent>();
			if ( TempMoverComponent.Length == 1 )
			{
				MoverLevel.Add(MC);
			}
			else
			{
				TempMoverComponent[1].AddToMoverChildrenObstacle(MC);
			}
		}

        foreach (MoverComponent MC in MoverLevel)
        {
            MC.gameObject.SetActive(false);
        }		

		Resetlevel();
		//ChildPositions = new ArrayList ();	
		//foreach (MoverComponent CHPos in MoverChildren) 
		//{
		//	ChildPositions.Add( CHPos );
		//}		 
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!IsPaused)
        {
            if (MoverLevel.Count != 0)
            {
                MoverLevel[LevelIndex].Move();
            }
            foreach (GameObject BG in BackGrounds)
            {
                BGScroller MCBG = BG.GetComponent<BGScroller>();
                MCBG.Scroll();
            }
        }
        else if (IsRollingBack)
        {
            if (MoverLevel.Count != 0)
            {
                MoverLevel[LevelIndex].MoveBack(StartPoint.transform.position);
            }
            foreach (GameObject BG in BackGrounds)
            {
                BGScroller MCBG = BG.GetComponent<BGScroller>();
                MCBG.Scroll();
            }
        }
       // else 
        //{
       //     MoverLevel[LevelIndex].BroadcastMessage("TtiggerGameIsPused", SendMessageOptions.DontRequireReceiver);
       // }
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
        MoverLevel[LevelIndex].gameObject.SetActive(false);
        if (LevelIndex + 1 == MoverLevel.Count)
        {
            LoadNextScene();
        }
        else
        { 
            MoverLevel [LevelIndex].LevelFinished (EndPoint.transform.position);
		    LevelIndex = ( LevelIndex + 1 ) % ( MoverLevel.Count );
		    Resetlevel();
        }
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
        isRollingBack = false;
        MoverLevel[LevelIndex].gameObject.SetActive(true);
        MoverLevel[LevelIndex].ResetLevel(StartPoint.transform.position, LevelIndex);
        MoverLevel[LevelIndex].BroadcastMessage("TtiggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
	}

	public void PauseGame()
	{
		isPaused = true;
        MoverLevel[LevelIndex].BroadcastMessage("TtiggerGameIsPused", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerGameIsPused", LevelIndex, SendMessageOptions.DontRequireReceiver);
	}
    public void RollOver()
	{
        isRollingBack = true;
        MoverLevel[LevelIndex].BroadcastMessage("TtiggerRollBack", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerRollBack", LevelIndex, SendMessageOptions.DontRequireReceiver);
    }
	public void ResumeGame()
	{
		isPaused = false;
	}

    void LoadNextScene()
    {
       // check to see if there is any level left to load
        if (Application.levelCount > Application.loadedLevel + 1)
        {
            int levelIndex = Application.loadedLevel + 1;
            Application.LoadLevel(levelIndex);
        }
            
        else 
        {
            //Game is finished !!!
        }
    }

}