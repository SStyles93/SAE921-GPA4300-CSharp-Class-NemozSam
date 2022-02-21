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
    [Tooltip("Give the same to the players, the manager will know from this when a player is hit")]
    [SerializeField] private PlayerManagerInterface _playerInterface;

    private void Awake()
    {
        //Link to the playerInputManager
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;

        _playerInterface.AddDamageReportAction(OnPlayerTakeDamage);
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
        playerInput.transform.position = _spawnPositions[_players.Count -1].position;
        playerInput.transform.rotation = _spawnPositions[_players.Count -1].rotation;

        //Create the ui
        //_playersUi.Add(Instantiate(_UIPrefab,
        //    _UIPositions[_players.Count -1].transform.position,
        //    _UIPositions[_players.Count -1].transform.rotation,
        //    _UIPositions[_players.Count -1].transform));

        _players[_players.Count - 1].GetComponent<PlayerGameLogic>().LinkUI(Instantiate(_UIPrefab,
            _UIPositions[_players.Count -1].transform.position,
            _UIPositions[_players.Count -1].transform.rotation,
            _UIPositions[_players.Count-1].transform).GetComponent<PlayerUI>());

        //Assign a color to the player
        _players[_players.Count - 1].GetComponent<PlayerGameLogic>().AssignColor(Random.ColorHSV());

        //Disables player joining after 4 players join
        if (_players.Count >= 4)
        {
            _playerInputManager.DisableJoining();
        }
    }

    void OnPlayerTakeDamage(PlayerGameLogic player)
    {
        //TODO, check for gameOvers
        SetupNewRound();
    }

    void SetupNewRound()
    {
        //Reset each player's spawn
        for(int i = 0 ; i < _players.Count ; i++)
        {
            _players[i].transform.position = _spawnPositions[i].position;
            _players[i].transform.rotation = _spawnPositions[i].rotation;
        }

        //Delete all bullets
        foreach (var bullet in FindObjectsOfType<Bullet>())
        {
            Destroy(bullet.gameObject);
        }
    }
}
