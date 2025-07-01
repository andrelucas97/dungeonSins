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
    [SerializeField] private AbilityDatabase abilityDatabase;
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

        UpdateSelectedCharacter(index, selectedData);
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
            // Busca o AbilityData pelo enum
            AbilityData abilityData = abilityDatabase.GetAbilityData(ability);
            if (abilityData == null)
            {
                Debug.LogWarning($"AbilityData não encontrado para: {ability}");
                continue;
            }

            // Instancia o botão
            GameObject obj = Instantiate(abilityTextPrefab, abilityContainer);
            obj.SetActive(true);

            // Atualiza o texto do botão
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = abilityData.AbilityName;
                text.enabled = true;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI não encontrado no prefab!");
            }

            // Passa o AbilityData completo pro handler
            AbilityButtonHandler handler = obj.GetComponent<AbilityButtonHandler>();
            if (handler != null)
            {
                GameObject tooltip = obj.transform.Find("ToolTipPanel")?.gameObject;
                TextMeshProUGUI tooltipText = tooltip?.GetComponentInChildren<TextMeshProUGUI>();

                handler.SetAbilityData(abilityData);
                handler.SetTooltipPanel(tooltip);
                handler.SetTooltipText(tooltipText);
            }
        }

    }
}
