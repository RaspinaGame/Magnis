using UnityEngine;
using System.Collections;
using UnityEngine.UI;


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
    public bool reTriggerOnStart = true;
    public bool bIsVoice;
    public float audioFadeTime = 1.0F;
    public float audioListenerFadeOutVolume = 0.2F;
    public string voiceTextString;
    public Text voiceTextComponent;

    private AudioSource audioSource;
    private Animation anim;
    private int startingTriggerCount;
    OAManager oAManager;

    Vector3 startingPosition;
    Vector3 startingScale;
    Quaternion startingRotation;
    bool trigerTouched;

    float animClipPlayDelayedTimer;

    Animator voiceTextAnim;
    bool audioIsPlaying;
    bool audioIsPaused;

    AudioSource bgMusic;

    
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
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.clip = null;
        if (bIsVoice)
        {
            audioSource.panLevel = 0;
            audioSource.pan = 0;
            audioSource.spread = 0;
            audioSource.priority = 1;
        }

        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
        anim.cullingType = AnimationCullingType.AlwaysAnimate;
        anim.animatePhysics = false;
        if (animClip != null)
        {
            anim.AddClip(animClip, animClip.name);
        }
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
          //  print("my trigger has been touched" + gameObject.name);
            if (animClip != null)
            {
                if (playRate > 0)
                {
                    anim[animClip.name].time = 0;
                    // anim[animClip.name].wrapMode = WrapMode.ClampForever;
                }
                else 
                {
                    anim[animClip.name].time = animClip.length;
                }

                anim[animClip.name].speed = playRate;
                anim.Play(animClip.name);

               
            }
            if (soundClip != null)
            {

                if (bIsVoice)
                {
                    if (soundClipPlayDelayedTime == 0)
                        PlayVoice();
                    else
                        Invoke("PlayVoice", soundClipPlayDelayedTime);
                }
                else 
                {
                    audioSource.clip = soundClip;
                    audioSource.PlayDelayed(soundClipPlayDelayedTime);
                    audioIsPlaying = true;
                }
            }
           
        }
    }

    void PlayVoice()
    {
        if (PlayerPrefs.GetInt("sfx", 1) == 1)
            audioSource.ignoreListenerVolume = true;
        bgMusic = GameObject.FindGameObjectWithTag("LevelController").GetComponent<AudioSource>();
        StartCoroutine(FadeAudioAndPlay(audioFadeTime));
        audioIsPlaying = true;
        if (voiceTextString != "")
        {
            if (voiceTextComponent == null)
            {
                Text[] texts = FindObjectsOfType<Text>();
                for (int i = 0; i < texts.Length; i++)
                {
                    if (texts[i].gameObject.CompareTag("VoiceText"))
                        voiceTextComponent = texts[i];
                }
            }
            voiceTextAnim = voiceTextComponent.GetComponentInParent<Animator>();
            voiceTextComponent.text = voiceTextString;
            voiceTextAnim.SetBool("show", true);
        }
        //  AudioListener.volume = 0.5f;
    }

    void ResetAudioListenerVolume()
    {
        StartCoroutine(ResetFade(audioFadeTime));
        if (voiceTextString != "")
        {
            voiceTextAnim.SetBool("show", false);
        }
        audioIsPlaying = false;
    }

    IEnumerator FadeAudioAndPlay(float timer)
    {
        float start = 1.0F;
        float end = audioListenerFadeOutVolume;
        float i = 0.0F;
        float step = 1.0F / timer;

        while (i <= 1.0F)
        {
            i += step * Time.deltaTime;
            if (PlayerPrefs.GetInt("sfx", 1) == 1)
                AudioListener.volume = Mathf.Lerp(start, end, i);
            bgMusic.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
        if (PlayerPrefs.GetInt("sfx", 1) == 1)
            AudioListener.volume = end;
        audioSource.Stop();
        audioSource.clip = soundClip;
        audioSource.volume = 1;
        if (!audioIsPaused)
            audioSource.Play();
     //   audioIsPlaying = true;
        Invoke("ResetAudioListenerVolume", soundClip.length);
    }

    IEnumerator ResetFade(float timer)
    {
        float start = audioListenerFadeOutVolume;
        float end = 1.0F;
        float i = 0.0F;
        float step = 1.0F / timer;

        while (i <= 1.0F)
        {
            i += step * Time.deltaTime;
            if (PlayerPrefs.GetInt("sfx", 1) == 1)
                AudioListener.volume = Mathf.Lerp(start, end, i);
            bgMusic.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
        if (PlayerPrefs.GetInt("sfx", 1) == 1)
            AudioListener.volume = end;
    }

    public virtual void OnStartLevel( int levelIndex)
    {
       
       // anim.Stop();
      //  resertToInitial();
        
        //anim.Play(animClip.name, PlayMode.StopAll);
        if (trigger == null && bIsVoice)
        {
            OnTriggerTouched();
        }
    }

    public virtual void OnRestartLevel( int levelIndex)
    {
       
       // anim.Stop();
        resertToInitial();
        
        //anim.Play(animClip.name, PlayMode.StopAll);
        if (trigger == null)
        {
            OnTriggerTouched();
        }
    }

    public virtual void OnLateStartLevel(int levelIndex)
    {

        // anim.Stop();
        //resertToInitial();

        //anim.Play(animClip.name, PlayMode.StopAll);
        if (trigger == null && !bIsVoice)
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

        if (animClip != null)
        {
            anim[animClip.name].speed = 0;
        }
        if (audioIsPlaying)
        {
            audio.Pause();
            audioIsPaused = true;
        }
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
        if (animClip != null)
        {
            anim[animClip.name].speed = playRate;
        }

        if (audioIsPlaying)
        {
            audio.Play();
            audioIsPaused = false;
        }
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
            if (animClip != null)
            {
                anim[animClip.name].time = 0;
                anim[animClip.name].speed = 0;
                anim.Play(animClip.name);
            }
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
        if (reTriggerOnStart)
        {
            triggerCount = startingTriggerCount;
        }

        if (bIsVoice)
        {
            StopAllCoroutines();
            AudioListener.volume = 1;
            if (voiceTextString != "")
            {
                voiceTextAnim.SetBool("show", false);
            }
        }
        audioSource.Stop();
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
        audioSource.enabled = false;
    }

    void OnEnable()
    {
        anim.enabled = true;
        audioSource.enabled = true;
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

