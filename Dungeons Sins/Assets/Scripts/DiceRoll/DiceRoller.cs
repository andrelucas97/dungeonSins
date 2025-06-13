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
    private int resultDie;

    // VAR PUBLICAS
    public GameObject TextShield => textShield;
    public GameObject TextDamage => textDamage;


    // BUTTON
    #region Button 
    public void ButtonDiceDamage()
    {
        RollAndAttack(resultDiceDamage);
    }
    public void ButtonDiceShield()
    {
        RollAndAttack(resultDiceShield);
    }
    #endregion

    // Dice Rolls
    private IEnumerator RollDice(int min, int max, TextMeshProUGUI diceResultTxt, GameObject typeDie,int rollTimes = 10, float delay = 0.1f)
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
        ResultDice(result, diceResultTxt);
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

    private void RollAndAttack(TextMeshProUGUI resultDice)
    {
        if (minionStat == null)
        {
            minionStat = FindObjectOfType<MinionStats>();
        }

        if (resultDice == resultDiceShield)
        {
            StartCoroutine(RollDice(1, 20, resultDiceShield, diceShield));                      

        } else if (resultDice == resultDiceDamage)
        {
            StartCoroutine(RollDice(1, 6, resultDiceDamage, diceDamage));
        }        
    }

    private void ResultDice(int resultado, TextMeshProUGUI resultDieTxt)
    {
        resultDie = resultado;

        if (resultDieTxt == resultDiceShield)
        {
            if (resultDie > minionStat.Shield)
            {
                isSucessful = true;
                diceShield.GetComponent<Button>().interactable = false;
                textShield.SetActive(false);

                diceDamage.SetActive(true);
                textDamage.SetActive(true);
            }
            else
            {
                isSucessful = false;

                if (resultado == 1)
                {
                    textMessageBox.text = BattleMessages.Instance.CriticalFail();
                    messageBox.SetActive(true);

                } else
                {
                    textMessageBox.text = BattleMessages.Instance.GetRandomFailMessage();
                    messageBox.SetActive(true);
                }
                diceShield.GetComponent <Button>().interactable = false;
                StartCoroutine(ResolveDiceRoll(isSucessful, resultado));
            }
        } else if (resultDieTxt == resultDiceDamage)
        {
            diceDamage.GetComponent<Button>().interactable = false;
            StartCoroutine(ResolveDiceRoll(isSucessful, resultDie));
        }
    }

    // Panel
    public void ShowDicePanel()
    {
        diceShield.GetComponent<Button>().interactable = true;
        diceDamage.GetComponent<Button>().interactable = true;
        gameObject.SetActive(true);
    }

    private IEnumerator ResolveDiceRoll(bool sucess, int result)
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        buttonPlay.GetComponent<Button>().interactable = true;

        if (sucess)
        {
            minionStat.TakeDamage(playerCard.Damage, result);
        }

        diceDamage.SetActive(false);
    }
}
