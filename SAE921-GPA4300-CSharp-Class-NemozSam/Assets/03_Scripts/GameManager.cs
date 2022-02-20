using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    [Header("Player References")]
    [Tooltip("The prefab you want to use as \"The Player\"")]
    [SerializeField] private GameObject _playerPrefab;
    [Tooltip("The players spawn positions")]
    [SerializeField] private List<Transform> _spawnPositions;
    [Tooltip("The prefab you want to use as \"The UI\" ")]
    [SerializeField] private GameObject _UIPrefab;
    [Tooltip("The players UI Positions on the screen")]
    [SerializeField] private List<GameObject> _UIPositions;

    [Header("Player \"Tracking\"")]
    [SerializeField] private int _playerCount = 0;
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private List<GameObject> _playersUi;

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
        _playersUi.Add(Instantiate(_UIPrefab,
            _UIPositions[_playerCount].transform.position,
            _UIPositions[_playerCount].transform.rotation,
            _UIPositions[_playerCount].transform));
        _playerCount++;
    }

}
