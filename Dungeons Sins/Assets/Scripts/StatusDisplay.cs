using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    // ATUALIZANDO
    [Header("Stats Minion")]
    [SerializeField] private TextMeshProUGUI nivelMinion;
    [SerializeField] private TextMeshProUGUI healthMinion;
    [SerializeField] private TextMeshProUGUI shieldMinion;
    [SerializeField] private TextMeshProUGUI damageMinion;

    [Header("Stats Player")]
    [SerializeField] private TextMeshProUGUI nivelPlayer;
    [SerializeField] private TextMeshProUGUI healthPlayer;
    [SerializeField] private TextMeshProUGUI shieldPlayer;
    [SerializeField] private TextMeshProUGUI damagePlayer;
    public static StatusDisplay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;       
    }

    private void ShowStatusNew(int nivel, int health, int shield, int damage, TextMeshProUGUI nivelText, TextMeshProUGUI healthText, TextMeshProUGUI shieldText, TextMeshProUGUI damageText)
    {
        nivelText.text = "1"; // adicionar!!!
        healthText.text = health.ToString();
        shieldText.text = shield.ToString();
        damageText.text = damage.ToString();
    }

    public void AttStatusPlayer(CharStats charStats, CharacterData charData)
    {

        ShowStatusNew(1, charStats.CurrentHealth, charStats.Shield, charStats.Damage, nivelPlayer, healthPlayer, shieldPlayer, damagePlayer);
    }
    public void AttStatusMinion(MinionStats minionStats, CardData cardData)
    {

        ShowStatusNew(1, minionStats.CurrentHealth, minionStats.Shield, minionStats.Damage, nivelMinion, healthMinion, shieldMinion, damageMinion);
    }
}
