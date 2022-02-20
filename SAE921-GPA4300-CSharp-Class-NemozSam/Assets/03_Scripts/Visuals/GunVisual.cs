using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVisual : MonoBehaviour
{
    [SerializeField] GameObject _rotationPoint;
    SpriteRenderer _sp;

    int _orderByHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _sp.sortingOrder = _orderByHeight +
            (Mathf.Abs((_rotationPoint.transform.rotation.eulerAngles.z + 90.0f) % 360.0f - 90.0f) < 90.0f ? -1 : 3);
    }

    public void OnOrderByHeightChanged(int order)
    {
        _orderByHeight = order;
    }
}
