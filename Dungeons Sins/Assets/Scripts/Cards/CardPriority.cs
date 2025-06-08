using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPriority : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public Transform previewParent; 
    private GameObject previewInstance;

    public GameObject cardPreviewPrefab;

    private Animator animator;
    private bool isHighlighted = false;


    void Awake()
    {
        animator = GetComponent<Animator>();

        if (cardPreviewPrefab == null)
        {
            cardPreviewPrefab = GameObject.Find("CardPreview");
        }
        if (previewParent == null)
        {
            GameObject teste = GameObject.Find("FirstCard");
            Debug.Log(teste == null ? "Não achou FirstCard" : "Achou FirstCard!");

            previewParent = GameObject.Find("FirstCard").transform;
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHighlighted) return;
        isHighlighted = true;
        animator.SetBool("isHighlighted", true);


        if (previewInstance != null) return;

        previewInstance = Instantiate(cardPreviewPrefab, previewParent);
        previewInstance.transform.position = transform.position;
        previewInstance.transform.localScale = Vector3.one * 1.1f;

        var source = GetComponent<CardMinionUI>();
        var preview = previewInstance.GetComponent<CardMinionUI>();

        if (source != null && preview != null)
        {
            preview.Setup(source.cardData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isHighlighted) return;
        isHighlighted = false;
        animator.SetBool("isHighlighted", false);


        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }
    }

}
