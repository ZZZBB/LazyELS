using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public bool isMute = false;
    public AudioClip gameOver;
    public AudioClip cilck;
    public AudioClip drop;
    public AudioClip down;
   
    public AudioClip hor;
    public AudioClip ror;
    public AudioSource audioSource;
    public AudioClip isLan;
    public AudioClip isAI;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
            }
   
    public void PlayIsLan()
    {
        PlayAudio(audioSource,6);
    }
   
   
    public void PlayClip()
    {
        PlayAudio(audioSource,2);
    }
  
    public void PlayOver()
    {
        PlayAudio(audioSource,7);
    }
    public void PlayAudio(AudioSource a,int i)
    {
        if (isMute) return;
        AudioClip ac = drop;
        switch (i)
        {
            case 1:
                ac = drop;
                break;
            case 2:
                ac = cilck;
                break;
            case 3:
                ac = down;
                break;
            case 4:
                ac = hor;
                break;
            case 5:
                ac = ror;
                break;
            case 6:
                 ac = isLan;
                break;
            case 7:
                ac = gameOver;
                break;
            case 8:
                ac = isAI;
                break;
        }
        audioSource.clip = ac;
        audioSource.Play();

    }

    public void OnAudioButton(Transform mute,Transform turnOn)
    {
        //PlayAudio(audioSource, 2);
        isMute = !isMute;
        mute.gameObject.SetActive(isMute);
        turnOn.gameObject.SetActive(!isMute);
        if (isMute == false)
        {
            PlayClip();
        }
    }
}
