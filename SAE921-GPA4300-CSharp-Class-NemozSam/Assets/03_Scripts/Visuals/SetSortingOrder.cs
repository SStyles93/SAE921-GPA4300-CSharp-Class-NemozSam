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
        _baseOrder = _sp.sortingOrder;
    }

    /// <summary>
    /// Called via message by SetOrderByHeight, add the base sorting order to the new one before applying it
    /// </summary>
    /// <param name="order">The order that corresponds to the height in the scene</param>
    void OnOrderByHeightChanged(int order)
    {
        _sp.sortingOrder = _baseOrder + order;
    }
}
