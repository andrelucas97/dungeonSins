using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField] private MinionsCard minionData;
    [SerializeField] private TextMeshProUGUI damageTextMinion;
    [SerializeField] private TextMeshProUGUI damageTextPlayer;
    [SerializeField] private TextMeshProUGUI nameMinion;
    [SerializeField] private TextMeshProUGUI namePlayer;
    public static StatusDisplay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;       
    }
    private void ShowStatus(string message, TextMeshProUGUI text)
    {
        text.text = message;
    }

    public void AttStatusPlayer(CharStats charStats, CharacterData charData)
    {

        namePlayer.text = $"{charData.CharName}, {charData.CodeName}";

        StatusDisplay.Instance.ShowStatus(
            $"Vida atual: {charStats.CurrentHealth}" +
            $"\nDefesa: {charStats.Shield}" +
            $"\nAtaque Atual: {charStats.Damage}"
            , damageTextPlayer);
    }
    public void AttStatusMinion(MinionStats minionStats, CardData cardData)
    {
        
        nameMinion.text = cardData.CardName;

        StatusDisplay.Instance.ShowStatus(
            $"Vida atual: {minionStats.CurrentHealth}" +
            $"\nDefesa: {minionStats.Shield}" +
            $"\nAtaque Atual: {minionStats.Damage}"
            , damageTextMinion
            );
    }
}
