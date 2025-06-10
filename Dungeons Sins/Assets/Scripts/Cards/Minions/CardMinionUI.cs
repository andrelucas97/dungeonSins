using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMinionUI : MonoBehaviour
{
    [SerializeField] private CardData cardData;
    //IMG EQUIP
    [SerializeField] private Image artworkImage;

    // NAME-DESCRIPT EQUIP
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // NIVEL 1
    [SerializeField] private TextMeshProUGUI healthN1Text;
    [SerializeField] private TextMeshProUGUI shieldN1Text;
    [SerializeField] private TextMeshProUGUI damageN1Text;
    // NIVEL 2
    [SerializeField] private TextMeshProUGUI healthN2Text;
    [SerializeField] private TextMeshProUGUI shieldN2Text;
    [SerializeField] private TextMeshProUGUI damageN2Text;
    //NIVEL 3
    [SerializeField] private TextMeshProUGUI healthN3Text;
    [SerializeField] private TextMeshProUGUI shieldN3Text;
    [SerializeField] private TextMeshProUGUI damageN3Text;

    [SerializeField] private GameObject elementObj;
    //IMG ELEMENT
    [SerializeField] private Image elementImage;

    private MinionStats minionStat;
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

        if (minionStat == null)
            minionStat = GetComponent<MinionStats>();

        minionStat.Initialize(minion);
    }
}
