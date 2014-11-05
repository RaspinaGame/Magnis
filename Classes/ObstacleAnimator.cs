using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]

public class ObstacleAnimator : MonoBehaviour {

    public GameObject trigger;
    public AudioClip soundClip;
    public float soundClipPlayDelayedTime = 0f;
    public AnimationClip animClip;
    public float playRate = 1f;
    public int triggerCount = 1;
    public bool repeatTrigger = false;

    private AudioSource audioSource;
    private Animation anim;
	// Use this for initialization
	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
        anim.AddClip(animClip, animClip.name);
        anim[animClip.name].speed = playRate;

        if (trigger.GetComponent<OATrigger>() != null)
        {
            trigger.GetComponent<OATrigger>().AddObstacleAnimator(this);
        }
        else 
        {
            trigger.AddComponent<OATrigger>().AddObstacleAnimator(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerTouched()
    {
        triggerCount--;
        if (triggerCount == 0 || repeatTrigger)
        {
            print("my trigger has been touched");
            anim.Play(animClip.name);
            audioSource.clip = soundClip;
            audioSource.PlayDelayed(soundClipPlayDelayedTime);
        }
    }

    void OnStartLevel()
    {
        if (trigger == null)
        {
            OnTriggerTouched();
        }
    }
}
