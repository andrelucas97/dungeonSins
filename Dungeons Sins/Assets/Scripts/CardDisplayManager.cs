using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;

public class CardDisplayManager : MonoBehaviour
{
    // VAR PRIVADAS

    [Header("Cards")]
    [SerializeField] private GameObject cardEquip;
    [SerializeField] private GameObject cardMinion;
    [SerializeField] private GameObject cardModifier;

    [Header("Buttons")]
    [SerializeField] private GameObject buttonSkip;
    [SerializeField] private GameObject buttonAttack;

    [Header("Positions")]
    [SerializeField] private Transform posEquips;
    [SerializeField] private Transform posMinion;

    [Header("Others")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private DiceRoller diceRoller;

    private List<CardData> cardList = new List<CardData>();
    // LISTA DECK MINIONS
    private List<MinionsCard> deckMinions = new List<MinionsCard>();
    // LISTA DECK EQUIP
    private List<EquipmentCard> deckEquip = new List<EquipmentCard>();
    // LISTA DECK MODIFIER
    private List<ModifierCard> deckModifier = new List<ModifierCard>();    
    //LISTA CARD DATA
    private List<CardData> deckAllCards = new List<CardData>();
    private List<CardData> minionsInGame = new List<CardData>();
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

        List<EquipmentCard> deckEquip = Resources.LoadAll<EquipmentCard>("ScriptableObjects/Cards/Equipments").ToList();
        List<ModifierCard> deckModifier = Resources.LoadAll<ModifierCard>("ScriptableObjects/Cards/Modifiers").ToList();
        List<MinionsCard> deckMinions = Resources.LoadAll<MinionsCard>("ScriptableObjects/Cards/Minions").ToList();

        
        // Add Cartas Equips + Modifier
        deckAllCards.AddRange(deckEquip);
        deckAllCards.AddRange(deckModifier);
        minionsInGame.AddRange(deckMinions);

        // Iniciar Lacaio
        //ClearCards(spawnedCardMinion);
        SpawnMinion();
    }

    private void SpawnAllCards()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            SpawnCard(cardList[i], i, posEquips, 1);
        }
    }

    public void ButtonSpawnCards()
    {
        SpawnCardsEquips();
    }
    public void ButtonSkipCards()
    {
        ReturnHandToDeck();
        actionManager.CheckEndOfTurn(this);
    }
    public void ClearShopCards()
    {
        ReturnHandToDeck();
    }
    public void SpawnMinion()
    {
        SpawnMinionGame();
    }
    public void RemoveCardDeck(GameObject cardGO)
    {
        RemoveDeck(cardGO);
    }
    public void AddCardSheet(GameObject cardGO)
    {
        cardsCharSheet.Add(cardGO);
    }
    public void RemoveCardSheet(GameObject cardGO)
    {
        cardsCharSheet.Remove(cardGO);
    }

    public void SkipCard()
    {
        CheckButtonSkip();
    }


    private void CheckButtonSkip()
    {
        if (hand.Count > 0)
        {
            buttonSkip.SetActive(true);
        }
        else
        {
            buttonSkip.SetActive(false);
        }
    }

    private void SpawnCardsEquips()
    {
        var hasActive = actionManager.TryUseAction();
        if (!hasActive) return;

        ReturnHandToDeck();

        if (deckAllCards.Count < 3)
        {
            CombatLog.Instance.AddMessage("Menos de 3 cartas disponíveis.");

            if (deckAllCards.Count == 0)
            {
                CombatLog.Instance.AddMessage("Baralho vazio!");
                return;
            }
        }

        var selected = deckAllCards.OrderBy(x => UnityRandom.value).Take(3).ToList();

        foreach (var card in selected)
        {
            deckAllCards.Remove(card);
            hand.Add(card);
        }

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject cardGO = SpawnCard(hand[i], i, posEquips, 2);
            spawnedCardEquip.Add(cardGO);
        }

        buttonAttack.GetComponent<Button>().enabled = false;

        SkipCard();
    }
    private void ReturnHandToDeck()
    {
        ClearCards(spawnedCardEquip);
        deckAllCards.AddRange(hand);
        hand.Clear();
        buttonSkip.SetActive(false);
        buttonAttack.GetComponent<Button>().enabled = true;

    }
    private void SpawnMinionGame()
    {
        ClearCards(spawnedCardMinion, spawnedCardMinion);

        if (minionsInGame.Count == 0)
        {
            Debug.LogWarning("Deck de lacaios vazio!");
            return;
        }
        int index = UnityEngine.Random.Range(0, minionsInGame.Count);
        CardData randomMinion = minionsInGame[index];

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

            MinionAttack minionAttack = cardGO.GetComponent<MinionAttack>();


            if (diceRoller == null)
            {
                Debug.LogError("diceRoller está NULL!", this);     
                diceRoller = FindObjectOfType<DiceRoller>();

            }

            if (diceRoller == null)
            {
                Debug.LogError("diceRoller está NULL!", this);
            }
            minionAttack.SetDependencies(diceRoller, actionManager);

        }
        return cardGO;
    }
    private void ClearCards(List<GameObject> spawnedCard, List<GameObject> removeList = null)
    {
        for (int i = spawnedCard.Count - 1; i >= 0; i--)
        {
            GameObject obj = spawnedCard[i];

            if (removeList != null)
            {
                var cardDisplay = obj.GetComponent<CardMinionUI>();

                minionsInGame.Remove(cardDisplay.CardData);
            }
            Destroy(obj);
        }
        spawnedCard.Clear();
    }
    private void RemoveDeck(GameObject cardGO)
    {
        var cardEquip = cardGO.GetComponent<CardEquipUI>();

        if (cardEquip != null)
        {
            var cardData = cardEquip.CardData;
            spawnedCardEquip.Remove(cardGO);
            hand.Remove(cardData);
        }

        var cardModifier = cardGO.GetComponent<CardModifierUI>();

        if (cardModifier != null)
        {
            var cardData = cardModifier.CardData;
            spawnedCardEquip.Remove(cardGO);
            hand.Remove(cardData);
        }
    }

    public List<CardData> GetAllCards()
    {
        if (deckAllCards == null || deckAllCards.Count == 0)
        {
            deckAllCards = new List<CardData>();
            deckAllCards.AddRange(deckMinions);
            deckAllCards.AddRange(deckEquip);
            deckAllCards.AddRange(deckModifier);
        }

        return deckAllCards;
    }
}
