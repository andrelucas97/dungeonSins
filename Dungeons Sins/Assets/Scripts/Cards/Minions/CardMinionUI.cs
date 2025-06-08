using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMinionUI : MonoBehaviour
{
    public CardData cardData;
    //IMG EQUIP
    public Image artworkImage;

    // NAME-DESCRIPT EQUIP
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    // NIVEL 1
    public TextMeshProUGUI healthN1Text;
    public TextMeshProUGUI shieldN1Text;
    public TextMeshProUGUI damageN1Text;
    // NIVEL 2
    public TextMeshProUGUI healthN2Text;
    public TextMeshProUGUI shieldN2Text;
    public TextMeshProUGUI damageN2Text;
    //NIVEL 3
    public TextMeshProUGUI healthN3Text;
    public TextMeshProUGUI shieldN3Text;
    public TextMeshProUGUI damageN3Text;

    public GameObject elementObj;
    //IMG ELEMENT
    public Image elementImage;
    public void Setup(CardData card)
    {
        cardData = card;

        artworkImage.sprite = card.artwork;
        nameText.text = card.cardName;
        descriptionText.text = card.description;

        MinionsCard minion = card as MinionsCard;        

        elementImage.sprite = minion.elementImage;

        healthN1Text.text = minion.level1.health.ToString();
        shieldN1Text.text = minion.level1.shield.ToString();
        damageN1Text.text = minion.level1.damage.ToString();

        healthN2Text.text = minion.level2.health.ToString();
        shieldN2Text.text = minion.level2.shield.ToString();
        damageN2Text.text = minion.level2.damage.ToString();

        healthN3Text.text = minion.level3.health.ToString();
        shieldN3Text.text = minion.level3.shield.ToString();
        damageN3Text.text = minion.level3.damage.ToString();

        if (minion.elementType == ElementType.None)
        {
            elementObj.SetActive(false);
        }
    }
}
