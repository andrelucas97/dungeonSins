using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetPlayer : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Transform slotWapon1;
    [SerializeField] private Transform slotWapon2;
    [SerializeField] private Transform slotHelmet;
    [SerializeField] private Transform slotArmor;
    [SerializeField] private Transform slotBoots;
    [SerializeField] private Transform slotBackpack;

    private CardDisplayManager cardManager;
    [SerializeField] private CharStats charStats;

    private void Start()
    {
        cardManager = FindObjectOfType<CardDisplayManager>();
    }

    public void EquipCard(GameObject cardGO, EquipmentCard card, TypeCardEquip type)
    {
        switch (type)
        {
            case TypeCardEquip.Weapon:

                if (slotWapon1.childCount == 0)
                {
                    ShowCardInSlot(cardGO ,card, slotWapon1);
                } else if (slotWapon2.childCount == 0)
                {
                    ShowCardInSlot(cardGO, card, slotWapon2);
                } else
                {
                    Debug.Log("Ambos os slots de arma estão ocupados!");
                }             
                break;

            case TypeCardEquip.Helmet:
                ShowCardInSlot(cardGO, card, slotHelmet);
                break;
            case TypeCardEquip.Armor:
                ShowCardInSlot(cardGO, card, slotArmor);
                break;
            case TypeCardEquip.Boots:
                ShowCardInSlot(cardGO, card, slotBoots);
                break;
        }
    }

    private void ShowCardInSlot(GameObject cardGO, EquipmentCard card, Transform slotPlayer)
    {
        if (slotPlayer.childCount > 0)
        {
            Debug.Log("Inventário Ocupado!");
            return;
        }
        cardManager.RemoveCardDeck(cardGO);
        cardManager.AddCardDeck(cardGO);

        cardGO.transform.SetParent(slotPlayer, false);
        RectTransform rt = cardGO.GetComponent<RectTransform>();

        LayoutElement layoutElement = cardGO.GetComponent<LayoutElement>();

        if (layoutElement != null)
        {
            layoutElement.ignoreLayout = true;
        }

        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;

        CardUtils.SetCardSize(rt, 0.80f);

        var fitter = cardGO.GetComponent<ContentSizeFitter>();
        if (fitter != null)
            fitter.enabled = false;

        Animator anim = cardGO.GetComponent<Animator>();
        if (anim != null)
        {
            StartCoroutine(PlayAndDisable(anim, "Disabled"));
            
        }           

        cardGO.GetComponent<CardEquipUI>().Setup(card);

        Debug.Log("cardGO: " + cardGO);
        Debug.Log("card: " + card);
        Debug.Log("slotPlayer: " + slotPlayer);

        charStats.UpdateStats();
    }    

    IEnumerator PlayAndDisable(Animator anim, string animName)
    {
        anim.Play(animName);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.enabled = false;
    }
}
