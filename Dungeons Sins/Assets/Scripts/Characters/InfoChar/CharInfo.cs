using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : MonoBehaviour
{
    public string Name;
    public ClassChar Class;
    public CharacterColors Color;
    public List<CharacterAbility> Abilities;

    public CharInfo(string name, ClassChar @class, CharacterColors color, List<CharacterAbility> abilities)
    {
        Name = name;
        Class = @class;
        Color = color;
        Abilities = abilities;
    }
}
