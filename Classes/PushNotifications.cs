using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PushNotifications : MonoBehaviour {

    private static PushNotifications instance = null;
    public static PushNotifications Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("PushNotifications").AddComponent<PushNotifications>(); //create game manager object if required
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

	void Start () 
    {
        GameThrive.Init("04c755e2-bf3d-11e4-b385-c7c43101007e", "", HandleNotification);

        // schedule notification to be delivered in 10 seconds
        var notif = new LocalNotification();
        notif.fireDate = DateTime.Now.AddSeconds(10);
        notif.alertAction = "Title Here";
        notif.alertBody = "My Alert Body Text";
        notif.applicationIconBadgeNumber = 99;
        notif.hasAction = true;//?
        notif.repeatCalendar = CalendarIdentifier.GregorianCalendar;
        notif.repeatInterval = CalendarUnit.Day;
        notif.soundName = LocalNotification.defaultSoundName;
        NotificationServices.ScheduleLocalNotification(notif);
    }               
    // Gets called when the player opens the notification.
    private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive) 
    {
        print("HandleNotification" + message + additionalData);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
