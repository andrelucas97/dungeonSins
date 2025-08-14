using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TemporaryMessageManager messageManager;
    [SerializeField] private GameObject howToPlay;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        Time.timeScale = 1f;

    }

    // FUNÇÕES PUBLICAS
    public void ButtonPlayGame(string sceneName)
    {
        PlayGame(sceneName);
    }

    public void ButtonStartGame()
    {
        StartGame();
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
    private void PlayGame(string newScene)
    {
        SceneManager.LoadScene(newScene);

    }    
    private void StartGame()
    {
        if (GameData.SelectedCharacterIndex == -1)
        {
            Debug.LogWarning("Nenhum personagem selecionado!");
            return;
        }
        SceneManager.LoadScene("GameScene");
    }
    private void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void HowToPlay()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(true);
    }
    private void OpenCollections()
    {
        messageManager.ShowMessage(2f);
    }
    private void MenuSettings()
    {
        messageManager.ShowMessage(2f);
    }

}
