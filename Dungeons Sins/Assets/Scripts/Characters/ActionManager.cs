using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private int maxActions = 3;
    private int currentAction;

    [SerializeField] private CharStats playerStat;
    [SerializeField] private StatusDisplay displayStats;
    [SerializeField] private MinionAttack attackMinion;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private GameObject turnMinionBox;
    [SerializeField] private TextMeshProUGUI textTurnMinion;

    [SerializeField] private TextMeshProUGUI textActionManager;

    void Start()
    {
        if (attackMinion == null)
        {
            attackMinion = FindObjectOfType<MinionAttack>();
        }

        StartTurn();
    }

    public void StartTurn()
    {
        InitializeTurn();
    }
    public bool TryUseAction()
    {

        if (currentAction > 0)
        {
            ExecuteAction();
            return true;
        }
        else
        {
            Debug.Log("Sem ações disponiveis.");
            return false;
        }
    }
    public void CheckEndOfTurn(CardDisplayManager cardDisplay)
    {
        if (currentAction == 0)
        {
            turnMinionBox.SetActive(true);
            textTurnMinion.text = "TURNO DO LACAIO!!";

            StartCoroutine(StartEnemy(attackMinion, cardDisplay));
        } else
        {
            Debug.Log($"Restam {currentAction} jogada(s)");
        }
    }
    private void InitializeTurn()
    {
        currentAction = maxActions;
        UpdateTextAction(currentAction);
    }
    private void ExecuteAction()
    {
        currentAction--;
        UpdateTextAction(currentAction);
    }
    private IEnumerator StartEnemy(MinionAttack minionAttack, CardDisplayManager cardDisplay)
    {
        yield return new WaitForSeconds(2f);
        turnMinionBox.SetActive(false);
        minionAttack.StartAttackMinion(diceRoller, this, cardDisplay);
    }
    private void UpdateTextAction(int currentAction)
    {
        textActionManager.text = ($"Ações: {currentAction}");

    }



}
