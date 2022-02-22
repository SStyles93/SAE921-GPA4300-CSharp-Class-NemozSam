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
    [SerializeField] private List<GameObject> _readyPlayers;
    [SerializeField] private List<GameObject> _potentialWinners = new List<GameObject>();
    [Tooltip("Give the same to the players. The manager will know from this when a player is hit")]
    [SerializeField] private PlayerManagerInterface _playerInterface;

    private void Awake()
    {
        //Link to the playerInputManager
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;

        _playerInterface.AddDamageReportAction(OnPlayerTakeDamage);
    }
    public void Update()
    {
        CheckIfReadyToStart();
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
        _potentialWinners.Add(playerInput.gameObject);

        //Set the player to his spawn point
        playerInput.GetComponent<PlayerGameLogic>().Spawn(_spawnPositions[_players.Count - 1]);

        //Create the ui
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

    /// <summary>
    /// Controls if all the conditions are fulfilled before enabling the players interactions
    /// </summary>
    private void CheckIfReadyToStart()
    {
        foreach (var player in _players)
        {
            if (!player.GetComponent<PlayerGameLogic>().IsReady)
                player.GetComponent<PlayerGameLogic>().BlockPlayer(true);
            else if (!_readyPlayers.Contains(player))
            {
                //adds ready players to a list (to compare with instantiated player)
                _readyPlayers.Add(player);
                //Instantiate & activate ReadyText
                PlayerUI playerUI =
                _UIPositions[_readyPlayers.IndexOf(player)].
                    GetComponentInChildren<PlayerUI>();
                playerUI.InstantiateReadyText();
                playerUI.EnableOrDisableReadyText(true);
            }
        }
        if (_readyPlayers.Count == _players.Count)
        {
            foreach (var player in _players)
            {
                //TODO: Set a timer Before activating the following methods

                //Unblock player
                player.GetComponent<PlayerGameLogic>().BlockPlayer(false);
                //Disable ReadyText
                PlayerUI playerUI =
               _UIPositions[_readyPlayers.IndexOf(player)].
                   GetComponentInChildren<PlayerUI>();
                playerUI.EnableOrDisableReadyText(false);
            }
        }
    }

    /// <summary>
    /// Method to check for the winning player
    /// </summary>
    /// <param name="player">player to check</param>
    void OnPlayerTakeDamage(PlayerGameLogic player)
    {
        //TODO, check for gameOvers

        if (_potentialWinners.Count > 1)
            _potentialWinners.Remove(player.gameObject);

        //Win
        if (_potentialWinners.Count == 1)
        {
            GetComponent<RoundManager>()?.NewRound(_spawnPositions, _players, _potentialWinners[0]);

            //Reset the potential winners
            _potentialWinners = new List<GameObject>(_players);

            //Clear out the dead ones
            for (int pIndex = 0; pIndex < _potentialWinners.Count; pIndex++)
            {
                if(_potentialWinners[pIndex].GetComponent<PlayerGameLogic>().Lives == 0)
                {
                    _potentialWinners.RemoveAt(pIndex);
                }
            }
        }
    }
}