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


        // Habilidades a serem usadas mais de uma vez por turno
        AbilityInstance armorElixir = GetAbility(CharacterAbility.ArmorElixir);
        AbilityInstance strengthElixir = GetAbility(CharacterAbility.StrengthElixir);

        // Verifica se a habilidade está ativa.
        if (instance.IsActivated || instance.WasUsed)
        {
            if (instance != armorElixir && instance != strengthElixir)
            {
                Debug.Log($"{abilityData.AbilityName} já está ativada ou usada. Nada será feito.");
                return;
            }
            
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
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance hurricaneArrow = GetAbility(CharacterAbility.HurricaneArrow);
        yield return StartCoroutine(diceRoller.RollDiceShield(3, "Player", actionManager, cardDisplayManager, ability));

        ConsumeAbility();
        hurricaneArrow.Desactivate();
        Debug.Log($"Usou {ability.Data.AbilityName}! Acerta vários inimigos.");
    }
    private IEnumerator UseLightArrow(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}! Ataca e ilumina o alvo.");
        yield break;
    }
    private IEnumerator UseCriticalArrow(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}! Aplica dano crítico.");
        yield break;
    }

    // IRA
    private IEnumerator UseBerseker(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseLeroy(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }

    // GULA
    private IEnumerator UseDevour(AbilityInstance ability)
    {

        // nao gastar acao caso esteja de vida cheia?
        if (!ability.TryThisTurn())
        {
            Debug.Log("Provocar já foi usada neste turno.");
            yield break;
        }

        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance devourInstance = GetAbility(CharacterAbility.Devour);
        AbilityInstance massGrowthInstance = GetAbility(CharacterAbility.MassGrowth);
        int damageBonus = ability.Data.BaseValue;
        int healBonus = 5;
        int actualHealth = playerCard.CurrentHealth; // comparando antes/depois se curou

        //playerCard.AbilityDevour(healBonus, damageBonus, minionStat);

        playerCard.AdicionalBuffPlayer(healBonus, devourInstance);
        minionStat.ApplyDirectDamage(damageBonus);

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} usou {ability.Data.AbilityName}, causando {ability.Data.BaseValue} e recuperando {(playerCard.CurrentHealth - actualHealth)} de vida!");

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
        //bool hasAction = actionManager.TryUseAction();
        //if (!hasAction) yield break;

        AbilityInstance massGrowthInstance = GetAbility(CharacterAbility.MassGrowth);
        playerCard.AdicionalBuffPlayer(massGrowthInstance.Data.BaseValue, massGrowthInstance);
        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Passiva: {massGrowthInstance.Data.AbilityName} ativada! +{massGrowthInstance.Data.BaseValue} de vida temporária aplicada.");
    
        yield break;
    }
    private IEnumerator UseTaunt(AbilityInstance ability)
    {
        if (!ability.TryThisTurn())
        {
            Debug.Log("Provocar já foi usada neste turno.");
            yield break;
        }

        if (playerCard.TotalShield >= playerCard.MaxShield)
        {
            CombatLog.Instance.AddMessage("Escudo máximo! Habilidade não ativada!");
            ability.Desactivate();
            yield break;
        }

        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance tauntInstance = GetAbility(CharacterAbility.Taunt);
        playerCard.AdicionalBuffPlayer(ability.Data.BaseValue, ability);

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} ganhou {ability.Data.BaseValue} de escudo ao utilizar {ability.Data.AbilityName}");

        ConsumeAbility();
        tauntInstance.Desactivate();
        yield break ;
    }

    // ORGULHO
    private IEnumerator  UseGlorySun(AbilityInstance ability)
    {
        if (!ability.TryThisTurn())
        {
            Debug.Log($"{ability.Data.AbilityName} já foi usada neste turno.");
            yield break;
        }
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance glorySunInstance = GetAbility(CharacterAbility.GlorySun);
        playerCard.AdicionalBuffPlayer(glorySunInstance.Data.BaseValue, glorySunInstance);
        minionStat.AdicionalDebuffDamage(3);

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} ganhou {ability.Data.BaseValue} de escudo ao utilizar {ability.Data.AbilityName} e reduziu {3} de força do {minionStat.CardData.CardName}");

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

        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;
        
        int addShield = playerCard.MaxShield - playerCard.TotalShield;

        playerCard.AdicionalBuffPlayer(addShield, ability);

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} aumentou teu escudo no máximo (+{addShield} - Total: {playerCard.TotalShield}).");

        ConsumeAbility();
        ability.Desactivate();
        yield break;
    }

    // PREGUICA
    private IEnumerator UseArmorElixir(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance elixirArmorInstance = GetAbility(CharacterAbility.ArmorElixir);

        playerCard.AdicionalBuffPlayer(elixirArmorInstance.Data.BaseValue, elixirArmorInstance);

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} ativou {ability.Data.AbilityName}, recebendo +{elixirArmorInstance.Data.BaseValue} de escudo!");

        ConsumeAbility();
        // ability.Desactivate(); // Sendo chamada no ActionManager, para limpar e desativa-lo

        yield break;
    }
    private IEnumerator UseStrenghtElixir(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance elixirStrengthInstance = GetAbility(CharacterAbility.StrengthElixir);

        playerCard.AdicionalBuffPlayer(elixirStrengthInstance.Data.BaseValue, elixirStrengthInstance);

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} ativou {ability.Data.AbilityName}, recebendo +{elixirStrengthInstance.Data.BaseValue} de força!");

        ConsumeAbility();
        // ability.Desactivate(); // Sendo chamada no ActionManager, para limpar e desativa-lo
        yield break;
    }
    private IEnumerator  UseHeal(AbilityInstance ability)
    {       

        AbilityInstance healInstance = GetAbility(CharacterAbility.Heal);

        if (playerCard.CurrentHealth >= playerCard.CharData.MaxHealth)
        {
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Vida máxima! Cura não utilizada.");
            ability.Desactivate();
            yield break;
        }

        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        playerCard.AdicionalBuffPlayer(healInstance.Data.BaseValue, healInstance);

        ConsumeAbility();
        ability.Desactivate();
        yield break;
    }
    private IEnumerator  UsePoisonous(AbilityInstance ability)
    {       

        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseNap(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;



        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {ability.Data.AbilityName} entra em efeito: o próximo turno dormirá, curando 10 de vida.");

        yield break;
    }
    private IEnumerator  UseRevive(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }

    // Luxuria
    private IEnumerator  UseInvisibility(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        AbilityInstance shadowStrikeInstance = GetAbility(CharacterAbility.ShadowStrike);

        shadowStrikeInstance.IsActivate();

        CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {ability.Data.AbilityName} ativado! Torna-se invisivel neste turno! (Passiva {shadowStrikeInstance.Data.AbilityName}: dano dobrado!!)");

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseShadowStrike(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator UseDaggerThrow(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }

    // ---------------------------
    private IEnumerator UseBurnBabyBurn(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator UseElementalist(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break ;
    }
    private IEnumerator  UseFuuton(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseLuckTide(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    private IEnumerator  UseStoneEdge(AbilityInstance ability)
    {
        bool hasAction = actionManager.TryUseAction();
        if (!hasAction) yield break;

        Debug.Log($"Usou {ability.Data.AbilityName}!");
        yield break;
    }
    #endregion
}
