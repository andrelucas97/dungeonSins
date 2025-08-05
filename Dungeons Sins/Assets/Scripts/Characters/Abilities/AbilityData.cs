using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityEffectType
{
    Damage,
    Heal,
    Buff,
    Debuff,
    Shield,
    StatusEffect,
    Utility,
}

public enum TargetType
{
    Self,
    Ally,
    Enemy,
    AllEnemies,
    AllAllies
}
public enum AbilityType
{
    Active,
    Passive
}

[CreateAssetMenu(fileName = "NewAbility", menuName = "CardsGame/Ability")]
public class AbilityData : ScriptableObject
{

    [Header("Name")]
    [SerializeField] private CharacterAbility abilityID;
    [SerializeField] private string abilityName;
    [TextArea][SerializeField] private string description;

    [Header("Type")]
    [SerializeField] private AbilityEffectType effectType;
    [SerializeField] private TargetType targetType;
    [SerializeField] private AbilityType type;

    [SerializeField] private int baseValue;

    [SerializeField] private bool requiresCondition;
    [SerializeField] private string conditionText;
    [SerializeField] private int duration;

    public CharacterAbility AbilityID => abilityID;
    public string AbilityName => abilityName;
    public string Description => description;
    public AbilityEffectType EffectType => effectType;
    public TargetType Target => targetType;
    public AbilityType Type => type;
    public int BaseValue => baseValue;
    public bool RequiresCondition => requiresCondition;
    public string ConditionText => conditionText;
    public int Duration => duration;
}
