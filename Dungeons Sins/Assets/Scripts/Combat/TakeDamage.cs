using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private CharStats playerCard;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private GameObject buttonAttack;
    [SerializeField] private GameObject textShield;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private CharacterAbility activeAbility;

    private MinionStats minionStat;

    void Awake()
    {
        actionManager = FindObjectOfType<ActionManager>();

        if (diceRoller == null)
        {
            diceRoller = FindObjectOfType<DiceRoller>();
            if (diceRoller == null)
                Debug.LogWarning("DiceRoller não encontrado na cena!");
        }
    }

    public void OnAttackButton()
    {
        Debug.Log("[TakeDamage] OnAttackButton chamado");

        if (actionManager == null)
        {
            Debug.LogWarning("actionManager está nulo!");
            return;
        }

        Debug.Log("actionManager está OK, testando cardDisplayManager...");

        if (playerCard != null)
        {
            if (minionStat == null)
            {
                minionStat = FindObjectOfType<MinionStats>();
            }

            bool hasAction = actionManager.TryUseAction();

            if (!hasAction) return;

            if (buttonAttack != null)
            {
                buttonAttack.GetComponent<Button>().interactable = false;
            }
            else
            {
                Debug.LogWarning("buttonAttack está nulo no TakeDamage.");
            }

            if (diceRoller != null && diceRoller.TextShield != null)
            {
                diceRoller.TextShield.SetActive(true);
                diceRoller.TextDamage.SetActive(false);
                diceRoller.ShowDicePanel(true);
            }
            else
            {
                Debug.LogWarning("diceRoller ou TextShield/TextDamage está nulo!");
            }

            diceRoller.TextShield.SetActive(true);
            diceRoller.TextDamage.SetActive(false);
            diceRoller.ShowDicePanel(true);
        }
    }

    public void ButtonAbility()
    {
        switch (activeAbility)
        {
            case CharacterAbility.Berserker:
                Debug.Log("Ativando Habilidade " + CharacterAbility.Berserker);
                break;
        }
    }
    public void SetActionManager(ActionManager manager)
    {
        actionManager = manager;
    }
    public void SetAttackButton(Button button)
    {
        buttonAttack = button.gameObject;
    }

}
