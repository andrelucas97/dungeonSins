using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class CardDisplayManager : MonoBehaviour
{
    public GameObject cardEquip;
    public GameObject cardMinion;
    public GameObject cardModifier;
    public Transform posEquips;
    public Transform posMinion;

    public List<CardData> cardList = new List<CardData>();

    // LISTA DECK MINIONS
    private List<MinionsCard> deckMinions = new List<MinionsCard>();

    // LISTA DECK EQUIP
    private List<EquipmentCard> deckEquip = new List<EquipmentCard>();

    // LISTA DECK MODIFIER
    private List<ModifierCard> deckModifier = new List<ModifierCard>();
    
    //LISTA CARD DATA
    private List<CardData> deckAllCards = new List<CardData>();
    private List<GameObject> deckGO = new List<GameObject>();

    //LISTA CARD NA FICHA
    private List<GameObject> cardsCharSheet = new List<GameObject>();

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

        Debug.Log("Quantidade de cartas no deck Agora: " + deckAllCards.Count);

        // Iniciar Lacaio
        ClearCards(spawnedCardMinion);
        SpawnMinion();
    }

    public void SpawnAllCards()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            SpawnCard(cardList[i], i);
        }
    }

    public void SpawnRandomCards()
    {
        Debug.Log("Quantidade de cartas no deck: " + deckAllCards.Count);

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
            GameObject cardGO = SpawnCard(hand[i], i);
            spawnedCardEquip.Add(cardGO);
        }
    }

    public void SpawnMinion()
    {
        ClearCards(spawnedCardMinion);       

        if (deckMinions.Count == 0)
        {
            Debug.LogWarning("Deck de lacaios vazio!");
            return;
        }

        int index = UnityEngine.Random.Range(0, deckMinions.Count);
        MinionsCard randomMinion = deckMinions[index];

        GameObject minionGO = SpawnMinionCard(randomMinion);
        spawnedCardMinion.Add(minionGO);
    }

    private void ClearCards(List<GameObject> spawnedCard)
    {
        foreach (var obj in spawnedCard)
        {
            Destroy(obj);
        }
        spawnedCard.Clear();
    }

    private GameObject SpawnMinionCard(MinionsCard randomMinion)
    {
        GameObject cardGO = Instantiate(cardMinion, posMinion);
        RectTransform rt = cardGO.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        CardMinionUI cardMinionUI = cardGO.GetComponent<CardMinionUI>();
        cardMinionUI.Setup(randomMinion);
        return cardGO;
    }

    private GameObject SpawnCard(CardData cardToShow, int index)
    {

        GameObject prefabToUse = null;

        if (cardToShow is EquipmentCard)
        {
            prefabToUse = cardEquip;
        }
        else if (cardToShow is ModifierCard) 
        {
            prefabToUse = cardModifier;
        }
        else
        {
            Debug.LogWarning("Tipo de carta desconhecido!");
            return null;
        }

        GameObject cardGO = Instantiate(prefabToUse);
        cardGO.transform.SetParent(posEquips, false);

        RectTransform rt = cardGO.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
        rt.anchoredPosition = new Vector2(index * 110, 0);

        if (cardGO.TryGetComponent<CardEquipUI>(out var equipUI))
        {
            equipUI.Setup(cardToShow);
        }
        else if (cardGO.TryGetComponent<CardModifierUI>(out var modifierUI))
        {
            modifierUI.Setup(cardToShow);
        }

        return cardGO;
    }

    public void RemoveCardDeck(GameObject cardGO)
    {
        var cardDisplay = cardGO.GetComponent<CardEquipUI>();
        var cardData = cardDisplay.CardData;


        spawnedCardEquip.Remove(cardGO);
        hand.Remove(cardData);
        Debug.Log("Quantidade de cartas na mão: " + hand.Count);
    }
    public void AddCardDeck(GameObject cardGO)
    {
        cardsCharSheet.Add(cardGO);
    }
}
