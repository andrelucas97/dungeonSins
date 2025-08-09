using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class DiceRoller : MonoBehaviour
{
    // VAR PRIVADAS
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private CardDisplayManager cardDisplayManager;

    [Header("Dice")]
    [SerializeField] private TextMeshProUGUI resultDiceShield;
    [SerializeField] private TextMeshProUGUI resultDiceDamage;
    [SerializeField] private GameObject diceShield;
    [SerializeField] private GameObject textShield;

    [Header("Message Box")]
    [SerializeField] private GameObject messageBox;
    [SerializeField] private TextMeshProUGUI textMessageBox;

    [Header("Card Character")]
    [SerializeField] private CharStats playerCard;

    [Header("Button Play")]
    [SerializeField] private GameObject buttonPlay;
    [SerializeField] private MinionStats minionStat;

    [Header("Sound Dice roll")]
    [SerializeField] private AudioClip diceRollClip;

    private bool isSuccessful = true;
    //private int resultDie;

    public TextMeshProUGUI ShieldText => resultDiceShield;
    public TextMeshProUGUI DamageText => resultDiceDamage;


    // VAR PUBLICAS
    public GameObject TextShield => textShield;

    // FUNÇÕES PUBLICAS
    // BUTTON
    #region Button 

    private void Awake()
    {
        minionStat = FindObjectOfType<MinionStats>();
    }

    public void OnClick_Shield()
    {
        ButtonDiceShield("Player", actionManager, cardDisplayManager);
    }

    public void ButtonDiceDamage(string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {
        RollAndAttack(resultDiceDamage, attacking, action, cardDisplay);
    }
    public void ButtonDiceShield(string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {

        StartCoroutine(RollAndAttack(resultDiceShield, attacking, action, cardDisplay));
    }

    public IEnumerator DiceShieldMinion(string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {

        if (diceRollClip != null)
            AudioManager.Instance.PlaySound(diceRollClip);

        yield return RollAndAttack(resultDiceShield, attacking, action, cardDisplay);
    }
    #endregion
    public void ShowDicePanel(bool isInteractable)
    {
        diceShield.GetComponent<Button>().interactable = isInteractable;
        gameObject.SetActive(true);
    }

    public IEnumerator RollDiceShield(int arrowCount, string attacking, ActionManager action, CardDisplayManager cardDisplay, AbilityInstance ability)
    {
        yield return RollMultipleDice(arrowCount, attacking, action, cardDisplay, ability);
    }

    // FUNÇÕES PRIVADAS
    private IEnumerator RollAndAttack(TextMeshProUGUI resultDice, string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {

        AbilityInstance lightArrowInstance = TakeDamage.Instance.GetAbility(CharacterAbility.LightArrow);

        if (minionStat == null)
        {
            minionStat = FindObjectOfType<MinionStats>();
        }

        if (resultDice == resultDiceShield)
        {
            if (attacking == "Minion" && lightArrowInstance != null && lightArrowInstance.IsActivated)
            {
                CombatLog.Instance.AddMessage($"Flecha de Luz ativada! Dado do {minionStat.CardData.CardName} reduzido para {(20-lightArrowInstance.Data.BaseValue)}");
                yield return RollDice(1, 17, resultDiceShield, diceShield, attacking, action, cardDisplay);

                TakeDamage.Instance.UseCurrentAbility();
                lightArrowInstance.Desactivate();
            }
            else
                yield return RollDice(1, 20, resultDiceShield, diceShield, attacking, action, cardDisplay);

        }
    }

    private IEnumerator RollMultipleDice(int arrowCount, string attacking, ActionManager action, CardDisplayManager cardDisplay, AbilityInstance ability)
    {
        int hits = 0;       

        for (int arrowIndex = 0; arrowIndex < arrowCount; arrowIndex++)
        {
            ShowDicePanel(true);
            yield return RollDice(1, 20, resultDiceShield, diceShield, attacking, action, cardDisplay);

            if (isSuccessful)
            {
                hits++;
                CombatLog.Instance.AddMessage($"[{ability.Data.AbilityName}] Ataque {arrowIndex+1} acertou o inimigo!!");
            }
            else
            {
                CombatLog.Instance.AddMessage($"[{ability.Data.AbilityName}] Ataque {arrowIndex+1} errou o inimigo!!");

            }

        }

        CombatLog.Instance.AddMessage($"[{ability.Data.AbilityName}] {hits}/{arrowCount} acertaram o inimigo!");

    }

    private IEnumerator RollDice(int min, int max, TextMeshProUGUI diceResultTxt, GameObject typeDie,string attacking, ActionManager action, CardDisplayManager cardDisplay, int rollTimes = 10, float delay = 0.1f)
    {
        int result = min;

        DisabledButton(typeDie.GetComponent<Button>());
        
        for (int i = 0; i < rollTimes; i++)
        {
            result = UnityEngine.Random.Range(min, max + 1);
            if (diceResultTxt != null)
                diceResultTxt.text = $"{result}";

            yield return new WaitForSeconds(delay);
        }
        yield return ResultDice(result, diceResultTxt, attacking, action, cardDisplay);
    }

    private void DisabledButton(Button button)
    {
        ColorBlock colors = button.colors;
        Color disabledColor = colors.disabledColor;
        disabledColor.a = 1f;
        colors.disabledColor = disabledColor;
        button.colors = colors;

        button.interactable = false;
    }


    private IEnumerator ResultDice(int resultado, TextMeshProUGUI resultDieTxt, string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {

        int damageStat = 0;
        int shieldStat = 0;
        BaseStats defenderStat = null;
        BaseStats attackingStat = null;


        string nameAtk = null;
        string nameDef = null;

        if (attacking == "Player")
        {

            shieldStat = minionStat.BaseShield;
            nameDef = minionStat.CardData.CardName;
            defenderStat = minionStat;

            nameAtk = playerCard.CharData.CharName;
            damageStat = playerCard.TotalDamage;
            attackingStat = playerCard;

        }
        else if (attacking == "Minion")
        {
            shieldStat = playerCard.TotalShield;
            nameDef = playerCard.CharData.CharName;
            defenderStat = playerCard;

            nameAtk = minionStat.CardData.CardName;
            damageStat = minionStat.TotalDamage;
            attackingStat = minionStat;
        }

        if (resultDieTxt == resultDiceShield)
        {
            if (resultado > shieldStat)
            {
                if (resultado == 20)
                {
                    textMessageBox.text = BattleMessages.Instance.CriticalAttack();
                    messageBox.SetActive(true);
                }

                isSuccessful = true;
                diceShield.GetComponent<Button>().interactable = false;
                
            }
            else
            {
                isSuccessful = false;
                if (resultado == 1)
                {
                    textMessageBox.text = BattleMessages.Instance.CriticalFail();
                    messageBox.SetActive(true);
                }
                else
                {
                    textMessageBox.text = BattleMessages.Instance.GetRandomFailMessage();
                    messageBox.SetActive(true);
                }
                diceShield.GetComponent<Button>().interactable = false;                
            }

            yield return ResolveDiceRoll(isSuccessful, resultado, damageStat, shieldStat, action, cardDisplay, attacking, nameAtk, nameDef, defenderStat, attackingStat);
        }
        else if (resultDieTxt == resultDiceDamage)
        {           

            yield return ResolveDiceRoll(isSuccessful, resultado, damageStat, shieldStat, action, cardDisplay, attacking, nameAtk, nameDef, defenderStat, attackingStat);
        }
    }

    // Panel

    private IEnumerator ResolveDiceRoll(bool sucess, int result, int atkStats,int defStats, ActionManager action, CardDisplayManager cardDisplay, string attacking, string nameAtk, string nameDef, BaseStats defenderStat, BaseStats attackingStat)
    {
        yield return new WaitForSeconds(2f);

        AbilityInstance hurricaneArrowInstance = playerCard.Abilities.FirstOrDefault(a => a.Data.AbilityID == CharacterAbility.HurricaneArrow);

        gameObject.SetActive(false);
        messageBox.SetActive(false);
        textShield.SetActive(false);

        buttonPlay.GetComponent<Button>().interactable = true;

        int damageMultiplier = 1;
        int finalDamage = 0;
        if (sucess)
        {

            var abilityData = TakeDamage.Instance.CurrentAbilityData;

            AbilityInstance bersekerInstance = TakeDamage.Instance.GetAbility(CharacterAbility.Berseker);
            AbilityInstance criticalArrowInstance = TakeDamage.Instance.GetAbility(CharacterAbility.CriticalArrow);
            AbilityInstance invisibilityInstance = TakeDamage.Instance.GetAbility(CharacterAbility.Invisibility);
            AbilityInstance shadowStrikeInstance = TakeDamage.Instance.GetAbility(CharacterAbility.ShadowStrike);

            // Berseker Ativado
            if (abilityData != null && bersekerInstance != null && bersekerInstance.IsActivated && attacking == "Player")
            {
                damageMultiplier = abilityData.BaseValue;

                if (result == 20)
                {
                    damageMultiplier *= 2;
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] CRÍTICO MÁXIMO DO BERSERKER!! O {nameAtk} causou {atkStats} (x{damageMultiplier})! Total: {atkStats * 4} de dano em {nameDef}");
                }
                else
                {
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Crítico do Berserker! O {nameAtk} atacou com força (dado: {result}), causando {atkStats} (x{damageMultiplier}) de dano em {nameDef}!");
                }

                TakeDamage.Instance.UseCurrentAbility();
                bersekerInstance.Desactivate();
            }
            // Flecha Critica Ativada
            else if (abilityData != null && criticalArrowInstance != null && criticalArrowInstance.IsActivated && attacking == "Player")
            {
                if (result >= abilityData.BaseValue)
                {
                    damageMultiplier = 2;
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Flecha Crítica ativada! O {nameAtk} acertou um disparo devastador (dado: {result}), causando {atkStats} (x2)! Total: {atkStats * 2} de dano em {nameDef}!");
                    Debug.Log("Flecha critica ativada!!");
                }
                else
                {
                    damageMultiplier = 1;
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Flecha Crítica ativada! O {nameAtk} disparou (dado: {result}), mas não foi crítico. Causou {atkStats} de dano normal em {nameDef}.");
                    Debug.Log("Flecha critica não está ativado!");

                }

                TakeDamage.Instance.UseCurrentAbility();
                criticalArrowInstance.Desactivate();
            }
            // Insivibilidade Ativada
            else if (abilityData != null && invisibilityInstance != null && invisibilityInstance.IsActivated && attacking == "Player")
            {
                Debug.Log("Invisibilidade ativada!");

                damageMultiplier = shadowStrikeInstance.Data.BaseValue;

                if (result == 20)
                {
                    damageMultiplier *= 2;
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] CRÍTICO MÁXIMO da Luxúria invisível!! {nameAtk} causou {atkStats} x{damageMultiplier} = {atkStats * damageMultiplier} de dano em {nameDef}!");
                }
                else
                {                    
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Dano dobrado pela invisibilidade!! {nameAtk} (dado: {result}) causou {atkStats} x{damageMultiplier} = {atkStats * damageMultiplier} de dano em {nameDef}!");
                }

                TakeDamage.Instance.UseCurrentAbility();
                invisibilityInstance.Desactivate();
                shadowStrikeInstance.Desactivate();

                CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Fim da invisibilidade!");
            }

            else
            {
                if (result == 20)
                {
                    damageMultiplier = 2;
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Ataque crítico! O {nameAtk} rolou {result} no dado, causando {atkStats} (x2)! Total: {atkStats * 2} de dano em {nameDef}");
                }
                else
                {
                    damageMultiplier = 1;
                    CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] O {nameAtk} atacou com força (dado: {result}), causando {atkStats} de dano em {nameDef}!");
                }
            }
            

            finalDamage = atkStats * damageMultiplier;
            defenderStat.TakeDamageApply(finalDamage, result, action, cardDisplay);

        }
        else if (result == 1) // Falha Critica
        {
            attackingStat.TakeDamageApply(atkStats, result, action, cardDisplay);
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Falha crítica! {nameAtk} tomará {atkStats} de dano!");
        }
        else
        {
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}]O ataque falhou! O dado ({result}) do {nameAtk} não ultrapassou a defesa ({defStats}) do {nameDef}");
        }

        if (attacking == "Player")
        {
            if (hurricaneArrowInstance == null || !hurricaneArrowInstance.IsActivated)
            {
                //onFinished?.Invoke();
                actionManager.CheckEndOfTurn(cardDisplayManager);
            }
        }
        else if (attacking == "Minion")
        {
            actionManager.CallCheckEndOfTurn(cardDisplayManager);

        }
    }
}
