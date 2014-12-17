using UnityEngine;
using System.Collections;

public class GameInfo : MonoBehaviour {

	private static GameInfo instance = null;
	private static LevelController levelController;

	public static GameInfo Instance
	{
		get
		{
			if (instance == null) instance = new GameObject ("GameInfo").AddComponent<GameInfo>(); //create game manager object if required
			return instance;
		}
	}

	void Awake()
	{
		//Check if there is an existing instance of this object
		if(instance)
			DestroyImmediate(gameObject); //Delete duplicate
		else
		{
			instance = this; //Make this object the only instance
			DontDestroyOnLoad (gameObject); //Set as do not destroy
		}

//        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	// Use this for initialization
	void Start () 
	{
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public static void SetLevelController(LevelController LC)
	{
		levelController = LC;
	}

	public static void LevelFinished()
	{
		levelController.LevelFinished();
	}

    //public static void SceneFinished()
    //{
    //    levelController.SceneFinished();
    //}

	public static void GameOver()
	{
		levelController.Resetlevel();
	}
	public static void PauseGame()
	{
		levelController.PauseGame ();
	}
    public static void RollOver()
	{
        levelController.RollOver();
	}
	public static void ResumeGame()
	{
		levelController.ResumeGame ();
	}

	public static bool IsGamePaused()
	{
		return levelController.IsPaused;
	}

    public static bool IsPlayerDead()
    {
        return levelController.IsPlayerDead;
    }

    
    public static void PlayerDide()
	{
        levelController.PlayerDide();
	}

    public static bool IsRollingBack()
    {
        return levelController.IsRollingBack;
    }

    public static Vector3 GetLevelStartPostion()
    {
        return levelController.MoverLevel[levelController.LevelIndex].transform.position;
    }

    public static int GetLevelIndex()
    {
        return levelController.LevelIndex;
    }
}
