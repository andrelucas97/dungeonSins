using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    public static TakeDamage Instance;

    [SerializeField] private CharStats playerCard;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private GameObject buttonAttack;
    [SerializeField] private GameObject textShield;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private CharacterAbility activeAbility;

    private MinionStats minionStat;
    private Dictionary<CharacterAbility, Action<AbilityData>> abilityActions;


    void Awake()
    {
        Instance = this;
        actionManager = FindObjectOfType<ActionManager>();

        abilityActions = new Dictionary<CharacterAbility, Action<AbilityData>>
        {
            { CharacterAbility.CriticalArrow, UseCriticalArrow },
            { CharacterAbility.LightArrow, UseLightArrow },
            { CharacterAbility.HurricaneArrow, UseHurricaneArrow },
        };

        if (diceRoller == null)
        {
            diceRoller = FindObjectOfType<DiceRoller>();
            if (diceRoller == null)
                Debug.LogWarning("DiceRoller não encontrado na cena!");
        }
    }


    public void OnAttackButton()
    {

        if (actionManager == null)
        {
            Debug.LogWarning("actionManager está nulo!");
            return;
        }

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
                diceRoller.ShowDicePanel(true);
            }
            else
            {
                Debug.LogWarning("diceRoller ou TextShield/TextDamage está nulo!");
            }

            diceRoller.TextShield.SetActive(true);
            diceRoller.ShowDicePanel(true);
        }
    }

    public void UseAbility(AbilityData abilityData)
    {
        if (abilityData == null)
        {
            Debug.LogWarning("AbilityData é nulo!");
            return;
        }

        CharacterAbility ability = abilityData.AbilityID;

        if (abilityActions.TryGetValue(ability, out Action<AbilityData> action))
        {
            action.Invoke(abilityData);
        }
        else
        {
            Debug.LogWarning("Nenhuma ação definida para a habilidade: " + ability);
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

    // HABILIDADES
    #region ABILITIES
    private void UseHurricaneArrow(AbilityData abilityData)
    {
        Debug.Log($"Usou {abilityData.AbilityName}! Acerta vários inimigos.");
    }

    private void UseLightArrow(AbilityData abilityData)
    {
        Debug.Log($"Usou {abilityData.AbilityName}! Ataca e ilumina o alvo.");
    }

    private void UseCriticalArrow(AbilityData abilityData)
    {
        Debug.Log($"Usou {abilityData.AbilityName}! Aplica dano crítico.");
    }
    #endregion
}
