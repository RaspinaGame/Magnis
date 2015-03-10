using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour
{

    private static SaveSystem instance = null;

    public static SaveSystem Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("SaveSystem").AddComponent<SaveSystem>(); //create game manager object if required
            return instance;
        }
    }

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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //|| UNITY_EDITOR
    //#if UNITY_Android 

    public static void SetInt(string inKey, int inInt)
    {
        PlayerPrefs.SetInt(inKey, inInt);
    }

    public static int GetInt(string inKey, int defaultInt)
    {
        return PlayerPrefs.GetInt(inKey, defaultInt);
    }

    public static void SetFloat(string inKey, float inFloat)
    {
        PlayerPrefs.SetFloat(inKey, inFloat);
    }

    public static float GetFloat(string inKey, int defaultFloat)
    {
        return PlayerPrefs.GetFloat(inKey, defaultFloat);
    }

    public static void SetString(string inKey, string inString)
    {
        PlayerPrefs.SetString(inKey, inString);
    }

    public static string GetString(string inKey, string defaultString)
    {
        return PlayerPrefs.GetString(inKey, defaultString);
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    //#endif

//#if UNITY_IPHONE || UNITY_STANDALONE_OSX

//    public static void SetInt(string inKey, int inInt)
//    {
//        P31Prefs.setInt(inKey, inInt);
//    }

//    public static int GetInt(string inKey, int defaultInt)
//    {
//        int result = P31Prefs.getInt(inKey);
//        return (result != 0) ? result : defaultInt;
//    }

//    public static void SetFloat(string inKey, float inFloat)
//    {
//        P31Prefs.setFloat(inKey, inFloat);
//    }

//    public static float GetFloat(string inKey, int defaultFloat)
//    {
//        float result = P31Prefs.getFloat(inKey); ;
//        return (result != 0) ? result : defaultFloat;
//    }

//    public static void SetString(string inKey, string inString)
//    {
//        P31Prefs.setString(inKey, inString);
//    }

//    public static string GetString(string inKey, string defaultString)
//    {
//        string result = P31Prefs.getString(inKey);
//        return (result != "") ? result : defaultString;
//    }

//    public static void Save()
//    {
//        //   P31Prefs.Save();
//    }

//    public static void DeleteAll()
//    {
//        // PlayerPrefs.DeleteAll();
//        P31Prefs.removeAll();
//    }

//#endif
}
