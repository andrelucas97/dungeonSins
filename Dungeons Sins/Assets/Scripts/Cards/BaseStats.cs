using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseStats
{
    int Shield { get; }
    int CurrentHealth { get; }
    int Damage { get; }

    void TakeDamage(int hitDamage, int damageMultiplier, ActionManager action, CardDisplayManager cardDisplay);

}
