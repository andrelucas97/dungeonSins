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
    [SerializeField] private GameObject diceDamage;
    [SerializeField] private GameObject textShield;
    [SerializeField] private GameObject textDamage;

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
    public GameObject TextDamage => textDamage;


    // BUTTON
    #region Button 

    public void OnClick_Shield()
    {
        ButtonDiceShield("playerAttacking", actionManager, cardDisplayManager);
    }

    public void OnClick_Damage()
    {
        ButtonDiceDamage("playerAttacking", actionManager, cardDisplayManager);
    }

    public void ButtonDiceDamage(string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {
        RollAndAttack(resultDiceDamage, attacking, action, cardDisplay);
    }
    public void ButtonDiceShield(string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {
        RollAndAttack(resultDiceShield, attacking, action, cardDisplay);
    }
    #endregion
    public void ShowDicePanel(bool isInteractable)
    {
        diceShield.GetComponent<Button>().interactable = isInteractable;
        diceDamage.GetComponent<Button>().interactable = isInteractable;
        gameObject.SetActive(true);
    }

    // Dice Rolls
    private void RollAndAttack(TextMeshProUGUI resultDice, string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {
        if (minionStat == null)
        {
            minionStat = FindObjectOfType<MinionStats>();
        }

        if (resultDice == resultDiceShield)
        {
            StartCoroutine(RollDice(1, 20, resultDiceShield, diceShield, attacking, action, cardDisplay));

        } else if (resultDice == resultDiceDamage)
        {
            Debug.Log("Jogando dado Damage");
            StartCoroutine(RollDice(1, 6, resultDiceDamage, diceDamage, attacking, action, cardDisplay));
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
                diceResultTxt.text = "Dado: " + result;

            yield return new WaitForSeconds(delay);
        }
        ResultDice(result, diceResultTxt, attacking, action, cardDisplay);
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


    private void ResultDice(int resultado, TextMeshProUGUI resultDieTxt, string attacking, ActionManager action, CardDisplayManager cardDisplay)
    {

        BaseStats atkStats = null;
        BaseStats defStats = null;

        if (attacking == "playerAttacking")
        {
            defStats = minionStat;
            atkStats = playerCard;

        } else if (attacking == "minionAttacking")
        {
            defStats = playerCard;
            atkStats = minionStat;
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
                textShield.SetActive(false);                        
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

            StartCoroutine(ResolveDiceRoll(isSucessful, resultado, atkStats, defStats, action, cardDisplay));
        }
        else if (resultDieTxt == resultDiceDamage)
        {           

            diceDamage.GetComponent<Button>().interactable = false;
            StartCoroutine(ResolveDiceRoll(isSucessful, resultado, atkStats, defStats, action, cardDisplay));
        }
    }

    // Panel

    private IEnumerator ResolveDiceRoll(bool sucess, int result, BaseStats atkStats,BaseStats defStats, ActionManager action, CardDisplayManager cardDisplay)
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        messageBox.SetActive(false);

        buttonPlay.GetComponent<Button>().interactable = true;

        if (sucess)
        {
            if (result == 20)
            {
                defStats.TakeDamage(atkStats.Damage*2, result, action, cardDisplay);

            } else
            {
                defStats.TakeDamage(atkStats.Damage, result, action, cardDisplay);
            }

        } else if (result == 1) // Falha Critica
        {
            atkStats.TakeDamage(atkStats.Damage, result, action, cardDisplay);
        }
            actionManager.CheckEndOfTurn(cardDisplayManager);
        diceDamage.SetActive(false);
    }
}
