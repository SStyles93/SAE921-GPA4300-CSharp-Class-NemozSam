using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownZone : MonoBehaviour
{
    [SerializeField] float _baseRadius = 4.0f;

    float _radius;
    float _mult = 1.0f;

    [SerializeField] float _growSpeed;

    public float RadiusMult
    {
        get { return _mult; }
        set { _mult = value; }
    }

    private void FixedUpdate()
    {
        _radius = Mathf.Lerp(_radius, _baseRadius * _mult, Time.fixedDeltaTime * _growSpeed);
        transform.localScale = Vector3.one * _radius;
    }

    void Break()
    {

    }
}
