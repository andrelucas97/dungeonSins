using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "CardsGame/Cards/Equipment")]
public class EquipmentCard : CardData
{
    // VAR PRIVADAS
    [Header("Bonus")]
    [SerializeField] private CardStats cardStat;
    [SerializeField] private int attackBonus;
    [SerializeField] private int defenseBonus;

    [Header("Type")]
    [SerializeField] private TypeCardEquip typeCardEquip;

    [Header("Label")]
    [SerializeField] private CardLabel cardLabel;

    // VAR PUBLICAS
    public CardStats CardStat => cardStat;
    public int AttackBonus => attackBonus;
    public int DefenseBonus => defenseBonus;
    public TypeCardEquip TypeCard => typeCardEquip;
    public CardLabel CardLabel => cardLabel;

}
