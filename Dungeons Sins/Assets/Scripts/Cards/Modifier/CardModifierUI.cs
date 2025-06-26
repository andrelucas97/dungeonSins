using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardModifierUI : MonoBehaviour
{
    // VAR PRIVADAS

    [Header("Card Data")]
    [SerializeField] private Image artworkImage;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Effect")]
    [SerializeField] private TextMeshProUGUI effectDescript;

    private ModifierCard cardData;

    // Arrastando Cards
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector2 originalPosition;
    private LayoutElement layoutElement;
    private Canvas canvas;
    private CanvasGroup canvasGroup;


    // VAR PUBLICAS
    public ModifierCard CardData => cardData;

    public void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(CardData card)
    {      


        artworkImage.sprite = card.Artwork;
        nameText.text = card.CardName;
        descriptionText.text = card.Description;
        
        var modifierCard = card as ModifierCard;

        cardData = modifierCard;

    }

}
