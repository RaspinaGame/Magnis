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
    public bool triggerOnScreen = true;

    private AudioSource audioSource;
    private Animation anim;
    private int startingTriggerCount;

    Vector3 startingPosition;
    Vector3 startingScale;
    Quaternion startingRotation;
	// Use this for initialization

    void Awake()
    {
        startingTriggerCount = triggerCount;
        startingPosition = transform.position;
        startingScale = transform.localScale;
        startingRotation = transform.localRotation;
    }

	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
        anim.AddClip(animClip, animClip.name);
        

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
            anim[animClip.name].speed = playRate;
            anim.Play(animClip.name);
            if (soundClip != null)
            {
                audioSource.clip = soundClip;
                audioSource.PlayDelayed(soundClipPlayDelayedTime);
            }
        }
    }

    public virtual void OnStartLevel( int levelIndex)
    {
       
       // anim.Stop();
        resertToInitial();
        
        //anim.Play(animClip.name, PlayMode.StopAll);
        if (trigger == null)
        {
            OnTriggerTouched();
        }
    }

    protected void resertToInitial()
    {
       // gameObject.SampleAnimation(animClip,0);
        anim[animClip.name].speed = -1;
        anim.Play(animClip.name);
      //  anim.Rewind();
        //animation.Rewind(animClip.name);

        //transform.position = startingPosition;
        //transform.localScale = startingScale;
        //transform.localRotation = startingRotation;
        triggerCount = startingTriggerCount;
       
    }

    public void GameIsPused()
    {
        anim.Stop();
    }

}

