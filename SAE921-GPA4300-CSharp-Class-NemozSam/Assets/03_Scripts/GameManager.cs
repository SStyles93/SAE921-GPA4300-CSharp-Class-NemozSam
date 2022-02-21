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
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private List<GameObject> _playersUi;

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    /// <summary>
    /// Instanciates the player and places him at player's spawning point according to his index(_playerCount)
    /// </summary>
    /// <param name="playerInput"> Players Input given by the PlayerInputManager after an input was emitted</param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"Player {_players.Count} has connected");

        //Add player into our list of players
        _players.Add(playerInput.gameObject);

        //Set the player to his spawn point
        playerInput.transform.position = _spawnPositions[_players.Count].position;
        playerInput.transform.rotation = _spawnPositions[_players.Count].rotation;
        
        //Create the ui
        _playersUi.Add(Instantiate(_UIPrefab,
            _UIPositions[_players.Count].transform.position,
            _UIPositions[_players.Count].transform.rotation,
            _UIPositions[_players.Count].transform));

        //Disables player joining after 4 players join
        if (_players.Count >= 4)
        {
            _playerInputManager.DisableJoining();
        }
    }
}
