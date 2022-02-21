using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameLogic : MonoBehaviour
{
    [SerializeField] int _startLives = 5;
    int _lives;
    public int Lives { get { return _lives; } }

    Color _playerColor;
    [SerializeField] List<SpriteRenderer> _colorableElements;
    [SerializeField] PlayerManagerInterface _managerInterface;

    PlayerUI _lifeUI;

    private void Start()
    {
        _lives = _startLives;
    }

    public void LinkUI(PlayerUI playerUI)
    {
        _lifeUI = playerUI;

    }

    public void AssignColor(Color color)
    {
        _playerColor = color;
        
        foreach(var colorable in _colorableElements)
        {
            colorable.color = _playerColor;
        }

        _lifeUI?.AssignColor(_playerColor);
    }

    public void OnTakeDamage()
    {
        _lifeUI.LoseLife();

        _managerInterface.ReportDamage(this);
    }

    public void Die()
    {
        //TODO
    }
}
