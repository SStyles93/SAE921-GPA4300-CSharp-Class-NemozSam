using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Slowable : MonoBehaviour
{
    int _slowEffectCount;

    private UnityAction _onSlow;
    private UnityAction _onUnSlow;

    public void AddSlowAction(UnityAction action)
    {
        _onSlow += action;
    }

    public void AddUnSlowAction(UnityAction action)
    {
        _onUnSlow += action;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_slowEffectCount++ == 0)
        {
            _onSlow?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (--_slowEffectCount == 0)
        {
            _onUnSlow?.Invoke();
        }
    }
}
