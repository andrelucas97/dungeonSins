using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
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

    public void AttStatusPlayer(CharStats charStats)
    {
        StatusDisplay.Instance.ShowStatus(
            $"Vida atual: {charStats.CurrentHealth}" +
            $"\nDefesa: {charStats.Shield}" +
            $"\nAtaque Atual: {charStats.Damage}"
            , damageTextPlayer);
    }
    public void AttStatusMinion(MinionStats minionStats)
    {

        StatusDisplay.Instance.ShowStatus(
            $"Vida atual: {minionStats.CurrentHealth}" +
            $"\nDefesa: {minionStats.Shield}" +
            $"\nAtaque Atual: {minionStats.Damage}"
            , damageTextMinion
            );
    }
}
