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

    public IEnumerator StartAttackMinion(DiceRoller dice, ActionManager action, CardDisplayManager cardDisplay)
    {
        Debug.Log("2.Iniciando ataque!");
        dice.ShowDicePanel(false);
        yield return dice.DiceShieldMinion("Minion", action, cardDisplay);
    }

    public void SetDependencies(DiceRoller dice, ActionManager action)
    {
        diceRoller = dice;
    }
}
