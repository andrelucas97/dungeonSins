using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMinionUI : MonoBehaviour
{
    // VAR PRIVADAS

    [Header("Card Data")]
    [SerializeField] private CardData cardData;
    //IMG EQUIP
    [SerializeField] private Image artworkImage;

    // NAME-DESCRIPT EQUIP
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Level Stats")]
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

    [Header("Element    ")]
    [SerializeField] private GameObject elementObj;
    //IMG ELEMENT
    [SerializeField] private Image elementImage;

    private MinionStats minionStat;
    public void Setup(CardData card)
    {
        cardData = card;

        artworkImage.sprite = card.Artwork;
        nameText.text = card.CardName;
        descriptionText.text = card.Description;

        MinionsCard minion = card as MinionsCard;        

        elementImage.sprite = minion.ElementImage;

        healthN1Text.text = minion.Level1.Health.ToString();
        shieldN1Text.text = minion.Level1.Shield.ToString();
        damageN1Text.text = minion.Level1.Damage.ToString();

        healthN2Text.text = minion.Level2.Health.ToString();
        shieldN2Text.text = minion.Level2.Shield.ToString();
        damageN2Text.text = minion.Level2.Damage.ToString();

        healthN3Text.text = minion.Level3.Health.ToString();
        shieldN3Text.text = minion.Level3.Shield.ToString();
        damageN3Text.text = minion.Level3.Damage.ToString();

        if (minion.ElementType == ElementType.None)
        {
            elementObj.SetActive(false);
        }

        if (minionStat == null)
            minionStat = GetComponent<MinionStats>();

        minionStat.Initialize(minion);
    }
}
