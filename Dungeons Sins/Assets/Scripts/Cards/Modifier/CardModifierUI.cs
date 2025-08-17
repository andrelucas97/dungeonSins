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
    [SerializeField] private Image backgroundImage;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Effect")]
    [SerializeField] private TextMeshProUGUI effectDescript;

    [Header("Card Preview")]
    [SerializeField] private CardPreviewManager previewManager;

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
        var modifierCard = card as ModifierCard;
        if (previewManager != null)
        {
            previewManager.Setup(modifierCard);
        }

        artworkImage.sprite = card.Artwork;
        nameText.text = card.CardName;
        descriptionText.text = card.Description;
        backgroundImage.sprite = card.Background;


        cardData = modifierCard;

    }

}
