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

    [Header("Char Stats")]
    [SerializeField] private TextMeshProUGUI namePlayer;
    [SerializeField] private TextMeshProUGUI profileChar;
    [SerializeField] private TextMeshProUGUI attackPlayer;
    [SerializeField] private TextMeshProUGUI defensePlayer;
    [SerializeField] private TextMeshProUGUI shieldPlayer;
    [SerializeField] private GameObject boxName;
    [SerializeField] private GameObject boxAbility;
    [SerializeField] private GameObject boxResume;
    [SerializeField] private GameObject[] boxStats;


    [Header("Char Abilities")]
    [SerializeField] private GameObject abilityTextPrefab;
    [SerializeField] private AbilityDatabase abilityDatabase;
    public Transform abilityContainer;

    private int selectedCharacterIndex = -1;

    void Start()
    {
        ButtonSelectCharacter(0);
    }

    // FUN��ES PUBLICAS
    public void ButtonSelectCharacter(int index)
    {

        var manager = CharacterSelectionManager.GetInstance();

        selectedCharacterIndex = index; 

        CharacterData selectedData = characterDatas[index];

        UpdateSelectedCharacter(index, selectedData);


        GameData.SelectedCharacterIndex = selectedCharacterIndex;
        CharacterSelectionManager.Instance.SelectedCharacter = selectedData;

    }

    // FUN��ES PRIVADAS

    private void UpdateSelectedCharacter(int index, CharacterData selectedData)
    {
        if (index < 0 || index >= characterSprites.Length)
        {
            Debug.LogError("�ndice do personagem inv�lido!");
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

        SetBoxColor(selectedData, boxName, boxAbility, boxStats, boxResume);

        foreach (Transform child in abilityContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterAbility ability in selectedData.Abilities)
        {
            AbilityData abilityData = abilityDatabase.GetAbilityData(ability);
            if (abilityData == null)
            {
                Debug.LogWarning($"AbilityData n�o encontrado para: {ability}");
                continue;
            }

            // Instancia o bot�o
            GameObject obj = Instantiate(abilityTextPrefab, abilityContainer);
            obj.SetActive(true);

            // Atualiza o texto do bot�o
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = abilityData.AbilityName;
                text.fontSize = 50;
                text.enabled = true;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI n�o encontrado no prefab!");
            }

            // Passa o AbilityData completo pro handler
            AbilityButtonHandler handler = obj.GetComponentInChildren<AbilityButtonHandler>();
            if (handler != null)
            {
                GameObject tooltip = obj.transform.Find("ToolTipPanel")?.gameObject;
                TextMeshProUGUI tooltipText = tooltip?.GetComponent<TextMeshProUGUI>();

                handler.SetAbilityData(abilityData);
                handler.SetTooltipText(tooltipText);
            }
            else
                Debug.Log("handler � null");
        }

    }

    private void SetBoxColor(CharacterData selectedData, GameObject boxName, GameObject boxAbility, GameObject[] boxStats, GameObject boxResume)
    {       

        ApplyColorBox(boxName, selectedData);
        ApplyColorBox(boxAbility, selectedData);
        ApplyColorBox(boxResume, selectedData);
        // Aplica nos boxStats
        foreach (GameObject statBox in boxStats)
        {
            ApplyColorBox(statBox, selectedData);
        }

    }

    private void ApplyColorBox(GameObject box, CharacterData selectedData)
    {
        Color baseColor = charColors.GetColor(selectedData.CharColor);

        if (box != null && box.TryGetComponent<Image>(out Image image))
        {
            image.color = baseColor;
        }
    }
}
