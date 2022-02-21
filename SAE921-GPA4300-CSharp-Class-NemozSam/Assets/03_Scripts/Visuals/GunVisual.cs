using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVisual : MonoBehaviour
{
    [SerializeField] GameObject _rotationPoint;
    SpriteRenderer _sp;

    /// <summary>
    /// the sprites the gun switch to when the orientation is changed
    /// </summary>
    [SerializeField] List<Sprite> _sprites = new List<Sprite>(8);

    int _orderByHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set the sorting order according to if the gun is above or below the player
        _sp.sortingOrder = _orderByHeight +
            (Mathf.Abs((_rotationPoint.transform.rotation.eulerAngles.z + 90.0f) % 360.0f - 90.0f) < 90.0f ? -1 : 3);

        //Ensure the gun doesn't spin and only its sprite does
        transform.rotation = Quaternion.identity;

        //Change the sprite according to the rotation
        _sp.sprite = _sprites[Mathf.RoundToInt(_rotationPoint.transform.rotation.eulerAngles.z / 45.0f) % 8];
    }

    public void OnOrderByHeightChanged(int order)
    {
        _orderByHeight = order;
    }
}
