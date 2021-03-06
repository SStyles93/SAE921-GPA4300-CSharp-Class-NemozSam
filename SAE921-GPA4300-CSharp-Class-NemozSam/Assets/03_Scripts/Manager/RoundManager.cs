using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _effectsSource;
    [SerializeField] AudioClip _roundWinSound;
    [SerializeField] AudioClip _gameWinSound;

    public void NewRound(List<Transform> spawnPositions, List<GameObject> players, GameObject winner)
    {
        StartCoroutine(ResetRound(spawnPositions, players, winner));
    }

    public void Win(GameObject winner, UnityAction winAction)
    {
        StartCoroutine(CelebrateWinner(winner, winAction));
    }

    IEnumerator CelebrateWinner(GameObject winner, UnityAction action)
    {
        Time.timeScale = 0.0f;
        yield return ClearLastRound();

        _effectsSource.PlayOneShot(_gameWinSound);

        if (Camera.main.GetComponent<CameraEffects>())
            yield return Camera.main.GetComponent<CameraEffects>().ZoomOnTarget(winner.transform.position, speedMult:0.5f);

        yield return new WaitForSecondsRealtime(3.0f);

        Time.timeScale = 1.0f;
        action.Invoke();
    }

    IEnumerator ResetRound(List<Transform> spawnPositions, List<GameObject> players, GameObject winner)
    {
        Time.timeScale = 0.0f;
        yield return ClearLastRound();

        _effectsSource.PlayOneShot(_roundWinSound);
        if (Camera.main.GetComponent<CameraEffects>())
            yield return Camera.main.GetComponent<CameraEffects>().ZoomOnTarget(winner.transform.position);

        yield return Celebration();
        _effectsSource.Stop();

        yield return SetupNewRound(spawnPositions, players);

        Time.timeScale = 1.0f;
    }

    IEnumerator ClearLastRound()
    {
        //Delete all bullets
        foreach (var bullet in FindObjectsOfType<Bullet>())
        {
            bullet.DestroyBullet();

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator Celebration()
    {
        yield return new WaitForSecondsRealtime(1.0f);
    }

    IEnumerator SetupNewRound(List<Transform> spawnPositions, List<GameObject> players)
    {
        if (Camera.main.GetComponent<CameraEffects>())
            yield return Camera.main.GetComponent<CameraEffects>().ResetCamera();

        //Disable all players while the round is being set up again
        foreach (var player in players)
            player.GetComponent<PlayerGameLogic>().DisableOrEnablePlayer(false);

        //Delete remaining bullets that player might have shot in-between
        foreach (var bullet in FindObjectsOfType<Bullet>())
        {
            Destroy(bullet.gameObject);
        }

        foreach (var cactus in FindObjectsOfType<Cactus>())
        {
            if (cactus.Regrow())
                yield return new WaitForSecondsRealtime(0.1f);
        }

        //Reset each player's spawn
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerGameLogic>().Spawn(spawnPositions[i]);
        }
    }
}
