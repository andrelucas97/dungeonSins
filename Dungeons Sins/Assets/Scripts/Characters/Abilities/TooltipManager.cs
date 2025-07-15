using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private GameObject tooltipPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        tooltipPanel.SetActive(false);

    }

    public void ShowTooltip(string text, Vector3? screenPosition = null)
    {
        tooltipText.text = text;
        tooltipPanel.SetActive(true);

        if (screenPosition.HasValue)
        {
            tooltipPanel.transform.position = screenPosition.Value + new Vector3(20, -20, 0);
        }
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
        tooltipText.text = "";
    }
}
