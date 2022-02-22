using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameLogic : MonoBehaviour
{
    [SerializeField] int _lives  = 5;

    public int Lives { get { return _lives; } }


    Color _playerColor;
    [SerializeField] List<SpriteRenderer> _colorableElements;
    [SerializeField] PlayerManagerInterface _managerInterface;

    PlayerUI _lifeUI;

    public Color PlayerColor { get => _playerColor; set => _playerColor = value; }

    public void LinkUI(PlayerUI playerUI)
    {
        _lifeUI = playerUI;
        _lifeUI.AssignLives(_lives);
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
        DisableOrEnablePlayer(false);
        GetComponent<EffectsSpawner>().SpawnEffect("Blood", false);

        _lifeUI.LoseLife();
        _lives--;

        _managerInterface.ReportDamage(this);
    }

    public void Die()
    {
        DisableOrEnablePlayer(false);

        GetComponent<EffectsSpawner>().SpawnEffect("Tomb", false);
    }

    /// <summary>
    /// Enable or disable the player's ability to move, act and be seen without disabling the playerInputs.
    /// </summary>
    /// <param name="enable">wether to enable or disable the player</param>
    void DisableOrEnablePlayer(bool enable)
    {
        GetComponent<PlayerActions>().CanShoot = enable;
        GetComponent<PlayerActions>().CanSpecial = enable;

        GetComponent<PlayerMovement>().CanMove = enable;

        GetComponent<Collider2D>().enabled = enable;
        GetComponent<Rigidbody2D>().simulated = enable;

        foreach(var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.enabled = enable;
        }
    }

    void GhostMode()
    {
        GetComponent<PlayerActions>().CanShoot = true;

        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.enabled = true;
        }
        GetComponentInChildren<PlayerVisuals>().BecomeGhost();
    }

    public void Spawn(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        if (_lives == 0)
        {
            GhostMode();
        }
        else
        {
            DisableOrEnablePlayer(true);
        }

        //TODO add particles that show the player just spawned in and/or sound cues

        GetComponent<PlayerActions>().ResetActions();
    }
}
