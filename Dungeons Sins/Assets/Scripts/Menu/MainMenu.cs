using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // FUNÇÕES PUBLICAS
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

    // FUNÇÕES PRIVADAS
    private void StartGame(string newScene)
    {
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
