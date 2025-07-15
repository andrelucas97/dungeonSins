using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;
    public CharacterData SelectedCharacter;
    [Header("Player Default")]
    [SerializeField] private CharacterData defaultCharacter;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (SelectedCharacter == null)
            {
                SelectedCharacter = defaultCharacter;

            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static CharacterSelectionManager GetInstance()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("CharacterSelectionManager");
            Instance = go.AddComponent<CharacterSelectionManager>();
            DontDestroyOnLoad(go);
        }
        return Instance;
    }
}
