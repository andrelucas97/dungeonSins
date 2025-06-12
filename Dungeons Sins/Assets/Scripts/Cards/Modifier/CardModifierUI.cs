using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardModifierUI : MonoBehaviour, IPointerClickHandler
{
    // VAR PRIVADAS

    [Header("Card Data")]
    [SerializeField] private Image artworkImage;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Effect")]
    [SerializeField] private TextMeshProUGUI effectDescript;

    private ModifierCard cardData;

    // VAR PUBLICAS
    public ModifierCard CardData => cardData;

    public void Setup(CardData card)
    {
        artworkImage.sprite = card.Artwork;
        nameText.text = card.CardName;
        descriptionText.text = card.Description;
        
        var modifierCard = card as ModifierCard;

        cardData = modifierCard;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SheetPlayer sheet = FindObjectOfType<SheetPlayer>();
        sheet.EquipCard(gameObject, cardData, cardData.TypeCard);
    }

}
