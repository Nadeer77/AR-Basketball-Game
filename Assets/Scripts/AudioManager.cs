using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip bgMusic;
    public AudioClip hitGroundSFX;
    public AudioClip scoreSFX;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        musicSource.clip = bgMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitGroundSFX);
    }

    public void PlayScore()
    {
        sfxSource.PlayOneShot(scoreSFX);
    }

    // 🎚 Volume control (optional)
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", value);
    }
}