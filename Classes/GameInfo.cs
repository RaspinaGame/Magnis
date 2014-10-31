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
	}

	// Use this for initialization
	void Start () 
	{
	
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

	public static void GameOver()
	{
		levelController.Resetlevel();
	}
	public static void PauseGame()
	{
		levelController.PauseGame ();
	}
	public static void ResumeGame()
	{
		levelController.ResumeGame ();
	}

	public static bool IsGamePaused()
	{
		return levelController.IsPaused;
	}
}
