using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionStats : MonoBehaviour, BaseStats
{
    // VAR PRIVADAS
    [SerializeField] private MinionsCard minionData;

    [Header("Stats")]
    [SerializeField] private int levelMinion;

    [SerializeField] private int currentHealth;
    [SerializeField] private int shield;
    [SerializeField] private int damage;

    [Header("Canvas")]
    [SerializeField] private TextMeshProUGUI messageStats;

    private CardData cardData;
    private MinionLevelStats cardLevelStats;
    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private ActionManager actionManager;

    // VAR PUBLICAS
    public int CurrentHealth => currentHealth;
    public int Shield => shield;
    public int Damage => damage;
    public CardData CardData => cardData;
    public void Initialize(MinionsCard card)
    {

        if (actionManager == null)
        {
            actionManager = FindObjectOfType<ActionManager>();
        }
        cardData = card;
        ApplyStats(card);
        StatusDisplay.Instance.AttStatusMinion(this, cardData);
        CombatLog.Instance.AddMessage($"{card.CardName} | ATK: {card.Level1.Damage} | DEF: {card.Level1.Shield} | Vida: {card.Level1.Health}");       

    }

    public void TakeDamage(int hitDamage, int resultDie, ActionManager action, CardDisplayManager cardDisplay)
    {
        ApplyDamage(hitDamage, resultDie);        
    }

    private void ApplyDamage(int hitDamage, int resultDie)
    {
        currentHealth -= hitDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        StatusDisplay.Instance.AttStatusMinion(this, cardData);        

        if (currentHealth == 0)
        {
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {cardData.CardName} foi derrotado.");

            if (cardDisplayManager == null)
                cardDisplayManager = FindObjectOfType<CardDisplayManager>();

            cardDisplayManager.SpawnMinion();
        }
    }

    private void ApplyStats(MinionsCard card)
    {
        switch (levelMinion)
        {
            case 1:
                currentHealth = card.Level1.Health;
                shield = card.Level1.Shield;
                damage = card.Level1.Damage;
                break;
            case 2:
                currentHealth = card.Level2.Health;
                shield = card.Level2.Shield;
                damage = card.Level2.Damage;
                break;
            case 3:
                currentHealth = card.Level3.Health;
                shield = card.Level3.Shield;
                damage = card.Level3.Damage;
                break;

            default:
                Debug.LogWarning("Minion level inválido. Usando nível 1 como padrão.");
                currentHealth = card.Level1.Health;
                break;


        }
    }
}
