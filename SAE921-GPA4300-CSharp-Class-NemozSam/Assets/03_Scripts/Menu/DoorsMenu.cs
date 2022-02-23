using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsMenu : MonoBehaviour
{
    [SerializeField] float _speed;

    public void OpenDoors()
    {
        StartCoroutine(SlideDoors());
        GetComponent<AudioSource>().Play();
    }

    IEnumerator SlideDoors()
    {
        do
        {
            foreach (Transform child in transform)
            {
                child.position += Mathf.Sign(child.position.x) * Vector3.right * Time.deltaTime * _speed;
            }

            yield return null;
        } while (GetComponentsInChildren<SpriteRenderer>()[0].isVisible) ;
    }
}
