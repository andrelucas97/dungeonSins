using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseStats
{
    int BaseShield { get; }
    int CurrentHealth { get; }
    int BaseDamage { get; }

    void TakeDamageApply(int hitDamage, int damageMultiplier, ActionManager action, CardDisplayManager cardDisplay);

}
