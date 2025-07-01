using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private enum AbilityButtonMode { CharacterSelect, InGame }
    [SerializeField] private AbilityButtonMode mode;
    [SerializeField] private AbilityData ability;
    [SerializeField] private CharacterAbility abilityData;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI tooltipText;


    public CharacterAbility AbilityData => abilityData;

    private void Awake()
    {
        if (tooltipPanel == null)
        {
            tooltipPanel = transform.Find("ToolTipPanel")?.gameObject;

            if (tooltipPanel == null)
                Debug.LogWarning("TooltipPanel filho não encontrado!");
        }
    }
    private void Start()
    {
            tooltipPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mode == AbilityButtonMode.CharacterSelect && tooltipPanel != null)
        {
            tooltipPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mode == AbilityButtonMode.CharacterSelect && tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    public void SetAbility(CharacterAbility ability)
    {
        abilityData = ability;
    }

    public void SetTooltipPanel(GameObject panel)
    {
        tooltipPanel = panel;
    }

    public void SetTooltipText(TextMeshProUGUI tooltipText)
    {
        if (ability != null)
        {
            string desc = ability.Description;

            if (ability.RequiresCondition && !string.IsNullOrEmpty(ability.ConditionText))
                desc += $"\n<color=#888><i>Condição: {ability.ConditionText}</i></color>";

            tooltipText.text = desc;
        }
        else
        {
            tooltipText.text = "Habilidade não configurada.";
        }
    }

    public void SetAbilityData(AbilityData data)
    {
        ability = data;
    }
}
