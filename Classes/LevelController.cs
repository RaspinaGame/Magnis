using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
	
	public GameObject StartPoint;
	public GameObject EndPoint;
    public BGScroller[] BackGroundsScroller;
   // public GameObject mainCamera;

	public MoverComponent[] MoverChildren;
	public List<MoverComponent> MoverLevel;
   // public MoverComponent CameraMoverComponent;


    public GameObject BossManager;

	public int LevelIndex = 0;
    public int editorLevelIndex = 0;
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

    public bool IsPlayerDead;

	// Use this for initialization
	void Start () {
       // PlayerPrefs.DeleteKey("ChapterReached");
		GameInfo.SetLevelController (this);

       // CameraMoverComponent = mainCamera.GetComponent<MoverComponent>();
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

        LevelIndex = PlayerPrefs.GetInt("LevelIndex", LevelIndex);

        if (editorLevelIndex != 0)
        {
            LevelIndex = editorLevelIndex;
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
              //  CameraMoverComponent.Move();
               MoverLevel[LevelIndex].Move();
            }
            foreach (BGScroller BG in BackGroundsScroller)
            {
                //BGScroller MCBG = BG.GetComponent<BGScroller>();
                BG.Scroll();
            }
        }
        else if (IsRollingBack)
        {
            if (MoverLevel.Count != 0)
            {
             //   MoverLevel[LevelIndex].MoveBack(StartPoint.transform.position);
            }
            foreach (BGScroller BG in BackGroundsScroller)
            {
                BG.ScrollBack();
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

            if (PlayerPrefs.GetInt("ChapterReached", 1) >= Application.loadedLevel && PlayerPrefs.GetInt("LevelReached", 0) < LevelIndex)
            {
                PlayerPrefs.SetInt("LevelReached", LevelIndex);
                PlayerPrefs.Save();
            }
            ResetNextlevel();
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
        IsPlayerDead = false;
		ResumeGame ();
        isRollingBack = false;
        MoverLevel[LevelIndex].gameObject.SetActive(true);
        MoverLevel[LevelIndex].ResetLevel(StartPoint.transform.position, LevelIndex);
        MoverLevel[LevelIndex].BroadcastMessage("TtiggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
	}

    public void ResetNextlevel()
    {
        ResumeGame();
        isRollingBack = false;
        MoverLevel[LevelIndex].gameObject.SetActive(true);
        MoverLevel[LevelIndex].ResetLevel(StartPoint.transform.position, LevelIndex);
        MoverLevel[LevelIndex].BroadcastMessage("TtiggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerOnStartNextLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
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

        MoverLevel[LevelIndex].BroadcastMessage("TtiggerGameIsResume", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerGameIsResume", LevelIndex, SendMessageOptions.DontRequireReceiver);
	}

    void LoadNextScene()
    {
       // check to see if there is any level left to load
        if (Application.levelCount > Application.loadedLevel + 1)
        {
            int levelIndex = Application.loadedLevel + 1;

            if (PlayerPrefs.GetInt("ChapterReached", 1) < levelIndex)
            {
                PlayerPrefs.SetInt("LevelIndex", 0);
                PlayerPrefs.SetInt("ChapterReached", levelIndex);
                PlayerPrefs.SetInt("LevelReached", 0);
                PlayerPrefs.Save();
            }

           // Application.LoadLevel(levelIndex);
            AutoFade.LoadLevel(levelIndex, 0.5f, 0.7f, Color.black);
        }
            
        else 
        {
            //Game is finished !!!
        }
    }

    public void PlayerDide()
    {
        IsPlayerDead = true;
    }

    void OnGUI()
    {
        GUI.color = Color.white;

        GUIStyle _style = GUI.skin.GetStyle("Label");
        _style.alignment = TextAnchor.UpperLeft;
        _style.fontSize = 20;

       
        GUI.Label(new Rect(20, 100, 200, 200), "Level : " + (LevelIndex+1));
     
    }

}