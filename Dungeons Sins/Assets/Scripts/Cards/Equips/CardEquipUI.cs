using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardEquipUI : MonoBehaviour, IPointerClickHandler
{
    //IMG EQUIP
    public Image artworkImage;

    // NAME-DESCRIPT EQUIP
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    // POINTER DEF/ATK
    public TextMeshProUGUI pointer;
    public TextMeshProUGUI stat;

    // 1 mão ou 2 mãos
    public TextMeshProUGUI handLabel;

    private EquipmentCard cardData;
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

        artworkImage.sprite = card.artwork;
        nameText.text = card.cardName;
        descriptionText.text = card.description;

        switch (cardEquip.cardStat)
        {
            case (CardStats.DEF):
                stat.text = CardStats.DEF.ToString();
                pointer.text = cardEquip.defenseBonus.ToString();
                break;
            case (CardStats.ATK):
                stat.text = CardStats.ATK.ToString();
                pointer.text = cardEquip.attackBonus.ToString();
                break;
        }

        switch (cardEquip.cardLabel)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        SheetPlayer sheet = FindObjectOfType<SheetPlayer>();
        sheet.EquipCard(gameObject, cardData, cardData.typeCardEquip);
    }
}
