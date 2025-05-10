using System.Collections.Generic;
using UnityEngine;

public enum Sfx
{
    Button,
    Buy,
    Clear,
    Fail,
    Grow,
    PauseIn,
    PauseOut,
    Sell,
    Touch,
    Unlock
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("# BGM")]
    private GameObject bgmPlayer;
    private AudioSource bgmAudioSource;
    [SerializeField]
    private AudioClip bgmClip;
    [SerializeField]
    private float bgmVolume;

    [Header("# SFX")]
    private GameObject sfxPlayer;
    private AudioSource sfxAudioSource;
    [SerializeField]
    private AudioClip[] sfxClip;
    [SerializeField]
    private float sfxVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void Start()
    {
        bgmAudioSource.Play();
    }
    
    private void Init()
    {
        // BGM 초기화
        bgmPlayer = new GameObject("BgmPlayer");
        bgmPlayer.transform.parent = transform;
        bgmAudioSource = bgmPlayer.AddComponent<AudioSource>();
        bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        bgmAudioSource.volume = bgmVolume;
        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.loop = true;

        // SFX 초기화
        sfxPlayer = new GameObject("SfxPlayer");
        sfxPlayer.transform.parent = transform;
        sfxAudioSource = sfxPlayer.AddComponent<AudioSource>();
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        sfxAudioSource.volume = sfxVolume;
        sfxAudioSource.loop = false;
        sfxAudioSource.playOnAwake = false;
    }

    public void PlaySfxAudio(Sfx sfx)
    {
        sfxAudioSource.clip = sfxClip[(int)sfx];
        sfxAudioSource.Play();
    }

    public void ChangeBgmVolume(float vol)
    {
        bgmVolume = vol;
        bgmAudioSource.volume = bgmVolume;
    }

    public void ChangeSfxVolume(float vol)
    {
        sfxVolume = vol;
        sfxAudioSource.volume = sfxVolume;
    }
}
