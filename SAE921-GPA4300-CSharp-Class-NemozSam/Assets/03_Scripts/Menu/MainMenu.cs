using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] float _transitionSpeed = 10.0f;
    bool _transition = false;
    RectTransform _rectTransform;

    float _goal;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _goal = _rectTransform.position.y * 3.0f;
        _transitionSpeed *= Screen.height;
        _transition = false;
    }

    public void PlayGame()
   {
        if (_transition)
            return;

        StartCoroutine(TransitionToNextScene());
   }

    IEnumerator TransitionToNextScene()
    {
        _transition = true;

        do
        {
            _rectTransform.position += new Vector3(0 ,Time.deltaTime * _transitionSpeed, 0);
            yield return null;

        } while (_rectTransform.position.y < _goal);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void QuitGame()
    {
        if (_transition)
            return;

        Application.Quit();
    }
}
 