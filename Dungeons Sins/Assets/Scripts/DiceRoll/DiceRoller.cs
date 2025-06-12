using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    // VAR PRIVADAS

    [Header("Dice")]
    [SerializeField] private TextMeshProUGUI resultDiceShield;
    [SerializeField] private TextMeshProUGUI resultDiceDamage;
    [SerializeField] private GameObject diceShield;
    [SerializeField] private GameObject diceDamage;

    [Header("Card Character")]
    [SerializeField] private CharStats playerCard;

    [Header("Button Play")]
    [SerializeField] private GameObject buttonPlay;
    private MinionStats minionStat;

    private bool isSucessful = true;

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
    private int RollDice(int min, int max, TextMeshProUGUI textResult)
    {
        int result = Random.Range(min, max+1);

        if (textResult != null)
            textResult.text = "Dado: " + result;       

        return result;
    }
    private void RollAndAttack(TextMeshProUGUI resultDice)
    {
        if (resultDice == resultDiceShield)
        {
            int resultado = RollDice(1, 20, resultDiceShield);

            if (minionStat == null)
            {
                minionStat = FindObjectOfType<MinionStats>();
            }

            if (resultado >= minionStat.Shield)
            {
                isSucessful = true;
                diceShield.GetComponent<Button>().interactable = false;
                diceDamage.SetActive(true);
            }
            else 
            {
                isSucessful = false;

                if (resultado == 1)
                    Debug.Log("Falha crítica! Leve o dano!");

                StartCoroutine(ResolveDiceRoll(isSucessful, resultado));
            }

        } else if (resultDice == resultDiceDamage)
        {
            int resultado = RollDice(1, 6, resultDiceDamage);

            if (minionStat == null)
            {
                minionStat = FindObjectOfType<MinionStats>();
            }

            diceDamage.GetComponent<Button>().interactable = false;
            StartCoroutine(ResolveDiceRoll(isSucessful, resultado));
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

    }
}
