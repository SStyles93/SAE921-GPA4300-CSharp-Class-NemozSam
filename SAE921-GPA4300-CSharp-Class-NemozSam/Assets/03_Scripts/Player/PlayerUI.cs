using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    int _lives;

    Color _playerColor;
    [SerializeField] List<Image> _colorableElements;
    [SerializeField] List<GameObject> _livesObjects;


    public void AssignLives(int lives)
    {
        //TODO Set the amount of image to the amount of lives
        _lives = lives;
    }

    public void AssignColor(Color color)
    {
        _playerColor = color;

        foreach (var colorable in _colorableElements)
        {
            colorable.color = _playerColor;
        }
    }

    public void LoseLife()
    {
        if (_livesObjects.Count == 0)
            return;

        //Disable the life
        _livesObjects[_livesObjects.Count - 1].SetActive(false);

        //Remove it frome the list
        _livesObjects.RemoveAt(_livesObjects.Count - 1);
    }
}
