using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardModifierUI : MonoBehaviour
{
    public Image artworkImage;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI effectDescript;

    public void Setup(CardData card)
    {
        artworkImage.sprite = card.artwork;
        nameText.text = card.cardName;
        descriptionText.text = card.description;

        ModifierCard modifierCard = card as ModifierCard;


    }
}
