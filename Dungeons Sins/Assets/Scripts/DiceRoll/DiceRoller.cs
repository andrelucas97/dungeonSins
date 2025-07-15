using System.Collections;
using System.Collections.Generic;
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
    private MinionStats minionStat;
    

    private bool isSucessful = true;
    //private int resultDie;

    public TextMeshProUGUI ShieldText => resultDiceShield;
    public TextMeshProUGUI DamageText => resultDiceDamage;


    // VAR PUBLICAS
    public GameObject TextShield => textShield;

    // BUTTON
    #region Button 

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
        yield return RollAndAttack(resultDiceShield, attacking, action, cardDisplay);
    }
    #endregion
    public void ShowDicePanel(bool isInteractable)
    {
        diceShield.GetComponent<Button>().interactable = isInteractable;
        gameObject.SetActive(true);
    }

    // Dice Rolls
    private IEnumerator RollAndAttack(TextMeshProUGUI resultDice, string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {
        if (minionStat == null)
        {
            minionStat = FindObjectOfType<MinionStats>();
        }

        if (resultDice == resultDiceShield)
        {
            yield return RollDice(1, 20, resultDiceShield, diceShield, attacking, action, cardDisplay);

        }      
    }
    private IEnumerator RollDice(int min, int max, TextMeshProUGUI diceResultTxt, GameObject typeDie,string attacking, ActionManager action, CardDisplayManager cardDisplay, int rollTimes = 10, float delay = 0.1f)
    {
        int result = min;

        DisabledButton(typeDie.GetComponent<Button>());
        
        for (int i = 0; i < rollTimes; i++)
        {
            result = Random.Range(min, max + 1);
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

        BaseStats atkStats = null;
        BaseStats defStats = null;

        string nameAtk = null;
        string nameDef = null;

        if (attacking == "Player")
        {
            defStats = minionStat;
            atkStats = playerCard;

            nameAtk = playerCard.CharData.CharName;
            nameDef = minionStat.CardData.CardName;

        }
        else if (attacking == "Minion")
        {
            defStats = playerCard;
            atkStats = minionStat;

            nameAtk = minionStat.CardData.CardName;
            nameDef = playerCard.CharData.CharName;

        }

        if (resultDieTxt == resultDiceShield)
        {
            if (resultado > defStats.Shield)
            {
                if (resultado == 20)
                {
                    textMessageBox.text = BattleMessages.Instance.CriticalAttack();
                    messageBox.SetActive(true);
                }

                isSucessful = true;
                diceShield.GetComponent<Button>().interactable = false;
                
            }
            else
            {
                isSucessful = false;
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

            yield return ResolveDiceRoll(isSucessful, resultado, atkStats, defStats, action, cardDisplay, attacking, nameAtk, nameDef);
        }
        else if (resultDieTxt == resultDiceDamage)
        {           

            yield return ResolveDiceRoll(isSucessful, resultado, atkStats, defStats, action, cardDisplay, attacking, nameAtk, nameDef);
        }
    }

    // Panel

    private IEnumerator ResolveDiceRoll(bool sucess, int result, BaseStats atkStats,BaseStats defStats, ActionManager action, CardDisplayManager cardDisplay, string attacking, string nameAtk, string nameDef)
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        messageBox.SetActive(false);
        textShield.SetActive(false);

        buttonPlay.GetComponent<Button>().interactable = true;

        if (sucess)
        {
            if (result == 20)
            {
                defStats.TakeDamage(atkStats.Damage * 2, result, action, cardDisplay);
                CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Ataque crítico! O /{nameAtk} causou ({atkStats.Damage})x2! Total: {atkStats.Damage *2} de dano em {nameDef}");
            }
            else
            {
                defStats.TakeDamage(atkStats.Damage, result, action, cardDisplay);
                CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] O {nameAtk} atacou com força ({result}), causando {atkStats.Damage} de dano em {nameDef}!");
            }

        }
        else if (result == 1) // Falha Critica
        {
            atkStats.TakeDamage(atkStats.Damage, result, action, cardDisplay);
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Falha crítica! {nameAtk} tomará {atkStats.Damage} de dano!");
        }
        else
        {
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}]O ataque falhou! O dado ({result}) do {nameAtk} não ultrapassou a defesa ({defStats.Shield}) do {nameDef}");
        }

        if (attacking == "Player")
            actionManager.CheckEndOfTurn(cardDisplayManager);
        else actionManager.CallCheckEndOfTurn(cardDisplayManager);

    }
}
