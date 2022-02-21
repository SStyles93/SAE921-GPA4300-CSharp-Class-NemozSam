using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] GameObject _animatedPart;
    Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void UpdateVisuals(Vector2 movement)
    {
        if (movement == Vector2.zero)
        {
            _animator.speed = 0.0f;
        }
        else
        {
            _animator.speed = 1.0f;
            transform.rotation = Quaternion.Euler(0, movement.x > 0 ? 180 : 0, 0);
        }
    }
}
