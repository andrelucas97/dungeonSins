using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class CharUI : MonoBehaviour
{
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI codeNameText;

    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI shield;

    [SerializeField] private TextMeshProUGUI damage;

    // Cor Correspondente ao Personagem
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image backgroundPointer;

    [SerializeField] private Image[] slots;

    private CharStats charStats;

    public void Setup(CharacterData character)
    {
        artworkImage.sprite = character.Portrait;
        nameText.text = character.CharName;
        codeNameText.text = character.CodeName;
        health.text = character.MaxHealth.ToString();
        shield.text = character.Shield.ToString();
        damage.text = character.Damage.ToString();

        Color baseColor = charColors.GetColor(character.CharColor);
        backgroundImage.color = baseColor;

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

        charStats.Initialize(character);

    }
}
