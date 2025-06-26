using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    // VAR PRIVADAS
    [Header("Data base")]
    [SerializeField] private string cardName;
    [SerializeField] private string description;
    [SerializeField] private Sprite artwork;
    [SerializeField] private CardType cardType;


    // VAR PUBLICAS
    public string CardName => cardName;
    public string Description => description;
    public Sprite Artwork => artwork;
    public CardType CardType => cardType;
    
}
