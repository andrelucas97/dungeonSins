using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionAttack : MonoBehaviour
{

    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private TextMeshProUGUI resultDiceShield;
    [SerializeField] private TextMeshProUGUI resultDiceDamage;
    [SerializeField] private ActionManager actionManager;

    public void StartAttackMinion(DiceRoller dice, ActionManager action, CardDisplayManager cardDisplay)
    {
        Debug.Log("Iniciando ataque!");
        dice.ShowDicePanel(false);
        dice.ButtonDiceShield("minionAttacking", action, cardDisplay);
        dice.ButtonDiceDamage("minionAttacking", action, cardDisplay);
        action.StartTurn();

    }

    public void SetDependencies(DiceRoller dice, ActionManager action)
    {
        diceRoller = dice;
    }
}
