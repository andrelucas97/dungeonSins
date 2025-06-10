using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CardsGame/Character")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string charName;
    [SerializeField] private string codeName;
    [SerializeField] private Sprite portrait; //image Personagem
    [SerializeField] private CharacterColors charColor;
    [SerializeField] private int maxHealth; //vida maxima
    [SerializeField] private int shield;
    [SerializeField] private int damage;
    [SerializeField] private CharacterAbility ability;


    public string CharName => charName;
    public string CodeName => codeName;
    public Sprite Portrait => portrait;
    public CharacterColors CharColor => charColor;
    public int MaxHealth => maxHealth;
    public int Shield => shield;
    public int Damage => damage;
    public CharacterAbility Ability => ability;

    //public EquipmentItem weaponSlot1;
    //public EquipmentItem weaponSlot2;
    //public EquipmentItem armor;
    //public EquipmentItem boots;
    //public EquipmentItem hat;
    //public EquipmentItem backpack;
}
