using UnityEngine;
using System.Collections;

public class AnimationEventHelper : MonoBehaviour {

    AudioSource audio;
    AudioSource audio2;
    public Animator inAnim;
	public ParticleSystem Psystem01;
	public ParticleSystem Psystem02;
	public ParticleSystem Psystem03;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play(AudioClip inClip)
    {
//        print("Play(AudioClip inClip)");
        if (audio == null)
        {
            audio = GetComponent<AudioSource>();
        }
        if (audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
        }

        audio.playOnAwake = false;
        audio.Stop();

        audio.PlayOneShot(inClip);
       // audio.clip = inClip;
       // audio.Play();
    }

    public void Play2(AudioClip inClip)
    {
        //        print("Play(AudioClip inClip)");
        //if (audio2 == null)
        //{
        //    audio2 = GetComponent<AudioSource>();
        //}
        if (audio2 == null)
        {
            audio2 = gameObject.AddComponent<AudioSource>();
        }

        audio2.playOnAwake = false;
        audio2.Stop();

        audio2.PlayOneShot(inClip);
        // audio.clip = inClip;
        // audio.Play();
    }

	public void ParticleEffect(float PSystemNum)
	{
		if (PSystemNum==1)
		{	
			Psystem01.gameObject.SetActive(true);
			Psystem01.Emit(90);
		}
		else if (PSystemNum==2)
		{
			Psystem02.gameObject.SetActive(true);
			Psystem02.Emit(90);
		}
		else if (PSystemNum==3)
		{
			Psystem03.gameObject.SetActive(true);
			Psystem03.Emit(90);
		}

		Invoke("StopPSystems",1.0f);
	}
	public void StopPSystems()
	{
		Psystem01.gameObject.SetActive(false);
		Psystem02.gameObject.SetActive(false);
		Psystem03.gameObject.SetActive(false);
	}

    public void LoadLevel(string levelName)
    {
        AutoFade.LoadLevel(levelName, 2f, 0.6f, Color.black);
    }

    public void AnimatortriggerSeter(string triggerName)
    {
        // GameObject.finn
        inAnim.SetTrigger(triggerName);
    }

    //public void AnimatorboolianSeter(string boolianName, bool inValue)
    //{
    //    inAnim.SetBool("boolianName", inValue);
    //}

    //public void AnimatorintigerSeter(string intigerName, int inValue)
    //{
    //    inAnim.SetInteger("intigerName", inValue);
    //}
}
