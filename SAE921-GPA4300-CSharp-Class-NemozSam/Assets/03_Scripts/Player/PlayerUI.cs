using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject _lifeObject;
    float _width;

    Color _playerColor;
    List<GameObject> _livesObjects = new List<GameObject>();

    public void AssignLives(int lives)
    {
        _width = _lifeObject.GetComponent<RectTransform>().rect.width;
        for (int life = 0; life < lives; life++)
        {
            //Create the life icon
            _livesObjects.Add(Instantiate(_lifeObject, _lifeObject.GetComponent<RectTransform>().position + 
                new Vector3(life * _width, 0,0), _lifeObject.transform.rotation, transform));

            //enable it
            _livesObjects[_livesObjects.Count - 1].SetActive(true);
        }
    }

    public void AssignColor(Color color)
    {
        _playerColor = color;

        _lifeObject.GetComponent<Image>().color = _playerColor;

        foreach (var colorable in _livesObjects)
        {
            colorable.GetComponent<Image>().color = _playerColor;
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
