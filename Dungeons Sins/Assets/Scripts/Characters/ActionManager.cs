using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    [Header("Turn")]
    [SerializeField] private TextMeshProUGUI textTurnManager;
    [SerializeField] private int currentTurn = 1;

    [Header("Buttons")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button deckButton;

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
    private int countBurnBaby = 0;
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

            attackButton.interactable = false;
            deckButton.interactable = false;

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

        attackButton.interactable = true;
        deckButton.interactable = true;

        var abilities = new Dictionary<CharacterAbility, AbilityInstance>();

        CharacterAbility[] ids =
        {
            CharacterAbility.Taunt,
            CharacterAbility.PerfectArmor,
            CharacterAbility.GlorySun,
            CharacterAbility.ArmorElixir,
            CharacterAbility.StrengthElixir,
            CharacterAbility.Poisonous,
            CharacterAbility.Nap, 
            CharacterAbility.BurnBabyBurn, 
            CharacterAbility.Fuuton,
            CharacterAbility.StoneEdge
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

        if (abilities.TryGetValue(CharacterAbility.StoneEdge, out var stoneEdge) && stoneEdge.IsActivated)
        {
            stoneEdge.Desactivate();
        }

        if (abilities.TryGetValue(CharacterAbility.Poisonous, out var poisonous) && poisonous.IsActivated)
        {
            if (poisonous.Data.Duration > countPoisonous)
            {
                minionStat.ApplyDirectDamage(poisonous.Data.BaseValue);
                countPoisonous++;
                
                CombatLog.Instance.AddMessage($"O veneno atinge o inimigo mais uma vez, levando {poisonous.Data.BaseValue} de dano! +{poisonous.Data.Duration - countPoisonous}turno(s) envenenado.");

                if (poisonous.Data.Duration == countPoisonous)
                {
                    poisonous.Desactivate();
                    countPoisonous = 0;
                }
            }
        }

        if (abilities.TryGetValue(CharacterAbility.BurnBabyBurn, out var babyBurn) && babyBurn.IsActivated)
        {
            if (babyBurn.Data.Duration > countBurnBaby)
            {

                minionStat.ApplyDirectDamage(babyBurn.Data.BaseValue);
                countBurnBaby++;
               
                CombatLog.Instance.AddMessage($"As chamas do Baby Burn Baby consomem o inimigo novamente, causando {babyBurn.Data.BaseValue} de dano! +{babyBurn.Data.Duration - countBurnBaby} turno(s) queimando.");

                if (babyBurn.Data.Duration == countBurnBaby)
                {
                    babyBurn.Desactivate();
                    countBurnBaby = 0;
                }

            }
        }

        var shieldAbilities = new[]
        {
            CharacterAbility.Taunt,
            CharacterAbility.PerfectArmor,
            CharacterAbility.ArmorElixir,
            CharacterAbility.Fuuton
        };        

        foreach (var abilityID in shieldAbilities)
        {
            if (abilities.TryGetValue(abilityID, out var ability) && (ability.WasUsed || ability.IsActivated))
            {
                playerStat.ClearTempBonus("Shield");

                if (abilityID == CharacterAbility.ArmorElixir)
                    ability.Desactivate();
            }
        }

        var damageAbilities = new[]
        {
            CharacterAbility.GlorySun,
            CharacterAbility.StrengthElixir
        };

        foreach (var abilityID in damageAbilities)
        {
            if (abilities.TryGetValue(abilityID, out var ability) && (ability.WasUsed || ability.IsActivated))
            {
                playerStat.ClearTempBonus("Damage");

                if (abilityID == CharacterAbility.StrengthElixir)
                    ability.Desactivate();

                if (abilityID == CharacterAbility.GlorySun)
                    minionStat.ClearTempDebuffs();
            }
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
        AbilityInstance stoneEdgeInstance = TakeDamage.Instance.GetAbility(CharacterAbility.StoneEdge);

        if ((invisibilityInstance != null && invisibilityInstance.IsActivated) || (stoneEdgeInstance != null && stoneEdgeInstance.IsActivated))
        {
            if (invisibilityInstance != null && invisibilityInstance.IsActivated)
            {
                CombatLog.Instance.AddMessage($"[T{CurrentTurn}] {playerStat.CharData.CodeName} está invisível! {minionStat.CardData.CardName} não conseguiu atacar!");
                
            } else if (stoneEdgeInstance != null && stoneEdgeInstance.IsActivated)
            {
                CombatLog.Instance.AddMessage($"[T{CurrentTurn}] {minionStat.CardData.CardName} está atordoado! Falha ao atacar.");
            }
            yield return new WaitForSeconds(1.5f);

            CallCheckEndOfTurn(cardDisplayManager);
        } else
        {
            yield return StartCoroutine(HandleMinionAttackThenStartTurn(minionAttack, cardDisplay));
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
