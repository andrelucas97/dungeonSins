using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private int maxActions = 3;
    private int currentAction;

    [SerializeField] private CharStats playerStat;
    [SerializeField] private StatusDisplay displayStats;

    void Start()
    {
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
            Debug.Log("Sem a��es disponiveis.");
            return false;
        }
    }

    private void ExecuteAction()
    {
        currentAction--;
        Debug.Log("A��es: " + currentAction);
    }

    private void InitializeTurn()
    {
        currentAction = maxActions;
        Debug.Log("Novo turno! A��es dispon�veis: " + currentAction);
    }


}
