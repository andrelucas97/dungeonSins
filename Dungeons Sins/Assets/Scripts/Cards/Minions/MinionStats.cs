using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionStats : MonoBehaviour
{
    // VAR PRIVADAS

    [Header("Stats")]
    [SerializeField] private int levelMinion;
    [SerializeField] private int currentHealth;
    [SerializeField] private int shield;
    [SerializeField] private int damage;

    [Header("Canvas")]
    [SerializeField] private TextMeshProUGUI messageStats;

    private CardData cardData;
    private MinionLevelStats cardLevelStats;

    // VAR PUBLICAS
    public int CurrentHealth => currentHealth;
    public int Shield => shield;
    public int Damage => damage;
    public void Initialize(MinionsCard card)
    {
        cardData = card;
        ApplyStats(card);
        StatusDisplay.Instance.AttStatusMinion(this);
    }

    public void TakeDamage(int hitDamage, int damageMultiplier)
    {
        currentHealth -= (hitDamage * damageMultiplier);

        StatusDisplay.Instance.AttStatusMinion(this);

        if (currentHealth <= 0)
        {
            currentHealth = Mathf.Max(currentHealth, 0);
            Debug.Log($"{cardData.CardName} foi derrotado.");
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
