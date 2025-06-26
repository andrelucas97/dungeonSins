using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Teste : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector2 originalPosition;
    private LayoutElement layoutElement;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    [SerializeField] private CardDisplayManager cardManager;
    [SerializeField] private CharStats charStats;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        layoutElement = GetComponent<LayoutElement>();
        canvasGroup = GetComponent<CanvasGroup>();
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        // 1. Ignora o layout
        if (layoutElement != null)
            layoutElement.ignoreLayout = true;

        // 2. Muda o pai para fora do layout
        transform.SetParent(canvas.transform, true);

        // 3. Centraliza o pivot pra evitar bugs de posição
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // 4. Permite passar por Raycasts
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localMousePos);

        rectTransform.anchoredPosition = localMousePos;

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            layoutElement.ignoreLayout = false;
        }
        else
        {
            CardUtils.SetCardSize(rectTransform, 0.75f);
        }

        rectTransform.localPosition = Vector3.zero;

    }
}
