using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameLogic : MonoBehaviour
{
    [SerializeField] int _lives  = 5;
    [SerializeField] private bool _isReady = false;

    Color _playerColor;
    [SerializeField] List<SpriteRenderer> _colorableElements;
    [SerializeField] PlayerManagerInterface _managerInterface;

    PlayerUI _lifeUI;

    #region Properties
    public int Lives { get { return _lives; } }
    public Color PlayerColor { get => _playerColor; set => _playerColor = value; }
    public bool IsReady { get => _isReady; set => _isReady = value; }
    #endregion

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
        //Disable the player
        DisableOrEnablePlayer(false);

        //Spawn the effect
        if(--_lives == 0)
        {
            GetComponent<EffectsSpawner>().SpawnEffect("Tomb", false);
        }
        else
        {
            GetComponent<EffectsSpawner>().SpawnEffect("Blood", false);
        }

        //Update the ui
        _lifeUI.LoseLife();

        //Report the damage
        _managerInterface.ReportDamage(this);
    }

    /// <summary>
    /// Enable or disable the player's ability to move, act and be seen without disabling the playerInputs.
    /// </summary>
    /// <param name="enable">wether to enable or disable the player</param>
    public void DisableOrEnablePlayer(bool enable)
    {
        GetComponent<PlayerActions>().CanShoot = enable;

        GetComponent<PlayerActions>().CanSpecial = enable;

        GetComponent<PlayerMovement>().CanMove = enable;

        GetComponent<Collider2D>().enabled = enable;
        GetComponent<Rigidbody2D>().simulated = enable;

        //Disable the player's visuals
        foreach(var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.enabled = enable;
        }
    }

    /// <summary>
    /// Enable the player without alowing him to interact
    /// </summary>
    /// <param name="block">wether to block the player or not. Set to true the player will be blocked</param>
    public void BlockPlayer(bool block)
    {
        //Access PlayerActions
        GetComponent<PlayerActions>().CanShoot = !block;
        GetComponent<PlayerActions>().CanSpecial = !block;
        //Access PlayerMovement
        GetComponent<PlayerMovement>().CanMove = !block;
        //Access Player Animations
        GetComponentInChildren<PlayerVisuals>().GetComponentInChildren<Animator>().enabled = !block;
    }

    /// <summary>
    /// GhostMode is a mode where the player can only shoot.
    /// </summary>
    void GhostMode()
    {
        GetComponent<PlayerActions>().CanShoot = true;

        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.enabled = true;
        }
        GetComponentInChildren<PlayerVisuals>().BecomeGhost();
    }

    /// <summary>
    /// Spawns the player at the defined spawnPoint
    /// </summary>
    /// <param name="spawnPoint">Transform used to spawn the player</param>
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
