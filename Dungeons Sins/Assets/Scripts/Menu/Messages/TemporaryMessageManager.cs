using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMessageManager : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    private Coroutine currentCoroutine;
    public void ShowMessage(float duration)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowMessageCoroutine(duration));
    }

    private IEnumerator ShowMessageCoroutine(float duration)
    {
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);

        currentCoroutine = null;
    }
}
