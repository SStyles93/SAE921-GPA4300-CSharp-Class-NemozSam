using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float _timeToDestroy = 1.0f;
    [SerializeField] GameObject _destroyEffect = null;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime(_timeToDestroy));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);

        if (_destroyEffect)
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity, transform.parent);
        }
    }
}
