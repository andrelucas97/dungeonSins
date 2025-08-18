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
    private Vector3 originalScale;
    private LayoutElement layoutElement;
    private Canvas canvas;
    private CanvasGroup canvasGroup; 

    private bool draggable = false;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;

        string nameScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        

        if (nameScene == "GameScene")
            draggable = true;
        else draggable = false;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable) return;

        canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        layoutElement = GetComponent<LayoutElement>();
        canvasGroup = GetComponent<CanvasGroup>();
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        if (layoutElement != null)
            layoutElement.ignoreLayout = true;

        transform.SetParent(canvas.transform, true);

        rectTransform.localScale = Vector3.one * 0.75f;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggable) return;

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
        if (!draggable) return;

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            layoutElement.ignoreLayout = false;
            rectTransform.localScale = Vector3.one;
        }
        else
        {
            CardUtils.SetCardSize(rectTransform, 1f);
        }

        rectTransform.localPosition = Vector3.zero;

    }
    
}
