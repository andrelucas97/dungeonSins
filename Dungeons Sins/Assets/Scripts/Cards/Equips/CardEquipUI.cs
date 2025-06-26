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
    [SerializeField] private TextMeshProUGUI handLabel;    

    private EquipmentCard cardData;
    private CardDisplayManager cardDisplayManager;

    // VAR PUBLICAS


    // VAR PUBLICAS
    public EquipmentCard CardData => cardData;

    public void Setup(CardData card)
    {
        var cardEquip = card as EquipmentCard;

        if (cardEquip == null)
        {
            Debug.LogWarning("Setup chamado com CardData que não é EquipmentCard!");
            return;
        }
        cardData = cardEquip;

        artworkImage.sprite = card.Artwork;
        nameText.text = card.CardName;
        descriptionText.text = card.Description;

        switch (cardEquip.CardStat)
        {
            case (CardStats.DEF):
                stat.text = CardStats.DEF.ToString();
                pointer.text = cardEquip.DefenseBonus.ToString();
                break;
            case (CardStats.ATK):
                stat.text = CardStats.ATK.ToString();
                pointer.text = cardEquip.AttackBonus.ToString();
                break;
        }

        switch (cardEquip.CardLabel)
        {
            case CardLabel.OneHand:
                handLabel.text = "Arma de uma mão";
                break;

            case CardLabel.TwoHands:
                handLabel.text = "Arma de duas mãos";
                break;
            case CardLabel.LightArmor:
                handLabel.text = "Armadura Leve";
                break;
            case CardLabel.HeavyArmor:
                handLabel.text = "Armadura Pesada";
                break;
            case CardLabel.None:
                handLabel.text = "";
                break;
        }
    }
}
