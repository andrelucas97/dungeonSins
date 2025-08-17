using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPreviewManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Collection Manager")]

    [SerializeField]private CardData cardData;
    private bool hoverPreviewEnabled = false;

    void Start()
    {
        string nameScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (nameScene == "Collection")
            hoverPreviewEnabled = true;
        else hoverPreviewEnabled = false;
    }

    public void Setup(CardData data)
    {
        cardData = data;        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverPreviewEnabled) return;

        CollectionManager.Instance.ShowPreview(cardData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hoverPreviewEnabled) return;

        CollectionManager.Instance.HidePreview();
    }
}
