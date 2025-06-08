using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{

    public GameObject characterSelectPanel;
    public GameObject panelChar;
    public GameObject deck;
    public GameObject cardsController;
    public GameObject buttonMinion;

    public GameObject inGame;

    public CharacterData[] characters;
    public CharUI fichaUI;



    void Start()
    {
        characterSelectPanel.SetActive(true);
        inGame.SetActive(false);
    }

    public void OnCharacterSelected(int index)
    {
        inGame.SetActive(true);
        characterSelectPanel.SetActive(false);
        fichaUI.Setup(characters[index]);
    }
}
