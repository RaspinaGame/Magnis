using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(BGAudioManager))]
public class LevelController : MonoBehaviour {
	
	//public GameObject StartPoint;
	public GameObject EndPoint;
    BGScroller[] BackGroundsScroller;
   // public GameObject mainCamera;

	public MoverComponent[] MoverChildren;
	public List<MoverComponent> MoverLevel;
   // public MoverComponent CameraMoverComponent;


    public GameObject BossManager;

  //  AudioSource BGAudioPlayer;

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

    public bool IsPlayerDead;

    BGAudioManager AudioManager;

    public GameObject anchorCenter;

    bool startLevelTriger;

    Transform cameraCashTransform;

    void Awake()
    {
        GameInfo.SetLevelController (this);
    }
    
    

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
       // PlayerPrefs.DeleteKey("ChapterReached");

        BackGroundsScroller = FindObjectsOfType<BGScroller>();

        AudioManager = GetComponent<BGAudioManager>();
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


        Startlevel();

        AudioManager.levelStart(LevelIndex);
		//ChildPositions = new ArrayList ();	
		//foreach (MoverComponent CHPos in MoverChildren) 
		//{
		//	ChildPositions.Add( CHPos );
		//}		
        PlayerPrefs.DeleteKey("LevelIndex");

        cameraCashTransform = Camera.main.transform;

        CreateBannerView();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!IsPaused)
        {
            if (MoverLevel.Count != 0)
            {
              //  CameraMoverComponent.Move();
                if (MoverLevel[LevelIndex].cashTransform.position.y - cameraCashTransform.position.y > MoverLevel[LevelIndex].offset.y)
                {
                    MoverLevel[LevelIndex].Move();
                    if (startLevelTriger)
                    {
                        LateStartLevel();
                        startLevelTriger = false;
                    }
                }
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
                MoverLevel[LevelIndex].RollBack();
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

            if (PlayerPrefs.GetInt("ChapterReached", 2) >= Application.loadedLevel && PlayerPrefs.GetInt("LevelReached", 0) < LevelIndex)
            {
                PlayerPrefs.SetInt("LevelReached", LevelIndex);
                PlayerPrefs.Save();
            }
            Startlevel();
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
        MoverLevel[LevelIndex].ResetLevel(Camera.main.transform.position + (Vector3)MoverLevel[LevelIndex].offset, LevelIndex);
        MoverLevel[LevelIndex].gameObject.SetActive(true);
        MoverLevel[LevelIndex].BroadcastMessage("TriggerOnRestartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TriggerOnRestartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
	}

    public void Startlevel()
    {
        ResumeGame();
        isRollingBack = false;
        MoverLevel[LevelIndex].ResetLevel(Camera.main.transform.position, LevelIndex);
        MoverLevel[LevelIndex].gameObject.SetActive(true);
        MoverLevel[LevelIndex].BroadcastMessage("TriggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TriggerOnStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);

        startLevelTriger = true;
        AudioManager.levelStart(LevelIndex);

        System.GC.Collect();
    }


    public void LateStartLevel()
    {
        MoverLevel[LevelIndex].BroadcastMessage("TriggerOnLateStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TriggerOnLateStartLevel", LevelIndex, SendMessageOptions.DontRequireReceiver);
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

        System.GC.Collect();
    }
	public void ResumeGame()
	{
		isPaused = false;

        MoverLevel[LevelIndex].BroadcastMessage("TtiggerGameIsResume", LevelIndex, SendMessageOptions.DontRequireReceiver);
        if (BossManager != null)
            BossManager.BroadcastMessage("TtiggerGameIsResume", LevelIndex, SendMessageOptions.DontRequireReceiver);

       // System.GC.Collect();
	}

    void LoadNextScene()
    {
       // check to see if there is any level left to load
        if (Application.levelCount > Application.loadedLevel + 1)
        {
            int chapterIndex = Application.loadedLevel + 1;

            PlayerPrefs.SetInt("LevelIndex", 0);
            if (PlayerPrefs.GetInt("ChapterReached", 2) < chapterIndex)
            {

                PlayerPrefs.SetInt("ChapterReached", chapterIndex);
                PlayerPrefs.SetInt("LevelReached", 0);
            }
            PlayerPrefs.Save();
           // Application.LoadLevel(levelIndex);
            AutoFade.LoadLevel(chapterIndex, 1.5f, 1.5f, new Color(0f, 0.1f, 0.1f));
        }
            
        else 
        {
            //Game is finished !!!
            AutoFade.LoadLevel("MainMenu", 1.5f, 0.7f, new Color(0f, 0.1f, 0.1f));

        }
    }

    public void PlayerDide()
    {
        IsPlayerDead = true;
    }

    public void SaveStars(int stars)
    {
        int chapterIndex = Application.loadedLevel;

        string s = "";
        s += chapterIndex;
        s += LevelIndex;

        int saveStars = PlayerPrefs.GetInt(s,0);

        if (saveStars < stars)
        {
            PlayerPrefs.SetInt(s, stars);
        }
    }

    public static void CreateBannerView()
    {
     //   string verticalPosition = "top"; // "top" or "bottom"
     //   string horizontalPosition = "center"; // "left", "right" or "center"
     //   AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
     //   AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
     //   AndroidJavaClass pluginClass = new AndroidJavaClass("ir.adad.AdadUnityPlugin");
     ////   pluginClass.CallStatic("setDisabled", new object[1] { "true" });
     //   pluginClass.CallStatic("createAdView",
     //                          new object[3] { activity, verticalPosition, horizontalPosition });
    } 

    //void OnGUI()
    //{
    //    GUI.color = Color.white;

    //    GUIStyle _style = GUI.skin.GetStyle("Label");
    //    _style.alignment = TextAnchor.UpperLeft;
    //    _style.fontSize = 20;

       
    //    GUI.Label(new Rect(20, 100, 200, 200), "Level : " + (LevelIndex+1));
     
    //}

}