using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour, BaseStats
{
    // VAR PRIVADAS

    [Header("Stats")]
    // Health
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;
    private Coroutine healthBarCoroutine;

    // Shield
    [SerializeField] private int baseShield;
    private int equipShieldBonus;
    private int tempShieldBonus;
    private int maxShield = 20;
    public int TotalShield => baseShield + equipShieldBonus + tempShieldBonus;

    // Damage
    [SerializeField] private int baseDamage;
    private int equipDamageBonus;
    private int tempDamageBonus;
    public int TotalDamage => baseDamage + equipDamageBonus + tempDamageBonus;

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
    public int BaseShield => baseShield;
    public int MaxShield => maxShield;
    public int BaseDamage => baseDamage;
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
        baseShield = charData.Shield;
        baseDamage = charData.Damage;

        // Instanciando Habilidades
        abilityInstances.Clear();

        var abilityDatas = abilityDatabase.GetAbilityDataList(this.charData.Abilities);

        foreach (var data in abilityDatas)
        {
            abilityInstances.Add(new AbilityInstance(data));
        }

        StatusDisplay.Instance.AttStatusPlayer(this, this.charData);

    }

    public void UpdateStatsSlot(string slotType)
    {

        //if (slotType != SlotType.Backpack.ToString())
        {
            baseShield = charData.Shield;
            baseDamage = charData.Damage;

            equipShieldBonus = 0;
            equipDamageBonus = 0;

            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                Transform slot = equipmentSlots[i];

                if (slot.childCount == 0) continue;

                GameObject card = slot.GetChild(0).gameObject;
                CardEquipUI cardEquipUI = card.GetComponent<CardEquipUI>();

                if (i == 3) continue; // slot backpack

                if (cardEquipUI != null)
                {
                    EquipmentCard equip = cardEquipUI.CardData;
                    switch (equip.CardStat)
                    {
                        case CardStats.ATK:
                            equipDamageBonus += equip.AttackBonus;
                            break;
                        case CardStats.DEF:
                            equipShieldBonus += equip.DefenseBonus;
                            break;
                    }
                }

            }
        }

        StatusDisplay.Instance.AttStatusPlayer(this, charData);
        actionManager.CheckEndOfTurn(cardDisplayManager);
    }

    public void TakeDamageApply(int hitDamage, int resultDie, ActionManager action, CardDisplayManager cardDisplay)
    {
        ApplyDamage(hitDamage, resultDie);
    }

    public void AdicionalBuffPlayer(int value, AbilityInstance abilityInstance)
    {
        AddBuff(value, abilityInstance);
    }

    public void RemoveShieldEndTurn(int value)
    {
        RemoveShieldBonus(value);
    }
    public void RemoveDamageEndTurn(int value)
    {
        RemoveDamageBonus(value);
    }
    public void ClearTempBonus(string type)
    {
        ClearBonus(type);
    }

    // FUNCOES PRIVADAS
    private void ActivateAbility(int healBonus, int damageBonus, MinionStats minionStat)
    {

        AbilityInstance devourInstance = Abilities.FirstOrDefault(a => a.Data.AbilityID == CharacterAbility.Devour);

        AddBuff(healBonus, devourInstance);
        minionStat.ApplyDirectDamage(damageBonus);
    }
    private void AddBuff(int amount, AbilityInstance abilityInstance)
    {

        AbilityEffectType effectType = abilityInstance.Data.EffectType;

        switch (effectType)
        {
            case AbilityEffectType.Shield:

                if (TotalShield < maxShield)                
                    tempShieldBonus += amount;
                break;
            case AbilityEffectType.Damage:
                tempDamageBonus += amount;
                break;
            case AbilityEffectType.Heal:

                if (currentHealth >= charData.MaxHealth)
                {

                    if (abilityInstance.Data.AbilityID == CharacterAbility.MassGrowth)
                    {
                        currentHealth += abilityInstance.Data.BaseValue;
                        break;
                    }
                    
                    Debug.Log("Vida máxima! Não é possivel curar!");
                    return;
                }

                currentHealth = Mathf.Min(currentHealth + amount, charData.MaxHealth);
                healthBarCoroutine = StartCoroutine(AnimateHealthBar(currentHealth));
                break;
        }        
        StatusDisplay.Instance.AttStatusPlayer(this, charData);
    }    
    private void RemoveShieldBonus(int amount)
    {
        tempShieldBonus -= amount;
        StatusDisplay.Instance.AttStatusPlayer(this, charData);
    }
    private void RemoveDamageBonus(int amount)
    {
        tempDamageBonus -= amount;
        StatusDisplay.Instance.AttStatusPlayer(this, charData);
    }

    private void ClearBonus(string typeBonus)
    {

        switch (typeBonus)
        {
            case "Shield":
                tempShieldBonus = 0;
                break;
            case "Damage":
                tempDamageBonus = 0;
                break;
        }

        StatusDisplay.Instance.AttStatusPlayer(this, charData);
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
