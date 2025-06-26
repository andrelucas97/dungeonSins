using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModifierSlot : MonoBehaviour, IDropHandler
{
    public SlotType slotType;

    [SerializeField] private CardDisplayManager cardManager;
    [SerializeField] private CharStats charStats;
    [SerializeField] private Transform slotWeapon1;
    [SerializeField] private Transform slotWeapon2;

    void Awake()
    {
        cardManager = FindObjectOfType<CardDisplayManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {

        var cardModifier = eventData.pointerDrag.GetComponent<CardModifierUI>();
        var cardEquip = eventData.pointerDrag.GetComponent<CardEquipUI>();

        if (cardModifier != null)
        {
            HandleDropMod(cardModifier, slotType);
        }
        else if (cardEquip != null)
            HandleDropEquip(cardEquip, slotType);
    }

    private void HandleDropEquip(CardEquipUI cardEquip, SlotType slot)
    {
        string typeCard = cardEquip.CardData.TypeCard.ToString();
        switch (slot)
        {
            case SlotType.Helmet:
                if (typeCard == "Helmet")
                {
                    EquipCard(cardEquip, typeCard);
                }
                break;
            case SlotType.Armor:
                if (typeCard == "Armor")
                {
                    EquipCard(cardEquip, typeCard);
                }
                break;
            case SlotType.Boots:
                if (typeCard == "Boots")
                {
                    EquipCard(cardEquip, typeCard);
                }
                break;
            case SlotType.Weapon1:
            case SlotType.Weapon2:
                if (typeCard == "Weapon")
                {
                    EquipCard(cardEquip, typeCard);
                }
                break;
            case SlotType.Backpack:
                if (typeCard == "Weapon")
                    {
                        EquipCardInSlot(cardEquip, typeCard);
                    }
                break;
        }        
    }

    private void EquipCard(CardEquipUI cardEquip, string typeCard)
    {
        string typeEquip = cardEquip.CardData.CardLabel.ToString();

        if (typeEquip == "TwoHands")
        {
            // Verifica se Weapon1 ou Weapon2 estão ocupados
            bool slot1Ocupado = HasCard(slotWeapon1, cardEquip);
            bool slot2Ocupado = HasCard(slotWeapon2, cardEquip);

            if (slot1Ocupado || slot2Ocupado)
            {
                Debug.Log("Espaço insuficiente para arma de duas mãos!");
                // Adicionar Deseja trocar? e aparecer tela sim ou nao.
                return;
            }

            RemoveOldCard(slotWeapon1, cardEquip);
            RemoveOldCard(slotWeapon2, cardEquip);

            cardEquip.transform.SetParent(slotWeapon1);
            cardEquip.transform.localPosition = Vector3.zero;
            cardEquip.transform.SetSiblingIndex(slotWeapon1.childCount - 1);

            GameObject marcador = new GameObject("TwoHandedMarker");
            marcador.transform.SetParent(slotWeapon2);
            marcador.transform.localPosition = Vector3.zero;

            Debug.Log("Arma de duas mãos equipada.");

            // Atualizações
            cardManager.RemoveCardDeck(cardEquip.gameObject);
            cardManager.ClearShopCards();
            cardManager.AddCardSheet(cardEquip.gameObject);
            cardManager.SkipCard();

            charStats.UpdateStats(typeCard);
            return;
        }

        if (typeEquip == "OneHand")
        {
            Transform marker = slotWeapon2.Find("TwoHandedMarker");
            if (marker != null)
            {
                Debug.Log("Não é possível equipar arma de uma mão enquanto uma arma de duas mãos está equipada!");
                // Exibir mensagem de aviso ou tela de troca
                return;
            }
        }
        

        // Armas normais (uma mão)
        RemoveOldCard(transform, cardEquip);
        EquipCardInSlot(cardEquip, typeCard);
    }

    private void EquipCardInSlot(CardEquipUI cardEquip, string typeCard)
    {
        cardEquip.transform.SetParent(transform);
        cardEquip.transform.localPosition = Vector3.zero;
        cardEquip.transform.SetSiblingIndex(transform.childCount - 1);

        Debug.Log($"cardManager: {cardManager}, cardEquip: {cardEquip}");

        cardManager.RemoveCardDeck(cardEquip.gameObject);
        cardManager.ClearShopCards();
        cardManager.AddCardSheet(cardEquip.gameObject);
        cardManager.SkipCard();

        charStats.UpdateStats(typeCard);
    }

    private void RemoveOldCard(Transform slot, CardEquipUI cardEquip)
    {
        foreach (Transform child in slot)
        {
            if (child.gameObject == cardEquip)
                continue;

            if (child.GetComponent<CardEquipUI>() != null || child.name == "TwoHandedMarker")
            {
                cardManager.RemoveCardSheet(child.gameObject);
                Destroy(child.gameObject);
            }
        }
    }

    private bool HasCard(Transform slot, CardEquipUI cardEquip)
    {
        foreach (Transform child in slot)
        {
            if (child.gameObject == cardEquip)
                continue;

            if (child.GetComponent<CardEquipUI>() != null)
                return true;

            if (child.name == "TwoHandedMarker")
                return true;
        }
        return false;
    }

    private void HandleDropMod(CardModifierUI cardModifier, SlotType slot)
    {

        string typeCard = cardModifier.CardData.TypeCard.ToString();

        if (slot == SlotType.Backpack)
        {
            // Remove carta antiga se existir
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            cardModifier.transform.SetParent(transform);
            cardModifier.transform.localPosition = Vector3.zero;
            cardModifier.transform.SetSiblingIndex(0);
            Debug.Log("Modifier equipado: " + cardModifier.CardData.CardName);

            cardManager.RemoveCardDeck(cardModifier.gameObject);
            cardManager.ClearShopCards();
            cardManager.AddCardSheet(cardModifier.gameObject);
            cardManager.SkipCard();


            charStats.UpdateStats(typeCard);
        }        
    }
}
