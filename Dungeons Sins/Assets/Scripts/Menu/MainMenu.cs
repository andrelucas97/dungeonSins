using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }


    // FUN��ES PUBLICAS
    public void ButtonPlayGame(string sceneName)
    {
        StartGame(sceneName);
    }
    public void ButtonBackToMenu()
    {
        MainMenuScene();
    }
    public void ButtonHowToPlay()
    {
        HowToPlay();
    }
    public void ButtonCollections()
    {
        OpenCollections();
    }
    public void ButtonSettings()
    {
        MenuSettings();
    }
    public void ButtonExit()
    {

    }

    // FUN��ES PRIVADAS
    private void StartGame(string newScene)
    {
        if (audioManager != null)
        {
            StartCoroutine(PlaySoundThenChangeScene(newScene));
        }
        else
        {
            SceneManager.LoadScene(newScene);

        }

    }
    private IEnumerator PlaySoundThenChangeScene(string newScene)
    {
        yield return StartCoroutine(audioManager.PlaySoundAndWait(clickSound));
        SceneManager.LoadScene(newScene);
    }

    private void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void HowToPlay()
    {
        throw new NotImplementedException();
    }
    private void OpenCollections()
    {
        throw new NotImplementedException();
    }
    private void MenuSettings()
    {
        throw new NotImplementedException();
    }
}
