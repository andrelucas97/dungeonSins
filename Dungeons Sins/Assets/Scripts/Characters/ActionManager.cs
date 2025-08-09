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

    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private TextMeshProUGUI textActionManager;

    private int maxActions = 3;
    [SerializeField] private int currentAction;
    private int countPoisonous = 0;
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

        var abilities = new Dictionary<CharacterAbility, AbilityInstance>();

        CharacterAbility[] ids =
        {
            CharacterAbility.Taunt,
            CharacterAbility.PerfectArmor,
            CharacterAbility.GlorySun,
            CharacterAbility.ArmorElixir,
            CharacterAbility.StrengthElixir,
            CharacterAbility.Poisonous,
            CharacterAbility.Nap
        };

        foreach (var id in ids)
        {
            var instance = TakeDamage.Instance.GetAbility(id);
            if (instance != null)
                abilities[id] = instance;
        }
        
        if(abilities.TryGetValue(CharacterAbility.Nap, out var nap) && nap.IsActivated)
        {

            playerStat.AdicionalBuffPlayer(nap.Data.BaseValue, nap);

            currentAction = 0;

            CheckEndOfTurn(cardDisplayManager);
            nap.Desactivate();

            return;
        }

        if (abilities.TryGetValue(CharacterAbility.Poisonous, out var poisonous) && poisonous.IsActivated)
        {
            Debug.Log("Veneno ativado.");

            if (poisonous.Data.Duration > countPoisonous)
            {
                CombatLog.Instance.AddMessage($"O veneno atinge o inimigo mais uma vez, levando {poisonous.Data.BaseValue} de dano! +{poisonous.Data.Duration- countPoisonous}turno(s) envenenado.");
                minionStat.ApplyDirectDamage(poisonous.Data.BaseValue);
                countPoisonous++;

                if (poisonous.Data.Duration == countPoisonous)
                {
                    poisonous.Desactivate();
                    countPoisonous = 0;
                }
            }
        }

        if (abilities.TryGetValue(CharacterAbility.Taunt, out var taunt) && taunt.WasUsed)
        {
            playerStat.ClearTempBonus("Shield");
        }

        if (abilities.TryGetValue(CharacterAbility.PerfectArmor, out var perfectArmor) && perfectArmor.WasUsed)
        {
            playerStat.ClearTempBonus("Shield");
        } 
        
        if (abilities.TryGetValue(CharacterAbility.GlorySun, out var sunGlory) && sunGlory.WasUsed)
        {
            playerStat.ClearTempBonus("Damage");
            minionStat.ClearTempDebuffs();
        }

        if (abilities.TryGetValue(CharacterAbility.ArmorElixir, out var armorElixir) && armorElixir.IsActivated)
        {
            playerStat.ClearTempBonus("Shield");
            armorElixir.Desactivate();
        }

        if (abilities.TryGetValue(CharacterAbility.StrengthElixir, out var strengthElixir) && strengthElixir.IsActivated)
        {
            playerStat.ClearTempBonus("Damage");
            strengthElixir.Desactivate();
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

        AbilityInstance invisibilityInstance = TakeDamage.Instance.GetAbility(CharacterAbility.Invisibility);

        if (invisibilityInstance != null && !invisibilityInstance.IsActivated)
            yield return StartCoroutine(HandleMinionAttackThenStartTurn(minionAttack, cardDisplay));

        else
        {
            CombatLog.Instance.AddMessage($"[T{CurrentTurn}] {playerStat.CharData.CodeName} está invisível! {minionStat.CardData.CardName} não conseguiu atacar!"
            );

            yield return new WaitForSeconds(1.5f);

            CallCheckEndOfTurn(cardDisplayManager);
        }


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
