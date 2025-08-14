using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{

    [Header("Steps")]
    [SerializeField] private GameObject[] steps;

    [Header("GameObjects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject tutorialMenu;

    [Header("PageDots Colors")]
    [SerializeField] private GameObject[] pageDots;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;


    private int currentStep = 0;

    void Start()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].SetActive(i == 0);
        }
        UpdateButtons();
        UpdatePageDots();
    }

    public void NextStep()
    {
        if (currentStep < steps.Length - 1)
        {
            steps[currentStep].SetActive(false);
            currentStep++;
            steps[currentStep].SetActive(true);
            UpdateButtons();
            UpdatePageDots();
        } else
        {
            tutorialMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            steps[currentStep].SetActive(false);
            currentStep--;
            steps[currentStep].SetActive(true);
            UpdateButtons();
            UpdatePageDots();
        }
        else
        {
            // Primeira página: voltar para menu
            tutorialMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    private void UpdatePageDots()
    {
        for (int i = 0; i < pageDots.Length; i++)
        {
            Image dotImage = pageDots[i].GetComponent<Image>();
            if (dotImage != null)
            {
                dotImage.color = (i == currentStep) ? activeColor : inactiveColor;
            }
        }
    }

    void UpdateButtons()
    {
        if (currentStep == 0)
        {
            backButton.GetComponentInChildren<TMP_Text>().text = "Menu";
        }
        else
        {
            backButton.GetComponentInChildren<TMP_Text>().text = "Voltar";
        }

        if (currentStep == steps.Length - 1)
        {
            nextButton.GetComponentInChildren<TMP_Text>().text = "Confirmar";
        }
        else
        {
            nextButton.GetComponentInChildren<TMP_Text>().text = "Próximo";
        }
    }

}
