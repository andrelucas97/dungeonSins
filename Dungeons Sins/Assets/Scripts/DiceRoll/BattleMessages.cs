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
        "Parecia promissor... at� o escudo dizer n�o.",
        "O ataque ricocheteou como se fosse nada.",
        "O escudo brilhou... e a chance sumiu.",
        "Um golpe sem impacto. O escudo aguentou firme.",
        "Voc� tentou... mas o escudo tentou mais.",
        "Nada mal... mas n�o foi o bastante.",
        "A flecha voou, mas o destino desviou.",
        "Quase! Mas o lacaio ainda sorri.",
        "Seu ataque encontrou... resist�ncia pura.",
        "O dado n�o foi seu aliado dessa vez."
    };

    private readonly string[] criticalFailMessages = new string[]
{
    "Falha cr�tica! Voc� trope�ou no pr�prio ego. Tome o dano!",
    "O inimigo nem percebeu que voc� atacou. Tome o dano!",
    "Sua arma falhou... e o orgulho tamb�m. Tome o dano!",
    "Uma falha t�o grande que at� o universo ficou constrangido. Tome o dano!",
    "Voc� perdeu a chance e ganhou vergonha. Tome o dano!"
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
