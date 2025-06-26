using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    [Header("List Images")]
    [SerializeField] private Sprite[] characterSprites;

    [Header("ImageUI")]
    [SerializeField] private Image selectedCharacterImage;

    private int selectedCharacterIndex = -1;

    void Start()
    {
        ButtonSelectCharacter(0);
    }

    // FUNÇÕES PUBLICAS
    public void ButtonSelectCharacter(int index)
    {
        UpdateSelectedCharacter(index);       
    }
    public void ButtonStartGame()
    {
        StartGame();
    }

    // FUNÇÕES PRIVADAS
    private void StartGame()
    {

        if (selectedCharacterIndex == -1)
        {
            return;
        }
        GameData.SelectedCharacterIndex = selectedCharacterIndex;

        SceneManager.LoadScene("GameScene");
    }
    private void UpdateSelectedCharacter(int index)
    {
        if (index < 0 || index >= characterSprites.Length)
        {
            Debug.LogError("Índice do personagem inválido!");
            return;
        }
        selectedCharacterIndex = index;

        if (selectedCharacterImage != null)
            selectedCharacterImage.sprite = characterSprites[index];
    }
}
