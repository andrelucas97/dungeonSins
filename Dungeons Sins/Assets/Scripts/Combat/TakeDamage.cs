using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private CharStats playerCard;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private GameObject buttonAttack;

    private MinionStats minionStat;
    public void OnAttackButton()
    {
        if (playerCard != null)
        {
            if (minionStat == null)
            {
                minionStat = FindObjectOfType<MinionStats>();
            }

            buttonAttack.GetComponent<Button>().interactable = false;
            diceRoller.ShowDicePanel();            
        }
    }

}
