using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
                CloseSettings();
            else
                OpenSettings();
        }
    }

    public void ButtonOpenSettings()
    {
        OpenSettings();
    }

    public void ButtonClosedSettings()
    {
        CloseSettings();
    }

    private void OpenSettings()
    {
        if (!panelConfig.activeSelf)
        {
            foreach (var hover in hoverButtons)
            {
                hover.ResetHoverState();
            }

            panelConfig.SetActive(true);
            Time.timeScale = 0f;
            blockPanel.SetActive(true);
        }
    }
    private void CloseSettings()
    {
        panelConfig.SetActive(false);
        Time.timeScale = 1f;
        blockPanel.SetActive(false);

    }
}
