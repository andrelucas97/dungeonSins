using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMinion", menuName = "CardsGame/Cards/Minion")]
public class MinionsCard : CardData
{
    // VAR PRIVADAS
    [Header("Element")]
    [SerializeField] private ElementType elementType;
    [SerializeField] private Sprite elementImage;

    [Header("LevelStat")]
    [SerializeField] private int level;
    [SerializeField] private MinionLevelStats level1;
    [SerializeField] private MinionLevelStats level2;
    [SerializeField] private MinionLevelStats level3;

    // VAR PUBLICAS
    public ElementType ElementType => elementType;
    public Sprite ElementImage => elementImage;
    public MinionLevelStats Level1 => level1;
    public MinionLevelStats Level2 => level2;
    public MinionLevelStats Level3 => level3;

}
