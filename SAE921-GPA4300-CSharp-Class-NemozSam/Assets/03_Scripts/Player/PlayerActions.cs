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

    [SerializeField] float _gunReloadTime = 3.0f;
    bool _loaded = true;

    [SerializeField] GameObject _shootPoint;
    [SerializeField] GameObject _bullet;

    [SerializeField] GameObject _slowDownEffect;
    GameObject _slowDownInstance = null;

    PlayerInput _input;
    PlayerGameLogic _gameLogic;

    bool _canShoot = true;
    bool _canSpecial = true;
    public bool CanShoot { get => _canShoot; set => _canShoot = value; }
    public bool CanSpecial { get => _canSpecial; set => _canSpecial = value; }

    private void Start()
    {
        _charge = _powerMaxCharge;

        _input = GetComponent<PlayerInput>();
        _gameLogic = GetComponent<PlayerGameLogic>();

        _input.actions["Fire"].performed += TryShoot;
        _input.actions["Fire"].performed += TrySetReady;

        _input.actions["Special"].performed += TrySpecial;
        _input.actions["Special"].canceled += TryStopSpecial;
    }

    private void Update()
    {
        //Change charge
        if(_slowDownInstance)
        {
            _charge -= Time.deltaTime;
            if (_charge <= 0.0f)
            {
                _charge = 0.0f;
                StopSpecial();
            }
        }
        else
        {
            _charge += Time.deltaTime * _powerChargeRate;
            if (_charge > _powerMaxCharge)
                _charge = _powerMaxCharge;
        }
    }

    void TrySetReady(InputAction.CallbackContext context)
    {
        if (!_gameLogic.IsReady)
        {
            SetReady();
        }
    }
    void SetReady()
    {
        _gameLogic.IsReady = true;
    }

    void TryShoot(InputAction.CallbackContext context)
    {
        if (_loaded && _canShoot)
            Shoot();
    }
    void Shoot()
    {
        //Create the bullet
        Instantiate(_bullet, _shootPoint.transform.position, _shootPoint.transform.rotation);

        //Handle the gun logic
        _loaded = false;
        StartCoroutine(Reload(_gunReloadTime));
    }

    void TrySpecial(InputAction.CallbackContext context)
    {
        if (!_slowDownInstance && _canSpecial)
            Special();
    }
    void Special()
    {
        _slowDownInstance = Instantiate(_slowDownEffect, transform);
        Color slowDownColor = _slowDownInstance.GetComponent<SpriteRenderer>().color;
        slowDownColor.r = _gameLogic.PlayerColor.r;
        slowDownColor.g = _gameLogic.PlayerColor.g;
        slowDownColor.b = _gameLogic.PlayerColor.b;
        _slowDownInstance.GetComponent<SpriteRenderer>().color = slowDownColor;
        StartCoroutine(UpdateSpecialInstance());
    }

    void TryStopSpecial(InputAction.CallbackContext context)
    {
        if (_slowDownInstance)
            StopSpecial();
    }
    void StopSpecial()
    {
        Destroy(_slowDownInstance);
    }

    //Change the radius of the slowDown zone each frame to correspond to the charge of our special bar
    IEnumerator UpdateSpecialInstance()
    {
        while(_slowDownInstance)
        {
            _slowDownInstance.GetComponent<SlowDownZone>().RadiusMult = _charge / _powerMaxCharge;
            yield return null;
        }

        yield break;
    }

    //Reload the gun after a set amount of time
    IEnumerator Reload(float time)
    {
        yield return new WaitForSeconds(time);

        //Play reload sound
        GetComponent<EffectsSpawner>().SpawnEffect("ReloadFlash", _shootPoint.transform);

        _loaded = true;
    }

    public void ResetActions()
    {
        //Stop all current processes
        StopAllCoroutines();

        //Destroy our special effects
        if (_slowDownInstance)
            Destroy(_slowDownInstance);

        //Reset our variables
        _loaded = true;
        _charge = _powerMaxCharge;
    }
}
