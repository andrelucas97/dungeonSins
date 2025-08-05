using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    // VAR PRIVADAS
    [SerializeField] private CardDisplayManager cardDisplayManager;

    [Header("Player")]
    [SerializeField] private CharStats playerCard;

    [Header("Dice")]
    [SerializeField] private DiceRoller diceRoller;

    [Header("Button Attack")]
    [SerializeField] private GameObject buttonAttack;

    [Header("Shield Text")]
    [SerializeField] private GameObject textShield;
    [SerializeField] private ActionManager actionManager;    

    [Header("Abilities")]
    [SerializeField] private GameObject boxMessageAbility;
    private Func<AbilityInstance, IEnumerator> currentAbilityAction;
    private AbilityInstance currentAbilityInstance;
    private Dictionary<CharacterAbility, Func<AbilityInstance, IEnumerator>> abilityActions;

    [Header("Minion")]
    [SerializeField] private MinionStats minionStat;

    // VAR PUBLICAS
    public static TakeDamage Instance;
    public AbilityInstance CurrentAbilityInstance => currentAbilityInstance;
    public AbilityData CurrentAbilityData => currentAbilityInstance?.Data;

    void Awake()
    {
        Instance = this;
        actionManager = FindObjectOfType<ActionManager>();        

        abilityActions = new Dictionary<CharacterAbility, Func<AbilityInstance, IEnumerator>>
        {
            { CharacterAbility.CriticalArrow, UseCriticalArrow },
            { CharacterAbility.LightArrow, UseLightArrow },
            { CharacterAbility.HurricaneArrow, UseHurricaneArrow },
            { CharacterAbility.ArmorElixir, UseArmorElixir },
            { CharacterAbility.Berseker, UseBerseker },
            { CharacterAbility.BurnBabyBurn, UseBurnBabyBurn },
            { CharacterAbility.DaggerThrow, UseDaggerThrow },
            { CharacterAbility.Devour, UseDevour },
            { CharacterAbility.Elementalist, UseElementalist },
            { CharacterAbility.Fuuton, UseFuuton },
            { CharacterAbility.GlorySun, UseGlorySun },
            { CharacterAbility.Heal, UseHeal },
            { CharacterAbility.Invisibility, UseInvisibility },
            { CharacterAbility.Leroy, UseLeroy },
            { CharacterAbility.LuckTide, UseLuckTide },
            { CharacterAbility.MassGrowth, UseMassGrowth },
            { CharacterAbility.Nap, UseNap },
            { CharacterAbility.PerfectArmor, UsePerfectArmor },
            { CharacterAbility.Poisonous, UsePoisonous },
            { CharacterAbility.Revive, UseRevive },
            { CharacterAbility.ShadowStrike, UseShadowStrike },
            { CharacterAbility.StoneEdge, UseStoneEdge },
            { CharacterAbility.StrengthElixir, UseStrenghtElixir },
            { CharacterAbility.Taunt, UseTaunt },
        };

        if (diceRoller == null)
        {
            diceRoller = FindObjectOfType<DiceRoller>();
            if (diceRoller == null)
                Debug.LogWarning("DiceRoller não encontrado na cena!");
        }
    }

    private void Start()
    {
        minionStat = FindObjectOfType<MinionStats>();
    }

    // VAR PUBLICAS

    public void OnButtonConfirmAbility()
    {
        ConfirmAbility();
    }

    public void UseCurrentAbility()
    {
        ConsumeAbility();
    }

    public void OnButtonCancelAbility()
    {
        CancelAbility();
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

            var abilityData = TakeDamage.Instance.CurrentAbilityData;

            AbilityInstance leroyInstance = GetAbility(CharacterAbility.Leroy);


            if (abilityData != null && leroyInstance != null && leroyInstance.IsActivated)
            {
                Debug.Log("Leroy funcionou!");
                minionStat.TakeDamageApply(playerCard.TotalDamage, (minionStat.BaseShield + 1), actionManager, cardDisplayManager);

                CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} usou Leroy: dano direto de {playerCard.TotalDamage} em {minionStat.CardData.CardName}.");

                actionManager.CheckEndOfTurn(cardDisplayManager);
                ConsumeAbility();
                leroyInstance.Desactivate();
            }
            else
            {
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
    }

    public void UseAbility(AbilityData abilityData)
    {        

        if (abilityData == null)
        {
            Debug.LogWarning("AbilityData é nulo!");
            return;
        }

        CharacterAbility ability = abilityData.AbilityID;

        AbilityInstance instance = playerCard.Abilities.FirstOrDefault(a => a.Data.AbilityID == ability);

        if (instance == null)
        {
            Debug.LogWarning($"AbilityInstance não encontrada para {ability}");
            return;
        }

        // Verifica se a habilidade está ativa.
        if (instance.IsActivated || instance.WasUsed)
        {
            Debug.Log($"{abilityData.AbilityName} já está ativada ou usada. Nada será feito.");
            return; 
        } else if (instance.Data.Type == AbilityType.Passive)
        {
            Debug.LogWarning("Habilidade é uma passiva. Impossivel ativá-la!");
            return;
        }

            instance.IsActivate();

        if (abilityActions.TryGetValue(ability, out Func<AbilityInstance, IEnumerator> action))
        {
            currentAbilityAction = action;
            currentAbilityInstance = instance;

            boxMessageAbility.SetActive(true);
            CombatLog.Instance.MessageBoxAbility(abilityData.AbilityName);
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
    private void ConfirmAbility()
    {

        if (currentAbilityInstance.Data.Type == AbilityType.Active)
        {
            bool hasAction = actionManager.TryUseAction();

            if (!hasAction) return;

            if (currentAbilityAction != null && currentAbilityInstance != null)
            {
                CombatLog.Instance.AddMessage($"Habilidade Ativada: {currentAbilityInstance.Data.AbilityName}");

                StartCoroutine(ExecuteAbilityAndCheckTurn(currentAbilityInstance));

                boxMessageAbility.SetActive(false);

                
            }
        } else
        {
            Debug.LogWarning("Habilidade é uma passiva. Impossivel ativá-la!");
        }

        
    }

    private IEnumerator ExecuteAbilityAndCheckTurn(AbilityInstance currentAbilityInstance)
    {
        if (currentAbilityAction != null)
        {
            
            if (currentAbilityAction.Method.ReturnType == typeof(IEnumerator))
            {
                yield return StartCoroutine((IEnumerator)currentAbilityAction.DynamicInvoke(currentAbilityInstance));
            }
            else
            {
                currentAbilityAction.Invoke(currentAbilityInstance);
            }
        }
        actionManager.CheckEndOfTurn(cardDisplayManager);
    }

    private void ConsumeAbility()
    {
        currentAbilityInstance = null;
        currentAbilityAction = null;
    }

    private void CancelAbility()
    {
        currentAbilityAction = null;
        currentAbilityInstance = null;
        boxMessageAbility.SetActive(false);

    }

    public AbilityInstance GetAbility(CharacterAbility abilityId)
    {
        return playerCard.Abilities.FirstOrDefault(a => a.Data.AbilityID == abilityId);
    }

    // AVAREZA
    private IEnumerator UseHurricaneArrow(AbilityInstance ability)
    {
        AbilityInstance hurricaneArrow = GetAbility(CharacterAbility.HurricaneArrow);

        yield return StartCoroutine(diceRoller.RollDiceShield(3, "Player", actionManager, cardDisplayManager, ability));

        ConsumeAbility();
        hurricaneArrow.Desactivate();
        Debug.Log($"Usou {ability.Data.AbilityName}! Acerta vários inimigos.");
    }
    private IEnumerator UseLightArrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}! Ataca e ilumina o alvo.");
        yield break;
    }
    private IEnumerator UseCriticalArrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}! Aplica dano crítico.");
        yield break;
    }

    // IRA
    private IEnumerator UseBerseker(AbilityInstance ability)
    {        
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseLeroy(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }

    // GULA
    private IEnumerator UseDevour(AbilityInstance ability)
    {

        // nao gastar acao caso esteja de vida cheia?

        AbilityInstance devourInstance = GetAbility(CharacterAbility.Devour);
        AbilityInstance massGrowthInstance = GetAbility(CharacterAbility.MassGrowth);

        if (!ability.TryThisTurn())
        {
            Debug.Log("Provocar já foi usada neste turno.");
            yield break;
        }

        int damageBonus = ability.Data.BaseValue;
        int healBonus = 5;

        int actualHealth = playerCard.CurrentHealth; // comparando antes/depois se curou

        playerCard.AbilityDevour(healBonus, damageBonus, minionStat);

        CombatLog.Instance.AddMessage($"{playerCard.CharData.CodeName} usou {ability.Data.AbilityName}, causando {ability.Data.BaseValue} e recuperando {(playerCard.CurrentHealth - actualHealth)} de vida!");

        if (massGrowthInstance != null)
        {
            yield return StartCoroutine(UseMassGrowth(massGrowthInstance));
        }

        ConsumeAbility();
        devourInstance.Desactivate();
        yield break;
    }
    private IEnumerator UseMassGrowth(AbilityInstance ability)
    {

        AbilityInstance massGrowthInstance = GetAbility(CharacterAbility.MassGrowth);

        playerCard.AdicionalLife(massGrowthInstance.Data.BaseValue);

        CombatLog.Instance.AddMessage($"Passiva: {massGrowthInstance.Data.AbilityName} ativada! +{massGrowthInstance.Data.BaseValue} de vida temporária aplicada.");
    
        yield break;
    }
    private IEnumerator UseTaunt(AbilityInstance ability)
    {       

        AbilityInstance tauntInstance = GetAbility(CharacterAbility.Taunt);

        if (!ability.TryThisTurn())
        {
            Debug.Log("Provocar já foi usada neste turno.");
            yield break;
        }

        playerCard.AdicionalShield(ability.Data.BaseValue);

        CombatLog.Instance.AddMessage($"{playerCard.CharData.CodeName} ganhou {ability.Data.BaseValue} de escudo ao utilizar {ability.Data.AbilityName}");

        ConsumeAbility();
        tauntInstance.Desactivate();
        yield break ;
    }

    // ORGULHO
    private IEnumerator  UseGlorySun(AbilityInstance ability)
    {
        AbilityInstance glorySunInstance = GetAbility(CharacterAbility.GlorySun);

        if (!ability.TryThisTurn())
        {
            Debug.Log($"{ability.Data.AbilityName} já foi usada neste turno.");
            yield break;
        }

        playerCard.AdicionalDamage(glorySunInstance.Data.BaseValue);
        minionStat.AdicionalDebuffDamage(3);

        ConsumeAbility();
        glorySunInstance.Desactivate();
        yield break;
    }
    private IEnumerator  UsePerfectArmor(AbilityInstance ability)
    {
        if (!ability.TryThisTurn())
        {
            Debug.Log($"{ability.Data.AbilityName} já foi usada neste turno.");
            yield break;
        }

        int addShield = playerCard.MaxShield - playerCard.TotalShield;

        playerCard.AdicionalShield(addShield);
        yield break;
    }

    // ---------------------------
    private IEnumerator UseArmorElixir(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator UseBurnBabyBurn(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator UseDaggerThrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator UseElementalist(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break ;
    }
    private IEnumerator  UseFuuton(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseHeal(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseInvisibility(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseLuckTide(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseNap(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UsePoisonous(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseRevive(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseShadowStrike(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseStoneEdge(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator UseStrenghtElixir(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    #endregion
}
