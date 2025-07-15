using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AbilityButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private enum AbilityButtonMode { CharacterSelect, InGame }
    [SerializeField] private AbilityButtonMode mode;
    [SerializeField] private AbilityData ability;
    [SerializeField] private CharacterAbility abilityData;
    [SerializeField] private TextMeshProUGUI tooltipText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            mode = AbilityButtonMode.InGame;

        } else if (SceneManager.GetActiveScene().name == "CharacterSelection")
        {
            mode = AbilityButtonMode.CharacterSelect;
        }
    }

    public CharacterAbility AbilityData => abilityData;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (mode == AbilityButtonMode.InGame)
        {
            Debug.Log("Botao Clicado");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetTooltipText(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();

    }

    public void SetAbility(CharacterAbility ability)
    {
        abilityData = ability;
    }


    public void SetTooltipText(TextMeshProUGUI tooltipText)
    {
        if (ability != null)
        {
            string desc = ability.Description;

            if (ability.RequiresCondition && !string.IsNullOrEmpty(ability.ConditionText))
                desc += $"\n<color=#888><i>Condição: {ability.ConditionText}</i></color>";

            if (mode == AbilityButtonMode.InGame)
            {
                Vector3 mousePos = Input.mousePosition;
                TooltipManager.Instance.ShowTooltip(desc, mousePos);
            }
            else
            {
                TooltipManager.Instance.ShowTooltip(desc);
            }

        }
        else
        {
            TooltipManager.Instance.ShowTooltip("Habilidade não configurada.");
        }
    }

    public void SetAbilityData(AbilityData data)
    {
        ability = data;
    }
}
