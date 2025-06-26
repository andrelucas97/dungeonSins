using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        int selected = GameData.SelectedCharacterIndex;

        if (selected < 0 || selected >= characterDatas.Length)
            selected = 0;

        CharacterData selectedData = characterDatas[selected];

        CharStats stats = playerPrefab.GetComponent<CharStats>();
        if (stats != null)
        {
            stats.Initialize(selectedData);
        }

        CharUI ui = playerPrefab.GetComponentInChildren<CharUI>();
        if (ui != null)
        {
            ui.Setup(selectedData);
        }
    }
}
