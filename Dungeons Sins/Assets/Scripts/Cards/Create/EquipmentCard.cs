using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "CardsGame/Cards/Equipment")]
public class EquipmentCard : CardData
{
    public int attackBonus;
    public int defenseBonus;

    public TypeCardEquip typeCardEquip;

    public CardStats cardStat;
    public CardLabel cardLabel;


}
