using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharUI : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image artworkImage;

    [Header("Profile")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI codeNameText;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI shield;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private Image[] slots;

    // Cor Correspondente ao Personagem
    [Header("Colors")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image backgroundPointer;
    [SerializeField] private Image backgroundStatusGame;

    [Header("Abilities")]
    [Header("Char Abilities")]
    [SerializeField] private GameObject abilityTextPrefab;
    [SerializeField] private Transform abilityContainer;
    [SerializeField] private AbilityDatabase abilityDatabase;

    private CharStats charStats;
    public void Setup(CharacterData character)
    {
        artworkImage.sprite = character.Portrait;
        nameText.text = $"{character.CharName},";
        codeNameText.text = character.CodeName;

        health.text = character.MaxHealth.ToString();
        shield.text = character.Shield.ToString();
        damage.text = character.Damage.ToString();

        Color baseColor = charColors.GetColor(character.CharColor);
        backgroundImage.color = baseColor;

        Color colorAlpha = baseColor;
        colorAlpha.a = 80f / 255f; // ou 0.3137f
        backgroundStatusGame.color = colorAlpha;


        float darkenFactor = 0.65f;
        Color darkColor = new Color(baseColor.r * darkenFactor, baseColor.g * darkenFactor, baseColor.b * darkenFactor, baseColor.a);
        codeNameText.color = darkColor;
        backgroundPointer.color = darkColor;

        foreach (Image slot in slots)
        {
            slot.color = darkColor;
        }

        if (charStats == null)
            charStats = GetComponent<CharStats>();

        foreach (Transform child in abilityContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterAbility ability in character.Abilities)
        {

            AbilityData abilityData = abilityDatabase.GetAbilityData(ability);
            if (abilityData == null)
            {
                Debug.LogWarning($"AbilityData não encontrado para: {ability}");
                continue;
            }

            GameObject obj = Instantiate(abilityTextPrefab, abilityContainer);
            obj.SetActive(true);

            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = abilityData.AbilityName;
                text.color = Color.black;
                text.fontSize = 18;
                text.enabled = true;
            }

            AbilityButtonHandler handler = obj.GetComponentInChildren<AbilityButtonHandler>();
            if (handler != null)
            {
                handler.SetAbilityData(abilityData);                
            }
            
        }
        charStats.Initialize(character);
    }
}
