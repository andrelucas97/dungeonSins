using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [Header("PanelConfig")]
    [SerializeField] private GameObject panelConfig;
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private List<ButtonTextColorHover> hoverButtons;

    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;
    private AudioManager audioManager;

    void Start()
    {
        panelConfig.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelConfig.activeSelf)
                CloseConfig();
            else
                OpenConfig();
        }
    }
    // FUNC PUBLICAS
    public void ButtonPlayAgain()
    {
        PlayAgain();
    }


    public void ButtonOpenConfig()
    {
        OpenConfig();
    }
    public void ButtonClosedConfig()
    {
        CloseConfig();
    }
    public void ButtonMenu(string sceneManager)
    {
        GoToMenu(sceneManager);
    }

    // FUNC PRIVADAS
    private void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
    private void GoToMenu(string sceneManager)
    {
        SceneManager.LoadScene(sceneManager);
    }

    private void OpenConfig()
    {
        if (!panelConfig.activeSelf)
        {
            foreach (var hover in hoverButtons)
            {
                hover.ResetHoverState();
            }

            panelConfig.SetActive(true);
            blockPanel.SetActive(true);
        }
    }
    private void CloseConfig()
    {
        panelConfig.SetActive(false);
        blockPanel.SetActive(false);

    }
}
