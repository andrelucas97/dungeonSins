using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class CardDisplayManager : MonoBehaviour
{
    [SerializeField] private GameObject cardEquip;
    [SerializeField] private GameObject cardMinion;
    [SerializeField] private GameObject cardModifier;
    [SerializeField] private Transform posEquips;
    [SerializeField] private Transform posMinion;

    private List<CardData> cardList = new List<CardData>();

    // LISTA DECK MINIONS
    private List<MinionsCard> deckMinions = new List<MinionsCard>();

    // LISTA DECK EQUIP
    private List<EquipmentCard> deckEquip = new List<EquipmentCard>();

    // LISTA DECK MODIFIER
    private List<ModifierCard> deckModifier = new List<ModifierCard>();
    
    //LISTA CARD DATA
    private List<CardData> deckAllCards = new List<CardData>();

    //LISTA CARD NA FICHA
    private List<GameObject> cardsCharSheet = new List<GameObject>();
    public List<GameObject> CardsCharSheet => cardsCharSheet;
    // MINHA MAO COMPRA
    private List<CardData> hand = new List<CardData>();
    
    // LIMPAR GAME OBJECT CARD
    private List<GameObject> spawnedCardEquip = new List<GameObject>();
    private List<GameObject> spawnedCardMinion = new List<GameObject>();


    void Start()
    {
        //SpawnAllCards();

        List<EquipmentCard> deckEquip = Resources.LoadAll<EquipmentCard>("ScriptableObjects/Cards/Equipments").ToList();
        List<ModifierCard> deckModifier = Resources.LoadAll<ModifierCard>("ScriptableObjects/Cards/Modifiers").ToList();

        deckMinions = Resources.LoadAll<MinionsCard>("ScriptableObjects/Cards/Minions").ToList();
        
        // Add Cartas Equips + Modifier
        deckAllCards.AddRange(deckEquip);
        deckAllCards.AddRange(deckModifier);

        // Iniciar Lacaio
        ClearCards(spawnedCardMinion);
        SpawnMinion();
    }

    private void SpawnAllCards()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            SpawnCard(cardList[i], i, posEquips, 1);
        }
    }

    private void SpawnRandomCards()
    {
        ClearCards(spawnedCardEquip);

        deckAllCards.AddRange(hand);
        hand.Clear();

        if (deckAllCards.Count < 3)
        {
            Debug.Log("Menos de 3 cartas disponíveis.");
            return;
        }

        var selected = deckAllCards.OrderBy(x => UnityRandom.value).Take(3).ToList();

        foreach (var card in selected)
        {
            deckAllCards.Remove(card);
            hand.Add(card);
        }

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject cardGO = SpawnCard(hand[i], i, posEquips, 1);
            spawnedCardEquip.Add(cardGO);
        }
    }

    private void SpawnMinion()
    {
        ClearCards(spawnedCardMinion);       

        if (deckMinions.Count == 0)
        {
            Debug.LogWarning("Deck de lacaios vazio!");
            return;
        }
        int index = UnityEngine.Random.Range(0, deckMinions.Count);
        MinionsCard randomMinion = deckMinions[index];

        GameObject cardGO = SpawnCard(randomMinion, 0, posMinion, 2);
        spawnedCardMinion.Add(cardGO);
    }
    private GameObject SpawnCard(CardData cardToShow, int index, Transform position, float scale)
    {

        GameObject prefabToUse = null;

        if (cardToShow is EquipmentCard)
        {
            prefabToUse = cardEquip;
        }
        else if (cardToShow is ModifierCard) 
        {
            prefabToUse = cardModifier;
        } else if (cardToShow is MinionsCard)
        {
            prefabToUse = cardMinion;
        }
        else
        {
            Debug.LogWarning("Tipo de carta desconhecido!");
            return null;
        }

        GameObject cardGO = Instantiate(prefabToUse);
        cardGO.transform.SetParent(position, false);

        RectTransform rt = cardGO.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
        rt.anchoredPosition = new Vector2(index * 110, 0);

        CardUtils.SetCardSize(rt, scale);

        if (cardGO.TryGetComponent<CardEquipUI>(out var equipUI))
        {
            equipUI.Setup(cardToShow);
        }
        else if (cardGO.TryGetComponent<CardModifierUI>(out var modifierUI))
        {
            modifierUI.Setup(cardToShow);
        }
        else if (cardGO.TryGetComponent<CardMinionUI>(out var minionUI))
        {
            minionUI.Setup(cardToShow);
        }
            return cardGO;
    }

    private void ClearCards(List<GameObject> spawnedCard)
    {
        foreach (var obj in spawnedCard)
        {
            Destroy(obj);
        }
        spawnedCard.Clear();
    }



    public void RemoveCardDeck(GameObject cardGO)
    {
        var cardDisplay = cardGO.GetComponent<CardEquipUI>();
        var cardData = cardDisplay.CardData;


        spawnedCardEquip.Remove(cardGO);
        hand.Remove(cardData);
    }
    public void AddCardDeck(GameObject cardGO)
    {
        cardsCharSheet.Add(cardGO);
    }
}
