using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public CardType cardType;
}
