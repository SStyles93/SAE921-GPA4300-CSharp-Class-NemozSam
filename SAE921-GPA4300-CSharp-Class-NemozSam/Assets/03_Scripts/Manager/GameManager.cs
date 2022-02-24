using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    [Header("Player References")]
    [Tooltip("The prefab you want to use as \"The Player\"")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] List<Color> _playerColors = new List<Color>(4);
    [Tooltip("The players spawn positions")]
    [SerializeField] private List<Transform> _spawnPositions;
    [Tooltip("The prefab you want to use as \"The UI\" ")]
    [SerializeField] private GameObject _UIPrefab;
    [Tooltip("The players UI Positions on the screen")]
    [SerializeField] private List<GameObject> _UIPositions;
    [Tooltip("The UICanvas's startTimer")]
    [SerializeField] private GameObject _startTimer;
    [Tooltip("Time before starting the game once all players are ready")]
    [SerializeField] private float _startTime = 4.0f;
    private bool _gameIsReady = false;
    private bool _countDownIsStarted = false;
    private bool _gameStarted;

    [Header("Player \"Tracking\"")]
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private List<GameObject> _readyPlayers;
    [Tooltip("Has to be set between 2 and 4 normally. Can be set to 1 for debug purpose.")]
    [SerializeField] private int nbPlayersToPlay = 2;
    [SerializeField] private List<GameObject> _potentialWinners = new List<GameObject>();
    [Tooltip("Give the same to the players. The manager will know from this when a player is hit")]
    [SerializeField] private PlayerManagerInterface _playerInterface;

    [SerializeField] AudioSource _musicSource;

    private void Awake()
    {
        _countDownIsStarted = false;

        //Link to the playerInputManager
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;

        _playerInterface.AddDamageReportAction(OnPlayerTakeDamage);
    }
    private void Update()
    {
        if (!_gameStarted)
            CheckIfReadyToStart();
    }

    /// <summary>
    /// Instanciates the player and places him at player's spawning point according to his index(_playerCount)
    /// </summary>
    /// <param name="playerInput"> Players Input given by the PlayerInputManager after an input was emitted</param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (_countDownIsStarted)
        {
            Destroy(playerInput.gameObject);
            return;
        }

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
        _players[_players.Count - 1].GetComponent<PlayerGameLogic>().AssignColor(_playerColors[_players.Count - 1]);

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
                SetReadyText(player, true);
            }
        }
        if (_readyPlayers.Count == _players.Count && _readyPlayers.Count >= nbPlayersToPlay && !_countDownIsStarted)
        {
            StartCoroutine(StartCountdown(_startTime));
            _countDownIsStarted = true;
        }
        if (_gameIsReady)
        {
            foreach (var player in _players)
            {
                //Disable ReadyText
                SetReadyText(player, false);

                //Unblock player
                player.GetComponent<PlayerGameLogic>().BlockPlayer(false);
            }

            _gameStarted = true;
        }
    }

    /// <summary>
    /// Enables or Disables ReadyText for the player
    /// </summary>
    /// <param name="player">The player with whom we want to interact</param>
    /// <param name="state">The state in witch we want to set the ReadyText</param>
    private void SetReadyText(GameObject player, bool state)
    {
        PlayerUI playerUI =
                _UIPositions[_players.IndexOf(player)].
                    GetComponentInChildren<PlayerUI>();
        if(state == true)
            playerUI.InstantiateReadyText();
        playerUI.EnableOrDisableReadyText(state);
    }

    /// <summary>
    /// Coroutine used to countdown time and prepare for game + UI(3,2,1,START!)
    /// </summary>
    /// <param name="time">Time before game start</param>
    /// <returns></returns>
    private IEnumerator StartCountdown(float time)
    {
        //Play music
        _musicSource.Play();

        _startTimer.gameObject.SetActive(true);
        //timer check is > 1.0f so that the "Start !" comes after the 1 and not the 0
        while (time > 1.0f)
        {
            time -= Time.unscaledDeltaTime;
            _startTimer.GetComponent<Text>().text = ((int)time).ToString();
            yield return null;        
        }
        _startTimer.GetComponent<Text>().text = "Start !";
        yield return new WaitForSecondsRealtime(1.0f);
        _startTimer.gameObject.SetActive(false);
        _gameIsReady = true;
    }

    private void Win() 
    {
        SceneManager.LoadSceneAsync(0);
    }

    /// <summary>
    /// Method to check for the winning player
    /// </summary>
    /// <param name="player">player to check</param>
    private void OnPlayerTakeDamage(PlayerGameLogic player)
    {
        if (_potentialWinners.Count > 1)
            _potentialWinners.Remove(player.gameObject);

        //Win
        if (_potentialWinners.Count == 1)
        {
            GameObject winner = _potentialWinners[0];

            //Reset the potential winners
            _potentialWinners = new List<GameObject>(_players);

            //Clear out the dead ones
            List<int> indexesToRemove = new List<int>();
            for (int pIndex = 0; pIndex < _potentialWinners.Count; pIndex++)
            {
                if(_potentialWinners[pIndex].GetComponent<PlayerGameLogic>().Lives <= 0)
                {
                    //Add them to the list of index to remove
                    indexesToRemove.Add(pIndex);
                }
            }
            //Remove the found index
            for (int removeI = indexesToRemove.Count - 1; removeI >= 0; removeI--)
            {
                _potentialWinners.RemoveAt(removeI);
            }


            if(_potentialWinners.Count == 1)
            {
                GetComponent<RoundManager>().Win(winner, Win);
            }
            else
            {
                GetComponent<RoundManager>()?.NewRound(_spawnPositions, _players, winner);
            }
        }
    }
}
