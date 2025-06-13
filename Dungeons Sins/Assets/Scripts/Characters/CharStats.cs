using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    // VAR PRIVADAS
    [SerializeField] private CharacterData testCharacterData;
    [SerializeField] private CharUI charUI;

    [SerializeField] private int currentHealth;
    [SerializeField] private int shield;
    [SerializeField] private int damage;

    private CharacterData baseData;
    [SerializeField] private CardDisplayManager cardDisplayManager;

    // VAR PUBLICAS
    public int CurrentHealth => currentHealth;
    public int Shield => shield;
    public int Damage => damage;

    public void Initialize(CharacterData data)
    {
        baseData = data;

        // Instanciando variaveis
        currentHealth = data.MaxHealth;
        shield = data.Shield;
        damage = data.Damage;
    }


    // Chamada TESTE
    private void Start()
    {
        if (testCharacterData != null)
        {
            charUI.Setup(testCharacterData);
            Debug.Log("Character initialized in test mode: " + testCharacterData.CharName);

            StatusDisplay.Instance.AttStatusPlayer(this, testCharacterData);
        }
    }

    public void UpdateStats()
    {
        Debug.Log("Atualizando Status Personagem");

        currentHealth = baseData.MaxHealth;
        shield = baseData.Shield;
        damage = baseData.Damage;

        foreach (GameObject card in cardDisplayManager.CardsCharSheet)
        {
            Debug.Log("Carta equipada: " + card.name);

            CardEquipUI cardEquipUI = card.GetComponent<CardEquipUI>();
            if (cardEquipUI != null) 
            {
                EquipmentCard equip = cardEquipUI.CardData;

                switch (equip.CardStat)
                {
                    case CardStats.ATK:
                        damage += equip.AttackBonus;
                        break;
                    case CardStats.DEF:
                        shield += equip.DefenseBonus;
                        break;
                }
            }            
        }
        StatusDisplay.Instance.AttStatusPlayer(this, testCharacterData);        
    }

}
