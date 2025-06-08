using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMinion", menuName = "CardsGame/Cards/Minion")]
public class MinionsCard : CardData
{
    public ElementType elementType;
    public Sprite elementImage;
    public MinionLevelStats level1;
    public MinionLevelStats level2;
    public MinionLevelStats level3;

}
