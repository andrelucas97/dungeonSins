using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerParent;
    public CharacterData charData;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerParent.position, Quaternion.identity, playerParent);
        CharUI charUI = playerGO.GetComponent<CharUI>();
        charUI.Setup(charData);
    }

    void Update()
    {
        
    }
}
