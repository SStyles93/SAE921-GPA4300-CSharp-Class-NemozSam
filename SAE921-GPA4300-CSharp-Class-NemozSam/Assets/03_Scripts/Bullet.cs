using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Slowable))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10.0f;
    [SerializeField] float _slowedSpeed = 2.0f;
    bool _slowed = false;

    Rigidbody2D _rb;
    Slowable _slowable;

    // Start is called before the first frame update
    void Start()
    {
        //Set the initial speed
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.up * _speed;

        //Set the on slow and unSlow actions
        _slowable = GetComponent<Slowable>();
        _slowable.AddSlowAction(() => _slowed = true);
        _slowable.AddUnSlowAction(() => _slowed = false);
    }

    private void FixedUpdate()
    {
        //Flip the rotation to be in the same direction as the velocity
        transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(_rb.velocity.x, _rb.velocity.y) * Mathf.Rad2Deg);

        _rb.velocity = transform.up * (_slowed? _slowedSpeed : _speed);
    }
}
