using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour, BaseStats
{
    // VAR PRIVADAS

    [Header("Stats")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int shield;
    [SerializeField] private int damage;
    [SerializeField] private Slider healthSlider;
    private Coroutine healthBarCoroutine;

    [Header("Class")]
    [SerializeField] private CharacterData charData;
    [SerializeField] private CharUI charUI;
    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private ActionManager actionManager;

    [SerializeField] private List<Transform> equipmentSlots;

    // VAR PUBLICAS
    public int CurrentHealth => currentHealth;
    public int Shield => shield;
    public int Damage => damage;
    public CharacterData CharData => charData;

    public void Initialize(CharacterData data)
    {
        charData = data;

        healthSlider.maxValue = charData.MaxHealth;
        healthSlider.value = currentHealth;

        // Instanciando variaveis
        currentHealth = data.MaxHealth;
        shield = data.Shield;
        damage = data.Damage;

        StatusDisplay.Instance.AttStatusPlayer(this, charData);

    }


    // Chamada TESTE
    private void Start()
    {
        actionManager = FindObjectOfType<ActionManager>();
        
    }

    public void UpdateStats(string teste)
    {

        if (teste != "slotBackpack")
        {
            shield = charData.Shield;
            damage = charData.Damage;

            foreach (Transform slot in equipmentSlots)
            {
                if (slot.childCount == 0) continue;

                GameObject card = slot.GetChild(0).gameObject;
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
        }

        StatusDisplay.Instance.AttStatusPlayer(this, charData);
        actionManager.CheckEndOfTurn(cardDisplayManager);
    }

    public void TakeDamage(int hitDamage, int resultDie, ActionManager action, CardDisplayManager cardDisplay)
    {
        ApplyDamage(hitDamage, resultDie);
    }

    private void ApplyDamage(int hitDamage, int resultDie)
    {
        currentHealth -= hitDamage;

        currentHealth = Mathf.Max(currentHealth, 0);
        StatusDisplay.Instance.AttStatusPlayer(this, charData);


        if (healthBarCoroutine != null)
            StopCoroutine(healthBarCoroutine);
        healthBarCoroutine = StartCoroutine(AnimateHealthBar(currentHealth));

        if (currentHealth == 0)
        {
            CombatLog.Instance.AddMessage($"[T{actionManager.CurrentTurn}] Game Over! Você foi derrotado!!");
        }
    }

    private IEnumerator AnimateHealthBar (int targetHealth)
    {
        float duration = 0.4f;
        float elapsed = 0f;

        float startValue = healthSlider.value;
        float endValue = targetHealth;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            healthSlider.value = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }

        healthSlider.value = endValue;

    }
}
