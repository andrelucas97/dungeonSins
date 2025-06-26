using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTextColorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    private Color normalColor = Color.white;
    private Color hoverColor = new Color32(0x67, 0x00, 0x00, 0xFF);

    private float transitionSpeed = 5f;
    private float scaleSpeed = 5f;
    private float hoverScale = 1.1f;


    private Color targetColor;
    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();

        targetColor = normalColor;
        text.color = normalColor;

        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        text.color = Color.Lerp(text.color, targetColor, Time.deltaTime * transitionSpeed);

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetColor = hoverColor;
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = normalColor;
        targetScale = originalScale;
    }
}   
