using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewModifier", menuName = "CardsGame/Cards/Modifier")]
public class ModifierCard : CardData
{
    public int healAmount;
    public int extraDiceRolls;
    public int shieldAmount;    
    public CardMod cardMod;
}
