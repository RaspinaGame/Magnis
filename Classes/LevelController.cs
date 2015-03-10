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

        Application.targetFrameRate = 60;
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

        LevelIndex = PlayerPrefs.GetInt("SelectedLevelIndex", LevelIndex);


        Startlevel();

        AudioManager.levelStart(LevelIndex);
		//ChildPositions = new ArrayList ();	
		//foreach (MoverComponent CHPos in MoverChildren) 
		//{
		//	ChildPositions.Add( CHPos );
		//}		
        PlayerPrefs.DeleteKey("SelectedLevelIndex");

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

            if (SaveSystem.GetInt("ChapterReached", 2) >= Application.loadedLevel && SaveSystem.GetInt("LevelReached", 0) < LevelIndex)
            {
                SaveSystem.SetInt("LevelReached", LevelIndex);
                SaveSystem.Save();
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

            SaveSystem.SetInt("SelectedLevelIndex", 0);
            if (SaveSystem.GetInt("ChapterReached", 2) < chapterIndex)
            {

                SaveSystem.SetInt("ChapterReached", chapterIndex);
                SaveSystem.SetInt("LevelReached", 0);
            }
           
           // Application.LoadLevel(levelIndex);
            AutoFade.LoadLevel(chapterIndex, 1.5f, 1.5f, new Color(0f, 0.1f, 0.1f));

            //Achievement chapter1
            if (chapterIndex == 3)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.AchievementChapter1);
            }
            //Achievement chapter2
            else if (chapterIndex == 4)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.AchievementChapter2);
            }
            //Achievement chapter3
            else if (chapterIndex == 5)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.AchievementChapter3);
            }
            //Achievement chapter4
            else if (chapterIndex == 6)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.AchievementChapter4);
            }
            //Achievement chapter5
            else if (chapterIndex == 7)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.AchievementChapter5);
            }
            
            SaveSystem.Save();
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

        int saveStars = SaveSystem.GetInt(s,0);

        if (saveStars < stars)
        {
            SaveSystem.SetInt(s, stars);

            int totalStars = SaveSystem.GetInt("totalStars",0);
            totalStars += (stars - saveStars);
            SaveSystem.SetInt("totalStars", totalStars);

            //LeaderBoard
            GameCenterIntegration.ReportScore(totalStars);

            //Achievement allstars
            if (totalStars >= 153)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.AchievementAllstars);
            }
            //Achievement 120stars
            else if (totalStars >= 120)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.Achievement120stars);
            }
            //Achievement 60stars
            else if (totalStars >= 60)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.Achievement60stars);
            }
            //Achievement 30stars
            else if (totalStars >= 30)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.Achievement30stars);
            }
            //Achievement  : 1stStep
            else if (Application.loadedLevel == 2 && LevelIndex == 0)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.Achievement1stStep);
            }
            //Achievement  : 3stars
            if (stars == 3)
            {
                GameCenterIntegration.ReportProgress(GameCenterIntegration.Achievement3stars);
            }
            
            
           
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