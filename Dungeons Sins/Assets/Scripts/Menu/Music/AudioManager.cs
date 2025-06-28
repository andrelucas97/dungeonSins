using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;

    [Header("Button Click")]
    [SerializeField] private AudioSource clickSource;

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

    void Start()
    {
        musicSource.Play();
    }

    private void PlayMusic(AudioClip clip)
    {
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
}
