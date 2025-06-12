using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinionLevelStats
{
    // VAR PRIVADAS
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private int shield;
    [SerializeField] private int damage;

    // VAR PUBLICAS
    public int Health => health;
    public int Shield => shield;
    public int Damage => damage;    
}
