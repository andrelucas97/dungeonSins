using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class AbilityInstance
{
    //VAR PRIVADAS
    private AbilityData _data;
    private bool _isActivated;
    private float _cooldownRemaining;
    private bool _wasUsed;

    //VAR PUBLICAS
    public AbilityData Data => _data;
    public bool IsActivated => _isActivated;
    public float CooldownRemaining => _cooldownRemaining;
    public bool WasUsed => _wasUsed;
    public static AbilityInstance Instance;
    public AbilityInstance(AbilityData data)
    {
        _data = data;
        _isActivated = false;
        _cooldownRemaining = 0f;
        _wasUsed = false;
    }

    public void IsActivate()
    {
        ActiveAbility();
    }
    public void Desactivate()
    {
        DesactivateAbility();
    }

    private void DesactivateAbility()
    {
        _isActivated = false;
    }

    private void ActiveAbility()
    {
        _isActivated = true;
    }
}
