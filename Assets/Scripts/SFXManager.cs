using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip audioFlipTry;
    public AudioClip audioMatchSuccess;
    public AudioClip audioMatchFail;
    public AudioClip audioGameOver;    

    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string SFX)
    {
        switch(SFX)
        {
            case "FlipTry":
                audioSource.clip = audioFlipTry;
                break;

            case "MatchSuccess":
                audioSource.clip = audioMatchSuccess;
                break;

            case "MatchFail":
                audioSource.clip = audioMatchFail;
                break;

            case "GameOver":
                audioSource.clip = audioGameOver;
                break;
        }

        audioSource.Play();
    }

    void StopSound()
    {
        audioSource.Stop();
    }
}
