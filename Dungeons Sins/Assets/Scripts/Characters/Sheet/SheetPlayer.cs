using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetPlayer : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Transform slotWapon1;
    [SerializeField] private Transform slotWapon2;
    [SerializeField] private Transform slotHelmet;
    [SerializeField] private Transform slotArmor;
    [SerializeField] private Transform slotBoots;
    [SerializeField] private Transform slotBackpack;

    private CardDisplayManager cardManager;
    [SerializeField] private CharStats charStats;
}
