    using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private UnityEvent onClickAction;
    [SerializeField] private AudioClip clickSound;

    public void Execute()
    {
        if (AudioManager.Instance != null && clickSound != null)
        {
            StartCoroutine(PlaySoundThenInvoke());
        }
        else
        {
            onClickAction.Invoke();
        }
    }

    private System.Collections.IEnumerator PlaySoundThenInvoke()
    {
        yield return AudioManager.Instance.PlaySoundAndWait(clickSound);
        onClickAction.Invoke();
    }
}