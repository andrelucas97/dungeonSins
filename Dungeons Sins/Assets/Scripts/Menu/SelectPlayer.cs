using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    [Header("List Images")]
    [SerializeField] private Sprite[] characterSprites;

    [Header("ImageUI")]
    [SerializeField] private Image selectedCharacterImage;

    [Header("Profile Char")]
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private TextMeshProUGUI namePlayer;
    [SerializeField] private TextMeshProUGUI profileChar;
    [SerializeField] private TextMeshProUGUI attackPlayer;
    [SerializeField] private TextMeshProUGUI defensePlayer;
    [SerializeField] private TextMeshProUGUI shieldPlayer;

    [Header("Char Abilities")]
    [SerializeField] private GameObject abilityTextPrefab;
    public Transform abilityContainer;

    private int selectedCharacterIndex = -1;

    void Start()
    {
        ButtonSelectCharacter(0);
    }

    // FUNÇÕES PUBLICAS
    public void ButtonSelectCharacter(int index)
    {
        selectedCharacterIndex = index; 

        CharacterData selectedData = characterDatas[index];

        Debug.Log("Selecionado: " + selectedCharacterIndex);
        UpdateSelectedCharacter(index, selectedData);
    }
    public void ButtonStartGame()
    {
        StartGame();
    }

    // FUNÇÕES PRIVADAS
    private void StartGame()
    {
        Debug.Log("Selected: " + selectedCharacterIndex);

        if (selectedCharacterIndex == -1)
        {
            return;
        }
        GameData.SelectedCharacterIndex = selectedCharacterIndex;
        SceneManager.LoadScene("GameScene");
    }
    private void UpdateSelectedCharacter(int index, CharacterData selectedData)
    {
        if (index < 0 || index >= characterSprites.Length)
        {
            Debug.LogError("Índice do personagem inválido!");
            return;
        }

        if (selectedCharacterImage != null)
            selectedCharacterImage.sprite = selectedData.Portrait;

        namePlayer.text = $"{selectedData.CharName}";
        profileChar.text = $"Pecado capital: {selectedData.CodeName}\n" +
            $"Idade: {selectedData.Age} anos\n" +
            $"Classe: {selectedData.ClassChar}";

        attackPlayer.text = selectedData.Damage.ToString();
        defensePlayer.text = selectedData.MaxHealth.ToString();
        shieldPlayer.text = selectedData.Shield.ToString();

        foreach (Transform child in abilityContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterAbility ability in selectedData.Abilities)
        {
            GameObject obj = Instantiate(abilityTextPrefab, abilityContainer);
            obj.SetActive(true);

            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = ability.ToString();
                text.enabled = true;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI não encontrado no prefab!");
            }
        }

    }
}
