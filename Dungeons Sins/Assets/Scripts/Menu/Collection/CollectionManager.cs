using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private Transform contentParent;   

    [Header("Cards")]
    [SerializeField] private GameObject cardEquip;
    [SerializeField] private GameObject cardMinion;
    [SerializeField] private GameObject cardModifier;

    [Header("Preview")]
    [SerializeField] private GameObject panelPreviewCard;
    [SerializeField] private Transform previewRoot;
    [SerializeField] private GameObject equipmentPrefab;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private GameObject modifierPrefab;
    [SerializeField] private float zoomCard;
    private GameObject currentPreview;

    public CardData cardData;
    private List<CardData> deckAllCards = new List<CardData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {

        var deckEquip = Resources.LoadAll<EquipmentCard>("ScriptableObjects/Cards/Equipments").ToList();
        var deckModifier = Resources.LoadAll<ModifierCard>("ScriptableObjects/Cards/Modifiers").ToList();
        var deckMinions = Resources.LoadAll<MinionsCard>("ScriptableObjects/Cards/Minions").ToList();


        deckAllCards.AddRange(deckEquip);
        deckAllCards.AddRange(deckModifier);
        deckAllCards.AddRange(deckMinions);


        foreach (CardData card in deckAllCards)
        {
            GameObject prefabToUse = null;

            if (card is EquipmentCard) prefabToUse = cardEquip;
            else if (card is ModifierCard) prefabToUse = cardModifier;
            else if (card is MinionsCard) prefabToUse = cardMinion;

            GameObject cardGO = Instantiate(prefabToUse, contentParent, false);

            if (cardGO.TryGetComponent<CardEquipUI>(out var equipUI)) equipUI.Setup(card);
            else if (cardGO.TryGetComponent<CardModifierUI>(out var modifierUI)) modifierUI.Setup(card);
            else if (cardGO.TryGetComponent<CardMinionUI>(out var minionUI)) minionUI.Setup(card);
        }
    }

    public void ShowPreview(CardData card)
    {

        if (currentPreview != null) Destroy(currentPreview);

        if (card == null)
        {
            Debug.Log("CardData está null!");
            return;
        }

        if (card is EquipmentCard equipCard)
        {
            currentPreview = Instantiate(equipmentPrefab, previewRoot);
            currentPreview.transform.localScale = Vector3.one * zoomCard;

            var ui = currentPreview.GetComponent<CardEquipUI>();
            if (ui != null)
                ui.Setup(equipCard);
        }
        else if (card is MinionsCard minionCard)
        {
            currentPreview = Instantiate(minionPrefab, previewRoot);
            currentPreview.transform.localScale = Vector3.one * zoomCard;

            var ui = currentPreview.GetComponent<CardMinionUI>();
            if (ui != null)
                ui.Setup(minionCard);
        }
        else if (card is ModifierCard modifierCard)
        {
            currentPreview = Instantiate(modifierPrefab, previewRoot);
            currentPreview.transform.localScale = Vector3.one * zoomCard;

            var ui = currentPreview.GetComponent<CardModifierUI>();
            if (ui != null)
                ui.Setup(modifierCard);
        }

    }

    public void HidePreview()
    {
        //panelPreviewCard.SetActive(false);
    }

}
