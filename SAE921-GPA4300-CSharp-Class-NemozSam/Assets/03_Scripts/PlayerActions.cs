using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject _shootPoint;
    [SerializeField] GameObject _bullet;

    [SerializeField] GameObject _slowDownEffect;

    PlayerInput _input;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();

        _input.actions["Fire"].performed += TryShoot;
        _input.actions["Special"].performed += TrySpecial;
    }

    void TryShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void Shoot()
    {
        Instantiate(_bullet, _shootPoint.transform.position, _shootPoint.transform.rotation);
    }

    void TrySpecial(InputAction.CallbackContext context)
    {
        Special();
    }

    void Special()
    {
        Instantiate(_slowDownEffect, transform);
    }
}
