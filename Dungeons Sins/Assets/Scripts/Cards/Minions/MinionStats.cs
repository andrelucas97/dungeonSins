using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionStats : MonoBehaviour
{
    [SerializeField] private int levelMinion;

    [SerializeField] private int currentHealth;
    [SerializeField] private int shield;
    [SerializeField] private int damage;

    private MinionsCard baseData;

    public void Initialize(MinionsCard card)
    {

        baseData = card;
        ApplyStats(card);
        Debug.Log("Minion iniciado com HP: " + currentHealth);
    }

    private void ApplyStats(MinionsCard card)
    {
        switch (levelMinion)
        {
            case 1:
                currentHealth = card.level1.health;
                shield = card.level1.shield;
                damage = card.level1.damage;
                break;
            case 2:
                currentHealth = card.level2.health;
                shield = card.level2.shield;
                damage = card.level2.damage;
                break;
            case 3:
                currentHealth = card.level3.health;
                shield = card.level3.shield;
                damage = card.level3.damage;
                break;

            default:
                Debug.LogWarning("Minion level inválido. Usando nível 1 como padrão.");
                currentHealth = card.level1.health;
                break;


        }
    }
}
