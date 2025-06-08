using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharUI : MonoBehaviour
{
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI codeNameText;

    public TextMeshProUGUI health;
    public TextMeshProUGUI shield;

    public TextMeshProUGUI damage;

    // Cor Correspondente ao Personagem
    public Image backgroundImage;
    public Image backgroundPointer;

    public Image[] slots;
    public void Setup(CharacterData character)
    {
        artworkImage.sprite = character.portrait;
        nameText.text = character.charName;
        codeNameText.text = character.codeName;
        health.text = character.maxHealth.ToString();
        shield.text = character.shield.ToString();
        damage.text = character.damage.ToString();

        Color baseColor = charColors.GetColor(character.charColor);
        backgroundImage.color = baseColor;

        float darkenFactor = 0.65f;
        Color darkColor = new Color(baseColor.r * darkenFactor, baseColor.g * darkenFactor, baseColor.b * darkenFactor, baseColor.a);
        codeNameText.color = darkColor;
        backgroundPointer.color = darkColor;

        foreach (Image slot in slots)
        {
            slot.color = darkColor;
        }
    }
}
