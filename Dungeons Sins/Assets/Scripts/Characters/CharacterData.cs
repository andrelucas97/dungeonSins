using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CardsGame/Character")]
public class CharacterData : ScriptableObject
{
    [Header("Char Name")]
    [SerializeField] private string charName;
    [SerializeField] private string codeName;
    [SerializeField] private int age;

    [Header("Class")]
    [SerializeField] private string classChar;
    [SerializeField] private ClassChar typeClass;

    [Header("Image")]
    [SerializeField] private Sprite portrait; //image Personagem

    [Header("Color Char")]
    [SerializeField] private CharacterColors charColor;

    [Header("Stats")]
    [SerializeField] private int maxHealth; //vida maxima
    [SerializeField] private int currentHealth;
    [SerializeField] private int shield;
    [SerializeField] private int damage;

    [Header("Ability")]
    [SerializeField] private List<CharacterAbility> abilities;
    [SerializeField] private GameObject characterPrefab;

    public string CharName => charName;
    public string CodeName => codeName;
    public int Age => age;
    public string ClassChar => classChar;
    public Sprite Portrait => portrait;
    public CharacterColors CharColor => charColor;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int Shield => shield;
    public int Damage => damage;
    public List<CharacterAbility> Abilities => abilities;
    public GameObject CharacterPrefab => characterPrefab;
}
