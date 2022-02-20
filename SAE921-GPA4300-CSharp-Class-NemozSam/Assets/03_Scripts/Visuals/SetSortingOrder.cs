using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Companion to SetOrderByHeight, apply the changes to the children of a gameObject
/// </summary>
public class SetSortingOrder : MonoBehaviour
{
    int _baseOrder;
    SpriteRenderer _sp;

    // Start is called before the first frame update
    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sortingOrder = _baseOrder;
    }

    void OnOrderByHeightChanged(int order)
    {
        _sp.sortingOrder = _baseOrder + order;
    }
}
