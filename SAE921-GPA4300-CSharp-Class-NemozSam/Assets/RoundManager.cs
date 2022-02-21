using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public void NewRound(List<Transform> spawnPositions, List<GameObject> players)
    {
        StartCoroutine(ResetRound(spawnPositions, players));
    }

    IEnumerator ResetRound(List<Transform> spawnPositions, List<GameObject> players)
    {
        Time.timeScale = 0.0f;
        yield return ClearLastRound();

        if (Camera.main.GetComponent<CameraEffects>())
            yield return Camera.main.GetComponent<CameraEffects>().ZoomOnTarget(players[0].transform.position);

        yield return Celebration();

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
        //Delete remaining bullets that player might have shot in-between
        foreach (var bullet in FindObjectsOfType<Bullet>())
        {
            Destroy(bullet.gameObject);
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

        //Reset each player's spawn
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerGameLogic>().Spawn(spawnPositions[i]);
        }

        foreach (var cactus in FindObjectsOfType<Cactus>())
        {
            if (cactus.Regrow())
                yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
