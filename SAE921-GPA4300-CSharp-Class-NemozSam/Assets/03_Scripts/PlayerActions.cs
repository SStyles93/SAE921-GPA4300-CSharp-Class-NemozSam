using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    //Time in seconds the power can be held
    [SerializeField] float _powerMaxCharge = 5.0f;
    [SerializeField] float _powerChargeRate = 2.0f;
    float _charge = 0.0f;

    [SerializeField] GameObject _shootPoint;
    [SerializeField] GameObject _bullet;

    [SerializeField] GameObject _slowDownEffect;
    GameObject _slowDownInstance = null;

    PlayerInput _input;

    private void Start()
    {
        _charge = _powerMaxCharge;

        _input = GetComponent<PlayerInput>();

        _input.actions["Fire"].performed += TryShoot;

        _input.actions["Special"].performed += TrySpecial;
        _input.actions["Special"].canceled += TryStopSpecial;
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

    void TryStopSpecial(InputAction.CallbackContext context)
    {
        StopSpecial();
    }

    void Special()
    {
        StopAllCoroutines();
        StartCoroutine(LoseSpecial());
        _slowDownInstance = Instantiate(_slowDownEffect, transform);
        StartCoroutine(UpdateSpecialInstance());
    }

    void StopSpecial()
    {
        StopAllCoroutines();
        StartCoroutine(RechargeSpecial());
        Destroy(_slowDownInstance);
    }

    IEnumerator LoseSpecial()
    {
        while(_charge > 0)
        {
            yield return null;
            _charge -= Time.deltaTime;
        }

        yield break;
    }

    IEnumerator RechargeSpecial()
    {
        while(_charge < _powerMaxCharge)
        {
            yield return null;
            _charge += Time.deltaTime * _powerChargeRate;
        }

        _charge = _powerMaxCharge;
        yield break;
    }

    IEnumerator UpdateSpecialInstance()
    {
        while(_slowDownInstance)
        {
            _slowDownInstance.GetComponent<SlowDownZone>().RadiusMult = _charge / _powerMaxCharge;
            yield return null;
        }

        yield break;
    }
}
