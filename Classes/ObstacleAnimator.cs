using UnityEngine;
using System.Collections;
//using UnityEditor;


[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(OAManager))]

public class ObstacleAnimator : MonoBehaviour {

    public GameObject trigger;
    public AudioClip soundClip;
    public float animClipPlayDelayedTime = 0f;
    public float soundClipPlayDelayedTime = 0f;
    public AnimationClip animClip;
    public AnimationClip initialAnimClip;
    public float crossFadeTime = 0.75f;
    public float playRate = 1f;
    public int triggerCount = 1;
    public bool repeatTrigger = false;
    public bool triggerOnScreen = true;

    private AudioSource audioSource;
    private Animation anim;
    private int startingTriggerCount;
    OAManager oAManager;

    Vector3 startingPosition;
    Vector3 startingScale;
    Quaternion startingRotation;

    bool trigerTouched;

    float animClipPlayDelayedTimer;
	// Use this for initialization

    void Awake()
    {
        startingTriggerCount = triggerCount;
        startingPosition = transform.position;
        startingScale = transform.localScale;
        startingRotation = transform.localRotation;
		oAManager = GetComponent<OAManager>();
		if (oAManager == null)
		{
			oAManager = gameObject.AddComponent<OAManager>();
		}
		oAManager.AddObstacleAnimator(this);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
        anim.cullingType = AnimationCullingType.AlwaysAnimate;
        anim.animatePhysics = true;
        anim.AddClip(animClip, animClip.name);
        
        //initialAnimClip = animClip;

        if (initialAnimClip != null)
        {
            anim.AddClip(initialAnimClip, initialAnimClip.name);
        }
        
        if (trigger != null)
        {
            if (trigger.GetComponent<OATrigger>() != null)
            {
                trigger.GetComponent<OATrigger>().AddObstacleAnimator(this);
            }
            else
            {
                trigger.AddComponent<OATrigger>().AddObstacleAnimator(this);
            }
        }
    }

	void Start () 
    {
        //gameObject.SampleAnimation(animClip, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (trigerTouched && !GameInfo.IsGamePaused())
        {
            animClipPlayDelayedTimer -= Time.deltaTime;
            if (animClipPlayDelayedTimer <= 0f)
            {
                trigerTouched = false;
                PlayEffects();
            }

        }
	
	}

    public void OnTriggerTouched()
    {
       // Invoke("PlayEffects", animClipPlayDelayedTime);

        if (animClipPlayDelayedTime == 0f)
        {
            PlayEffects();
        }
        else 
        {
            animClipPlayDelayedTimer = animClipPlayDelayedTime;
            trigerTouched = true;
        }
    }

    void PlayEffects()
    {
        triggerCount--;
        if (triggerCount == 0 || repeatTrigger)
        {
            oAManager.AddTriggerdObstacleAnimators(this);
            print("my trigger has been touched" + gameObject.name);
            anim[animClip.name].time = 0;
           // anim[animClip.name].wrapMode = WrapMode.ClampForever;
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

    public virtual void OnStartNextLevel( int levelIndex)
    {
       
       // anim.Stop();
       // resertToInitial();
        
        //anim.Play(animClip.name, PlayMode.StopAll);
        if (trigger == null)
        {
            OnTriggerTouched();
        }
    }

    public void GameIsPused(int levelIndex)
    {
      //  if (!GameInfo.IsPlayerDead())
      //  {
 
      //  }
       // AnimationEvent animEve = new AnimationEvent();
        //animEve.functionName= "FirstFrameRiched";
        //animEve.time = 0;
        //animClip.AddEvent(animEve);
     //   CancelInvoke();
        //resertToInitial();
        //if (!anim.IsPlaying(animClip.name))
           // anim[animClip.name].time = animClip.length;

        anim[animClip.name].speed = 0;
       // anim[animClip.name].speed = -1f;
        //anim.Play(animClip.name);
        
       // anim.Stop(animClip.name);
       
        //
       // anim.Stop(animClip.name);
       
        //anim[animClip.name].normalizedTime = 0.0F;
        //anim[animClip.name].enabled = true;
        //anim[animClip.name].weight = 1;
        //anim.CrossFade("initialAnimClip", 1.5f);
        //anim.Blend("initialAnimClip");
       // animation["initialAnimClip"].enabled = false;
        //anim.Play("initialAnimClip");
        //anim.Sample();
        //anim.Stop();

       // print("GameIsPused" + gameObject.name);
       
    }

        
    public void GameIsResumed(int levelIndex)
    {
      //  CancelInvoke();
        anim[animClip.name].speed = playRate;
    }

    public void RoleBack(int levelIndex)
    {
        if (initialAnimClip != null)
        {
            anim.CrossFade(initialAnimClip.name, crossFadeTime);
        }
    }

    protected void resertToInitial()
    {
        if (oAManager.ShoudBlendToinitialAnimClip(this))
        {
           // if (anim["initialAnimClip"] == null)
           // {
            //    initialAnimClip = CustomizeAnimByFrame(animClip);
            //    anim.AddClip(initialAnimClip, "initialAnimClip");
            //}
            //anim.CrossFade("initialAnimClip",1f);
            anim[animClip.name].time = 0;
            anim[animClip.name].speed = 0;
            anim.Play(animClip.name);
            // anim.Stop();
        }
        //anim["initialAnimClip"].wrapMode = WrapMode.Loop;
        
       // anim[animClip.name].speed = -0.5f;
        //anim.Play(animClip.name);
      //  anim.Rewind();
        //animation.Rewind(animClip.name);
        
        //anim.Stop();
        //transform.position = startingPosition;
        //transform.localScale = startingScale;
        //transform.localRotation = startingRotation;
        triggerCount = startingTriggerCount;
       
    }

    void FirstFrameRiched()
    {
        //anim.Stop(animClip.name);
       // animClip.(animEve);
       // anim[animClip.name].speed = -0.5f;
       // anim[animClip.name].time = 0;
       // anim.Play(animClip.name);
        //  anim.Rewind();
        //animation.Rewind(animClip.name);
         
    }

    void OnDisable()
    {
       // print("OnDisable");
        anim.enabled = false;
    }

    void OnEnable()
    {
        anim.enabled = true;
    }



    //AnimationClip CustomizeAnimByFrame(AnimationClip inClip,int frameIndex = 0)
    //{
    //   // AnimationClip customAnim = new AnimationClip();
    //    //AnimationClipCurveData[] animClipCurves =  AnimationUtility.GetAllCurves(inClip,true);

    //    //foreach (AnimationClipCurveData animClipCurve in animClipCurves)
    //    //{
    //    //    AnimationCurve newCurve = new AnimationCurve();

    //    //    newCurve.AddKey(animClipCurve.curve.keys[frameIndex].time, animClipCurve.curve.keys[frameIndex].value);
    //    //    newCurve.AddKey(animClipCurve.curve.keys[frameIndex+1].time, animClipCurve.curve.keys[frameIndex].value);
    //    //    animClipCurve.curve = newCurve;
    //    //}

    //    //foreach (AnimationClipCurveData animClipCurve in animClipCurves)
    //    //{
    //    //    customAnim.SetCurve("", animClipCurve.type, animClipCurve.propertyName, animClipCurve.curve);
    //    //}

    //   // return customAnim;
    //}

}

