using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float _timeToDestroy = 1.0f;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime(_timeToDestroy));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);
    }
}
