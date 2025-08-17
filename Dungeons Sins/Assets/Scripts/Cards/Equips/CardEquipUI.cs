using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardEquipUI : MonoBehaviour
{
    // VAR PRIVADAS

    [Header("Card Data")]
    //IMG EQUIP
    [SerializeField] private Image artworkImage;
    [SerializeField] private Image backgroudImage;
    // NAME-DESCRIPT EQUIP
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Pointer")]
    // POINTER DEF/ATK
    [SerializeField] private TextMeshProUGUI pointer;
    [Header("Stat")]
    [SerializeField] private TextMeshProUGUI stat;

    [Header("Label")]
    // 1 mão ou 2 mãos
    [SerializeField] private TextMeshProUGUI typeLabel;

    [SerializeField] private CardPreviewManager previewManager;

    private EquipmentCard cardData;
    private CardDisplayManager cardDisplayManager;

    // VAR PUBLICAS
    public EquipmentCard CardData => cardData;
    public TextMeshProUGUI TypeLabel => typeLabel;
    public void Setup(CardData card)
    {
        var cardEquip = card as EquipmentCard;

        if (previewManager != null)
        {
            previewManager.Setup(cardEquip);
        }

        if (cardEquip == null)
        {
            Debug.LogWarning("Setup chamado com CardData que não é EquipmentCard!");
            return;
        }
        cardData = cardEquip;

        artworkImage.sprite = card.Artwork;
        nameText.text = card.CardName;
        descriptionText.text = card.Description;
        backgroudImage.sprite = card.Background;

        pointer.text = cardEquip.ValueBonus.ToString();
        stat.text = cardEquip.CardStat.ToString();        

        switch (cardEquip.CardLabel)
        {
            case CardLabel.OneHand:
                typeLabel.text = "Arma de uma mão";
                break;

            case CardLabel.TwoHands:
                typeLabel.text = "Arma de duas mãos";
                break;
            case CardLabel.LightArmor:
                typeLabel.text = "Armadura Leve";
                break;
            case CardLabel.HeavyArmor:
                typeLabel.text = "Armadura Pesada";
                break;
            case CardLabel.None:
                typeLabel.text = "";
                break;
        }
    }
}
