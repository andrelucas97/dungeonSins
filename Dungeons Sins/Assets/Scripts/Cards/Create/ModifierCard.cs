using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewModifier", menuName = "CardsGame/Cards/Modifier")]
public class ModifierCard : CardData
{
    // VAR PRIVADAS
    [Header("Effect Description")]
    [SerializeField] private string effectDescription;

    [Header("Stats")]
    [SerializeField] private int healAmount;
    [SerializeField] private int extraDiceRolls;
    [SerializeField] private int shieldAmount;
    [SerializeField] private CardMod cardMod;

    [Header("Card Type")]
    [SerializeField] private TypeCardEquip typeCard;

    // VAR PUBLICAS
    public TypeCardEquip TypeCard => typeCard;
    public string EffectDescription => effectDescription;
}
