using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    [SerializeField] int _health = 3;
    int _curHealth = 3;
    Sprite _baseSprite;
    [SerializeField] Sprite _dmgSprite;
    [SerializeField] Sprite _deadSprite;

    SpriteRenderer _sp;

    private void Start()
    {
        _curHealth = _health;
        _sp = GetComponent<SpriteRenderer>();
        _baseSprite = _sp.sprite;
    }

    public void OnTakeDamage()
    {
        StopAllCoroutines();


        if (--_curHealth == 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ShowDamage());
        }
    }

    void Die()
    {
        _sp.sprite = _deadSprite;
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator ShowDamage()
    {
        _sp.sprite = _dmgSprite;
        yield return new WaitForSeconds(0.3f);
        _sp.sprite = _baseSprite;
    }

    void OnReset()
    {
        _sp.sprite = _baseSprite;
        _curHealth = _health;
        GetComponent<Collider2D>().enabled = true;
    }
}
