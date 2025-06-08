using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Equipment,
    Modifier,
    Minion
}

//Equip
public enum TypeCardEquip
{
    Boots,
    Armor,
    Helmet,
    Weapon,
    Modifier
}

public enum CardLabel 
{
    OneHand,
    TwoHands,
    LightArmor,
    HeavyArmor,
    None
}
//Equip
public enum CardStats
{
    DEF,
    ATK
}

//Modifier
public enum CardMod
{
    healAmount,
    extraRoll,
    shieldAmount
}

public enum ElementType
{
    None,
    Fire,
    Water,
    Earth,
    Air
}
