using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource trainSoundSource;
    [SerializeField] private AudioSource carHornSoundSource;

    [SerializeField] private AudioClip homeBgrClip;
    [SerializeField] private AudioClip btnClip;

    //[SerializeField] private AudioClip trainLightClip;
    //[SerializeField] private AudioClip carHornClip;

    private bool musicActive, soundActive;
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        musicActive = PlayerPrefs.GetInt("Music Game", 1) == 1 ? true : false;
        soundActive = PlayerPrefs.GetInt("Sound Game", 1) == 1 ? true : false;
        SetMusicClip(homeBgrClip);
        SetMuteorPause(!musicActive, 0, false);
        SetMuteorPause(!soundActive, 1, false);
        DontDestroyOnLoad(gameObject);
    }
 
    public void SetMuteorPause(bool mute, int type, bool setData)
    {
        if (type == 0)
        {
            if (setData)
            {
                PlayerPrefs.SetInt("Music Game", mute ? 0 : 1);
            }
            if (mute)
            {
                musicSource.Pause();
            }
            else
            {
                musicSource.Play();
            }
            return;
        }
        if (setData)
        {
            PlayerPrefs.SetInt("Sound Game", mute ? 0 : 1);
        }
        soundSource.mute = mute;
        trainSoundSource.mute = mute;
        carHornSoundSource.mute = mute;
    }
    public void PlaySoundButton()
    {
        soundSource.PlayOneShot(btnClip);
    }
    public void SetMusicClip(AudioClip clip)
    {
        musicSource.clip = clip;
    }
    public bool GetMuteSound()
    {
        return soundSource.mute;
    }
    public bool GetMuteMusic()
    {
        return !musicSource.isPlaying;
    }
    public void PlayTrainLightSound(bool play)
    {
        if (play)
        {
            trainSoundSource.Play();
            trainSoundSource.loop = true;
        }
        else
        {
            trainSoundSource.loop = false;
        }
    }
    public void PlayCarHornSound()
    {
        carHornSoundSource.Play();
    }
}
