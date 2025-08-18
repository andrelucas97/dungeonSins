using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLog : MonoBehaviour
{
    public static CombatLog Instance;

    [SerializeField] private Transform logContainer;
    [SerializeField] private GameObject logMessagePrefab;
    [SerializeField] private int maxMessages = 50;

    private Queue<GameObject> messages = new Queue<GameObject>();

    [Header("Box Ability")]
    [SerializeField] private TextMeshProUGUI textBoxAbility;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMessage(string message)
    {
        GameObject newMsg = Instantiate(logMessagePrefab, logContainer);
        newMsg.GetComponent<TextMeshProUGUI>().text = message;

        messages.Enqueue(newMsg);

        if (messages.Count > maxMessages)
        {
            GameObject oldMsg = messages.Dequeue();
            Destroy(oldMsg);
        }
    }

    public void MessageBoxAbility(string abilityName)
    {
        textBoxAbility.text = $"Deseja ativar habilidade \n" +
            $"{abilityName}?";
    }

    public void MessageBoxCard(string cardName)
    {
        textBoxAbility.text = $"Deseja ativar a carta \n" +
            $"{cardName}?";
    }

}
