using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGAudioManager : MonoBehaviour 
{
    public float fadeTime = 4.0F;

    BGAudio[] BGaudio;
    LevelController levelController;
    AudioSource audioSource;
    enum Fade { In, Out };
    
	
    void Awake()
    {
        levelController = GetComponent<LevelController>();
        BGaudio = GetComponentsInChildren<BGAudio>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.panLevel = 0;
        audioSource.pan = 0;
        audioSource.spread = 0;
        audioSource.priority = 0;
        
    }

    void Start()
    {
        audioSource.ignoreListenerPause = true;
        audioSource.ignoreListenerVolume = true;
    }

	// Update is called once per frame
	void Update () 
    {
        
	}

    public void levelStart(int levelIndex)
    {
        if (levelIndex < BGaudio.Length)
        {
            if (audioSource.clip != null)
            {
                if (audioSource.clip.name != BGaudio[levelIndex].audioClip.name)
                {
                    PlayClip(BGaudio[levelIndex].audioClip);
                }
            }
            else
            {
                PlayClip(BGaudio[levelIndex].audioClip);
            }
        }
        else if (BGaudio.Length != 0)
        {
            if (audioSource.clip != null)
            {
                if (audioSource.clip.name != BGaudio[BGaudio.Length-1].audioClip.name)
                {
                    PlayClip(BGaudio[BGaudio.Length - 1].audioClip);
                }
            }
            else
            {
                PlayClip(BGaudio[BGaudio.Length - 1].audioClip);
            }
        }
    }


    void PlayClip(AudioClip inClip)
    {
       // audioSource.Play()
        if (audioSource.clip != null)
            StartCoroutine(FadeAudioAndPlay(fadeTime, Fade.Out, inClip));
        else
        {
            audioSource.clip = inClip;
            audioSource.Play();
        }
       // audioSource.clip = inClip;
        //audioSource.clip = BGaudio[levelIndex].audioClip;
    }






    IEnumerator FadeAudioAndPlay(float timer, Fade fadeType, AudioClip inClip)
    {
        float start = fadeType == Fade.In ? 0.0F : 1.0F;
        float end = fadeType == Fade.In ? 1.0F : 0.0F;
        float i = 0.0F;
        float step = 1.0F / timer;

        while (i <= 1.0F)
        {
            i += step * Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }

        audioSource.Stop();
        audioSource.clip = inClip;
        audioSource.volume = 1;
        audioSource.Play();
    }
}
