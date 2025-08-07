using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Turn")]
    [SerializeField] private TextMeshProUGUI textTurnManager;
    [SerializeField] private int currentTurn = 1;

    [Header("Action")]
    [SerializeField] private CharStats playerStat;
    [SerializeField] private MinionStats minionStat;
    //[SerializeField] private StatusDisplay displayStats;
    [SerializeField] private MinionAttack attackMinion;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private GameObject turnMinionBox;
    [SerializeField] private TextMeshProUGUI textTurnMinion;

    [SerializeField] private TextMeshProUGUI textActionManager;

    private int maxActions = 3;
    [SerializeField] private int currentAction;
    public int CurrentTurn => currentTurn;

    void Start()
    {
        if (attackMinion == null)
        {
            attackMinion = FindObjectOfType<MinionAttack>();
        }

        if (minionStat == null)
            minionStat = FindObjectOfType<MinionStats>();
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
            CombatLog.Instance.AddMessage("TURNO DO LACAIO!!");
        
            StartCoroutine(StartEnemy(attackMinion, cardDisplay));
        }
        else
        {

            CombatLog.Instance.AddMessage($"<align=center>---- Resta(m) { currentAction} jogada(s) ----</align>");
        }
    }
    private void InitializeTurn()
    {
        currentTurn++;
        currentAction = maxActions;

        AbilityInstance tauntInstance = TakeDamage.Instance.GetAbility(CharacterAbility.Taunt);
        AbilityInstance perfectArmorInstance = TakeDamage.Instance.GetAbility(CharacterAbility.PerfectArmor);
        AbilityInstance sunGloryInstance = TakeDamage.Instance.GetAbility(CharacterAbility.GlorySun);
        AbilityInstance armorElixirInstance = TakeDamage.Instance.GetAbility(CharacterAbility.ArmorElixir);
        AbilityInstance strengthElixirInstance = TakeDamage.Instance.GetAbility(CharacterAbility.StrengthElixir);


        if (tauntInstance != null && tauntInstance.WasUsed)
        {
            playerStat.ClearTempBonus("Shield");
        }

        if (perfectArmorInstance != null && perfectArmorInstance.WasUsed)
        {
            playerStat.ClearTempBonus("Shield");
        } 
        
        if (sunGloryInstance != null && sunGloryInstance.WasUsed)
        {
            playerStat.ClearTempBonus("Damage");
            minionStat.ClearTempDebuffs();
        }

        if (armorElixirInstance != null && armorElixirInstance.IsActivated)
        {
            playerStat.ClearTempBonus("Shield");
            armorElixirInstance.Desactivate();
        }

        if (strengthElixirInstance != null && strengthElixirInstance.IsActivated)
        {
            playerStat.ClearTempBonus("Damage");
            strengthElixirInstance.Desactivate();
        }

        ResetAbilitiesForPlayer();        

        UpdateTextAction(currentAction, currentTurn);
    }

    private void ResetAbilitiesForPlayer()
    {

        if (playerStat == null) return;

        foreach (var ability in playerStat.Abilities)
        {
            ability.ResetAbilities();
        }
    }

    private void ExecuteAction()
    {
        currentAction--;
        UpdateTextAction(currentAction, currentTurn);
    }
    private IEnumerator StartEnemy(MinionAttack minionAttack, CardDisplayManager cardDisplay)
    {
        yield return new WaitForSeconds(2f);
        turnMinionBox.SetActive(false);

        yield return StartCoroutine(HandleMinionAttackThenStartTurn(minionAttack, cardDisplay));
    }

    private IEnumerator HandleMinionAttackThenStartTurn(MinionAttack minionAttack, CardDisplayManager cardDisplay)
    {
        yield return minionAttack.StartAttackMinion(diceRoller, this, cardDisplay);
    }

    private void UpdateTextAction(int currentAction, int currentTurn)
    {
        textTurnManager.text = $"TURNO {currentTurn}";
        textActionManager.text = $"AÇÕES: {currentAction}";

    }

    internal void CallCheckEndOfTurn(CardDisplayManager cardDisplayManager)
    {

        if (currentAction == 0)
            StartTurn();

        CheckEndOfTurn(cardDisplayManager);
    }
}
