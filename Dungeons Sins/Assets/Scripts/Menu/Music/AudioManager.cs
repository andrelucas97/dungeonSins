using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Menu")]
    [SerializeField] private AudioClip musicMenuSource;

    [Header("Music Game")]
    [SerializeField] private AudioClip musicGameSource;

    [Header("Music Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Button Click")]
    [SerializeField] private AudioSource clickSource;

    [Header("Dice Roll")]
    [SerializeField] private AudioSource audioSourceDice;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMenuMusic();
        SetMusicVolume(0.1f);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "CharacterSelection")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "GameScene")
        {
            PlayGameMusic();
        }
    }

    public void PlayMenuMusic()
    {
        PlayMusic(musicMenuSource);
    }

    public void PlayGameMusic()
    {
        PlayMusic(musicGameSource);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public IEnumerator PlaySoundAndWait(AudioClip clip)
    {
        if (clickSource != null && clip != null)
        {
            clickSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }

    public void SetMusicVolume(float volume)
    {
        
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MusicVolume", dbVolume);
    }

    internal void PlaySound(AudioClip diceRollClip)
    {
        audioSourceDice.PlayOneShot(diceRollClip);
    }
}
