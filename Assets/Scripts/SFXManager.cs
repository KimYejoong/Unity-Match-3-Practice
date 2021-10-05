using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip audioFlipTry;
    public AudioClip audioMatchSuccess;
    public AudioClip audioMatchFail;
    public AudioClip audioGameOver;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        this._audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string sfx)
    {
        switch(sfx)
        {
            case "FlipTry":
                _audioSource.clip = audioFlipTry;
                break;

            case "MatchSuccess":
                _audioSource.clip = audioMatchSuccess;
                break;

            case "MatchFail":
                _audioSource.clip = audioMatchFail;
                break;

            case "GameOver":
                _audioSource.clip = audioGameOver;
                break;
        }

        _audioSource.Play();
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }
}
