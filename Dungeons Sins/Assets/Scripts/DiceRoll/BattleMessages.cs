using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMessages : MonoBehaviour
{
    public static BattleMessages Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private readonly string[] failedAttackMessages = new string[]
    {
        "Parecia promissor... até o escudo dizer não.",
        "O ataque ricocheteou como se fosse nada.",
        "O escudo brilhou... e a chance sumiu.",
        "Um golpe sem impacto. O escudo aguentou firme.",
        "Você tentou... mas o escudo tentou mais.",
        "Nada mal... mas não foi o bastante.",
        "A flecha voou, mas o destino desviou.",
        "Quase! Mas o lacaio ainda sorri.",
        "Seu ataque encontrou... resistência pura.",
        "O dado não foi seu aliado dessa vez."
    };

    private readonly string[] criticalFailMessages = new string[]
{
    "Falha crítica! Você tropeçou no próprio ego. Tome o dano!",
    "O inimigo nem percebeu que você atacou. Tome o dano!",
    "Sua arma falhou... e o orgulho também. Tome o dano!",
    "Uma falha tão grande que até o universo ficou constrangido. Tome o dano!",
    "Você perdeu a chance e ganhou vergonha. Tome o dano!"
};

    public string GetRandomFailMessage()
    {
        return failedAttackMessages[Random.Range(0, failedAttackMessages.Length)];
    }

    public string CriticalFail()
    {
        return criticalFailMessages[Random.Range(0, criticalFailMessages.Length)];
    }
}
