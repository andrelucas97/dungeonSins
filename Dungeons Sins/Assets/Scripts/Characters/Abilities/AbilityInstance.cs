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
    public bool WasUsed => _wasUsed;
    public static AbilityInstance Instance;
    public AbilityInstance(AbilityData data)
    {
        _data = data;
        _isActivated = false;
        _wasUsed = false;
    }
    // FUNCAO PUBLICA
    public void IsActivate()
    {
        ActiveAbility();
    }
    public void Desactivate()
    {
        DesactivateAbility();
    }

    public bool TryThisTurn()
    {
        return UseThisTurn();
    }

    public void ResetAbilities()
    {
        ResetTurn();
    }
    // FUNCAO PRIVADA
    private void DesactivateAbility()
    {
        _isActivated = false;
    }

    private void ActiveAbility()
    {
        _isActivated = true;
    }
    private bool UseThisTurn()
    {
        if (_wasUsed) return false;

        _wasUsed = true;
        return true;
    }
    private void ResetTurn()
    {
        _wasUsed = false;
    }
}
