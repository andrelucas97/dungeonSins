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

    [Header("Abilities")]
    [SerializeField] private AbilityDatabase abilityDatabase;
    private List<AbilityInstance> abilityInstances = new List<AbilityInstance>();

    [SerializeField] private List<Transform> equipmentSlots;

    // VAR PUBLICAS
    public int CurrentHealth => currentHealth;
    public int Shield => shield;
    public int Damage => damage;
    public CharacterData CharData => charData;
    public List<AbilityInstance> Abilities => abilityInstances;
    private void Start()
    {
        actionManager = FindObjectOfType<ActionManager>();
        
    }

    public void Initialize(CharacterData charData)
    {
        this.charData = charData;

        healthSlider.maxValue = this.charData.MaxHealth;
        healthSlider.value = currentHealth;

        // Instanciando variaveis
        currentHealth = charData.MaxHealth;
        shield = charData.Shield;
        damage = charData.Damage;

        // Instanciando Habilidades
        abilityInstances.Clear();

        var abilityDatas = abilityDatabase.GetAbilityDataList(this.charData.Abilities);

        foreach (var data in abilityDatas)
        {
            abilityInstances.Add(new AbilityInstance(data));
        }

        StatusDisplay.Instance.AttStatusPlayer(this, this.charData);

    }


    // Chamada TESTE

    public void UpdateStatsSlot(string slotType)
    {

        if (slotType != "slotBackpack")
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
