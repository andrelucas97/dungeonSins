using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CardsGame/Character")]
public class CharacterData : ScriptableObject
{
    public string charName;
    public string codeName;
    public Sprite portrait; //image Personagem
    public CharacterColors charColor;

    public int maxHealth; //vida maxima
    public int currentHealth; //vida atual
    public int shield;
    public int damage;

    public CharacterAbility ability;

    public EquipmentItem weaponSlot1;
    public EquipmentItem weaponSlot2;
    public EquipmentItem armor;
    public EquipmentItem boots;
    public EquipmentItem hat;
    public EquipmentItem backpack;
}
