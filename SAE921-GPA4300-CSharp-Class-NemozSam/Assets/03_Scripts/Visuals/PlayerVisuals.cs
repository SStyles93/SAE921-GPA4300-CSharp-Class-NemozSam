using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] GameObject _body;
    [SerializeField] GameObject _colorable;
    [SerializeField] GameObject _ghost;

    Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Sets the players animator speed to the given movement speed
    /// </summary>
    /// <param name="movement">movement vector used to set speed of animatior</param>
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

    /// <summary>
    /// Method used to give the player a "Ghost" visual
    /// </summary>
    public void BecomeGhost()
    {
        _body.SetActive(false);
        _colorable.SetActive(false);
        _ghost.SetActive(true);
    }
}
