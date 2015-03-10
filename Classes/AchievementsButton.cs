using UnityEngine;
using System.Collections;

public class AchievementsButton : MonoBehaviour
{

	// Use this for initialization
	void Start () 
    {
       // SaveSystem.DeleteAll();
       // SaveSystem.Save();
        GameCenterIntegration.Authenticate();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnClick()
    {
       // SaveSystem.DeleteAll();
       // SaveSystem.Save();
        GameCenterIntegration.Authenticate();
        GameCenterIntegration.ShowAchievementsUI();
    }
}
