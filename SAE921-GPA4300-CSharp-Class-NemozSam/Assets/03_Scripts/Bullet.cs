using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Slowable))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10.0f;
    [SerializeField] float _slowedSpeed = 2.0f;
    [SerializeField] float _speedChangeRate = 2.0f;
    float _curSpeed;
    float _goalSpeed;

    Rigidbody2D _rb;
    Slowable _slowable;

    SpriteRenderer _sp;
    bool _canDamage = false;
    Color _baseColor;
    [SerializeField] Color _transparentColor;

    // Start is called before the first frame update
    void Start()
    {
        //Set the initial speed
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.up * _speed;
        _curSpeed = _goalSpeed = _speed;

        //Set the on slow and unSlow actions
        _slowable = GetComponent<Slowable>();
        _slowable.AddSlowAction(() => _goalSpeed = _slowedSpeed);
        _slowable.AddUnSlowAction(() => _goalSpeed = _speed);

        //Make the bullet transparent and unable to deal damage at first
        _sp = GetComponentInChildren<SpriteRenderer>();
        _baseColor = _sp.color;
        _sp.color = _transparentColor;
    }

    private void FixedUpdate()
    {
        //Flip the rotation to be in the same direction as the velocity
        transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(_rb.velocity.x, _rb.velocity.y) * Mathf.Rad2Deg);

        _rb.velocity = transform.up * (_curSpeed);

        _curSpeed = Mathf.Lerp(_curSpeed, _goalSpeed, Time.fixedDeltaTime * _speedChangeRate);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_canDamage)
        {
            collision.transform.GetComponent<Damageable>()?.TakeDamage();
        }
        else
        {
            BecomeTangible();
        }
    }

    void BecomeTangible()
    {
        _canDamage = true;
        _sp.color = _baseColor;
    }

    public void OnOrderByHeightChanged(int order)
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = order;
    }
}
