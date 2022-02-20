using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    [Header("Player References")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<Transform> _spawnPositions;

    [Header("Player \"Tracking\"")]
    [SerializeField] private int _playerCount = 0;
    [SerializeField] private List<GameObject> _players;

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
    }
    private void Update()
    {
        if(_playerCount >= 4)
        {
            _playerInputManager.DisableJoining();
        }
    }

    /// <summary>
    /// Instanciates the player and places him at player's spawning point according to his index(_playerCount)
    /// </summary>
    /// <param name="playerInput"> Players Input given by the PlayerInputManager after an input was emitted</param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"Player {_playerCount + 1} has connected");
        _players.Add(playerInput.gameObject);
        playerInput.transform.position = _spawnPositions[_playerCount].position;
        playerInput.transform.rotation = _spawnPositions[_playerCount].rotation;
        _playerCount++;
    }

}
