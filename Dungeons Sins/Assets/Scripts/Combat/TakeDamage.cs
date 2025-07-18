using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Action<AbilityInstance> currentAbilityAction;
    private AbilityInstance currentAbilityInstance;


    private MinionStats minionStat;
    private Dictionary<CharacterAbility, Action<AbilityInstance>> abilityActions;

    // VAR PUBLICAS
    public static TakeDamage Instance;
    public AbilityInstance CurrentAbilityInstance => currentAbilityInstance;
    public AbilityData CurrentAbilityData => currentAbilityInstance?.Data;

    void Awake()
    {
        Instance = this;
        actionManager = FindObjectOfType<ActionManager>();

        abilityActions = new Dictionary<CharacterAbility, Action<AbilityInstance>>
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

            if (abilityData != null && abilityData.AbilityID == CharacterAbility.Leroy)
            {
                Debug.Log("Leroy funcionou!");
                minionStat.TakeDamage(playerCard.Damage, (minionStat.Shield+1), actionManager, cardDisplayManager);
                actionManager.CheckEndOfTurn(cardDisplayManager);

                CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] {playerCard.CharData.CodeName} usou Leroy: dano direto de {playerCard.Damage} em {minionStat.CardData.CardName}.");

                ConsumeAbility();
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
        if (instance.IsActivated)
        {
            Debug.Log($"{abilityData.AbilityName} já está ativada. Nada será feito.");
            return; 
        }

        instance.IsActivate();

        if (abilityActions.TryGetValue(ability, out Action<AbilityInstance> action))
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
        bool hasAction = actionManager.TryUseAction();

        if (!hasAction) return;

        if (currentAbilityAction != null && currentAbilityInstance != null)
        {
            currentAbilityAction.Invoke(currentAbilityInstance);

            CombatLog.Instance.AddMessage($"Habilidade Ativada: {currentAbilityInstance.Data.AbilityName}");


            boxMessageAbility.SetActive(false);            

            actionManager.CheckEndOfTurn(cardDisplayManager);
        }
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
    private void UseHurricaneArrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}! Acerta vários inimigos.");
    }

    private void UseLightArrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}! Ataca e ilumina o alvo.");
    }

    private void UseCriticalArrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}! Aplica dano crítico.");
    }

    private void UseArmorElixir(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseBerseker(AbilityInstance ability)
    {        
        Debug.Log($"Usou {ability.Data.AbilityName}!");
    }
    private void UseBurnBabyBurn(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseDaggerThrow(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseDevour(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseElementalist(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseFuuton(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseGlorySun(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseHeal(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseInvisibility(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseLeroy(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseLuckTide(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseMassGrowth(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseNap(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UsePerfectArmor(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UsePoisonous(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseRevive(AbilityInstance ability)
    {

        Debug.Log($"Usou {ability.Data.AbilityName}!");
    }
    private void UseShadowStrike(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseStoneEdge(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseStrenghtElixir(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    private void UseTaunt(AbilityInstance ability)
    {
        Debug.Log($"Usou {ability.Data.AbilityName}!");

    }
    #endregion
}
