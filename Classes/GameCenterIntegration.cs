using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterIntegration : MonoBehaviour {

    private static GameCenterIntegration instance = null;
    public static GameCenterIntegration Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("GameCenterIntegration").AddComponent<GameCenterIntegration>(); //create game manager object if required
            return instance;
        }
    }


    public const string AchievementAllstars = "allstars";
    public const string Achievement120stars = "120stars";
    public const string Achievement60stars = "60stars";
    public const string Achievement30stars = "30stars";
    public const string Achievement1stStep = "1stStep";
    public const string Achievement3stars = "3stars";
    public const string AchievementChapter1 = "chapter1";
    public const string AchievementChapter2 = "chapter2";
    public const string AchievementChapter3 = "chapter3";
    public const string AchievementChapter4 = "chapter4";
    public const string AchievementChapter5 = "chapter5";

    void Awake()
    {
        //Check if there is an existing instance of this object
        if (instance)
            DestroyImmediate(gameObject); //Delete duplicate
        else
        {
            instance = this; //Make this object the only instance
            DontDestroyOnLoad(gameObject); //Set as do not destroy
        }

        //        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
   

	// Use this for initialization
	void Start () {
        //Social.localUser.Authenticate(success =>
        //{
        //    if (success)
        //    {
        //        Debug.Log("Authentication successful");
        //        string userInfo = "Username: " + Social.localUser.userName +
        //            "\nUser ID: " + Social.localUser.id +
        //            "\nIsUnderage: " + Social.localUser.underage;
        //        Debug.Log(userInfo);

        //        // Request loaded achievements, and register a callback for processing them
        //      //  Social.LoadAchievements (ProcessLoadedAchievements);

        //        Social.ShowAchievementsUI();
        //        ReportProgress("Wellcom");
        //        //Social.ReportScore();
        //    }
        //    else
        //        Debug.Log("Authentication failed"); 
        //});

        //Social.localUser.LoadFriends(success =>
        //{
        //    Debug.Log(success ? "Loaded " + Social.localUser.friends.Length + " friends" : "Loading friends failed");

        //   // print(Social.localUser.friends[0].userName);
        //});

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // This function gets called when the LoadAchievement call completes
    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
            Debug.Log("Got " + achievements.Length + " achievements");

        // You can also call into the functions like this
        Social.ReportProgress("Achievement01", 100.0, result =>
        {
            if (result)
                Debug.Log("Successfully reported achievement progress");
            else
                Debug.Log("Failed to report achievement");
        });
    }

    public static void Authenticate()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    Debug.Log("Authentication successful");
                    string userInfo = "Username: " + Social.localUser.userName +
                        "\nUser ID: " + Social.localUser.id +
                        "\nIsUnderage: " + Social.localUser.underage;
                    Debug.Log(userInfo);


                    GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
                    ReportProgress("welcome");
                    CheckAchievement();
                }
                else
                    Debug.Log("Authentication failed");
            });
        }
    }

    public static void ReportProgress(string achievementID)
    {
        SaveSystem.SetInt(achievementID,1);
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementID, 100.0, result =>
            {
                if (result)
                {
                    Debug.Log("Successfully reported achievement progress : " + achievementID);
                    SaveSystem.SetInt(achievementID, 0);
                }
                else
                    Debug.Log("Failed to report achievement : " + achievementID);
            });
        }
        else
        {
            Debug.Log("Failed to report achievement : not authenticated");
        }
    }

    public static void ReportScore(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "StarNumber", result =>
            {
                if (result)
                    Debug.Log("Successfully reported Score");
                else
                    Debug.Log("Failed to report Score");
            });
        }
        else
        {
            Debug.Log("Failed to report Score : not authenticated");
        }
    }

    public static void ShowAchievementsUI()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            Debug.Log("Failed to ShowAchievementsUI : not authenticated");
        }
    }

    public static void ShowLeaderboardUI()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("Failed to ShowLeaderboardUI : not authenticated");
        }
    }

    static void CheckAchievement()
    {
        if(SaveSystem.GetInt(Achievement120stars,0) == 1)
        {
            ReportProgress(Achievement120stars);
        }
        if (SaveSystem.GetInt(Achievement1stStep, 0) == 1)
        {
            ReportProgress(Achievement1stStep);
        }
        if (SaveSystem.GetInt(Achievement30stars, 0) == 1)
        {
            ReportProgress(Achievement30stars);
        }
        if (SaveSystem.GetInt(Achievement3stars, 0) == 1)
        {
            ReportProgress(Achievement3stars);
        }
        if (SaveSystem.GetInt(Achievement60stars, 0) == 1)
        {
            ReportProgress(Achievement60stars);
        }
        if (SaveSystem.GetInt(AchievementAllstars, 0) == 1)
        {
            ReportProgress(AchievementAllstars);
        }
        if (SaveSystem.GetInt(AchievementChapter1, 0) == 1)
        {
            ReportProgress(AchievementChapter1);
        }
        if (SaveSystem.GetInt(AchievementChapter2, 0) == 1)
        {
            ReportProgress(AchievementChapter2);
        }
        if (SaveSystem.GetInt(AchievementChapter3, 0) == 1)
        {
            ReportProgress(AchievementChapter3);
        }
        if (SaveSystem.GetInt(AchievementChapter4, 0) == 1)
        {
            ReportProgress(AchievementChapter4);
        }
        if (SaveSystem.GetInt(AchievementChapter5, 0) == 1)
        {
            ReportProgress(AchievementChapter5);
        }
    }
}
