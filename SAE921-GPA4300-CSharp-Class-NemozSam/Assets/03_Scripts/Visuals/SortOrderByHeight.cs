using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderByHeight : MonoBehaviour
{
    /// <summary>
    /// Increase this value to magnify the difference in sorting order
    /// </summary>
    static private float granularity = 7.0f;

    ///disable that if you implement your own way to change the sorting order through the OnOrderByHeightChanged message
    [SerializeField] bool _changeRendererDirectly = true;
    SpriteRenderer _sp;
    int _baseOrder;

    int _orderByHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (_changeRendererDirectly)
        {
            _sp = GetComponent<SpriteRenderer>();
            _baseOrder = _sp.sortingOrder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Assign a new sorting order based on the inverse of the height (lower transforms have higher sorting layer)
        int newOrder = (int)(-transform.position.y * granularity);

        //Change the order and apply the changes only if the value is different
        if (newOrder != _orderByHeight)
        {
            _orderByHeight = newOrder;

            if (_changeRendererDirectly)
            {
                //Apply the changes directly
                _sp.sortingOrder = _baseOrder + _orderByHeight;
            }
            else
            {
                //Send the messages to this gameObject and its children
                BroadcastMessage("OnOrderByHeightChanged", _orderByHeight);
            }
        }
    }
}
