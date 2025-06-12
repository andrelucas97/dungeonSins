using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewModifier", menuName = "CardsGame/Cards/Modifier")]
public class ModifierCard : CardData
{
    // VAR PRIVADAS
    [Header("")]
    [SerializeField] private int healAmount;
    [SerializeField] private int extraDiceRolls;
    [SerializeField] private int shieldAmount;
    [SerializeField] private CardMod cardMod;

    [Header("Card Type")]
    [SerializeField] private TypeCardEquip typeCard;

    // VAR PUBLICAS
    public TypeCardEquip TypeCard => typeCard;
}
