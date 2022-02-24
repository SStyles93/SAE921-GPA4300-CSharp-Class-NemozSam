using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] float _zoomSpeed;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _zoomedSize;
    float _margin = 0.05f;

    float _baseZoom;
    Vector3 _basePos;
    Quaternion _baseRot;

    Camera _camera;

    [Header("Shake")]
    bool _canShake = true;
    private float _shakeAmount = 0.0f;
    [Tooltip("Use this value for the minimum shaking that can happen")]
    [SerializeField] float _minShake = 0.7f;
    [Tooltip("How quickly the camera size changes scale with shake amount")]
    [SerializeField] float _shakeCoeff = 0.2f;
    [Tooltip("How quickly the camera rotation changes scale with shake amount")]
    [SerializeField] float _shakeRotCoeff = 2.0f;
    [Tooltip("Maximum rotation the shaking can reach")]
    [SerializeField] float _maxShakeRot = 15.0f;
    [Tooltip("Maximum difference with base zoom the shaking can reach")]
    [SerializeField] float _maxShakeZoom = 2.0f;
    [Tooltip("How quickly the camera moves between different rotations and sizes")]
    [SerializeField] float _shakeSpeed = 2.0f;
    Quaternion _shakeGoalRot;
    float _shakeGoalSize;
    bool _resetShakeGoals = true;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _baseZoom = _shakeGoalSize = _camera.orthographicSize;
        _basePos = transform.position;
        _baseRot = _shakeGoalRot = transform.rotation;
    }

    /// <summary>
    /// Add time and power (same value) to the current shaking
    /// Start shaking if it wasn't
    /// </summary>
    /// <param name="shake">The amount in seconds to shake for</param>
    public void AddShake(float shake)
    {
        _shakeAmount += shake;
    }

    /// <summary>
    /// Force to shake for a a given time. Overrides current shaking.
    /// </summary>
    /// <param name="shake">The amount in seconds to shake for</param>
    public void ReplaceShake(float shake)
    {
        _shakeAmount = shake;
    }

    private void Update()
    {
        if (_shakeAmount > 0.0f && _canShake)
        {
            //randomly select a goal rotation and orthographic size based on _shakeAmount if we don't have one
            if (_resetShakeGoals)
            {
                _resetShakeGoals = false;
                float shake = Mathf.Max(_minShake, _shakeAmount);
                //Find a goal rotation
                Vector3 rot = _baseRot.eulerAngles;
                _shakeGoalRot = Quaternion.Euler(rot.x, rot.y,
                    rot.z + Mathf.Clamp(Random.Range(-shake * _shakeRotCoeff, shake * _shakeRotCoeff), -_maxShakeRot, _maxShakeRot));

                //Find a goal orthographic size
                _shakeGoalSize = _baseZoom + Mathf.Clamp(Random.Range(-shake * _shakeCoeff, shake * _shakeCoeff), -_maxShakeZoom, _maxShakeZoom);
            }
            _shakeAmount -= Time.deltaTime;
        }
        else
        {
            //Reset shake
            _shakeAmount = 0.0f;
            
            //Go to a normal rotation
            _shakeGoalRot = _baseRot;

            //Reset the size to the normal amount only if we're alowed to normally shake
            //(if not, we're zooming and we don't want to modify the size)
            if (_canShake)
            {
                _shakeGoalSize = _baseZoom;
            }
        }

        //lerp to goals
        if (_canShake)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _shakeGoalSize, _shakeSpeed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation , _shakeGoalRot , _shakeSpeed * Time.deltaTime);

        //Set a bool to reset them if we're close enough
        if (Mathf.Abs(_camera.orthographicSize - _shakeGoalSize) < _margin ||
            Mathf.Abs(transform.rotation.eulerAngles.z - _shakeGoalRot.eulerAngles.y) < _margin)
            _resetShakeGoals = true;
    }

    public IEnumerator ResetCamera()
    {
        yield return ZoomOnTarget(_basePos, false);
    }

    public IEnumerator ZoomOnTarget(Vector3 target, bool zoomIn = true, float speedMult = 1.0f)
    {
        _canShake = false;
        float zoomTo = zoomIn ? _zoomedSize : _baseZoom;

        do
        {
            //Move to position
            transform.position = Vector3.Lerp(transform.position, target, Time.unscaledDeltaTime * _moveSpeed * speedMult);
            transform.position = new Vector3(transform.position.x, transform.position.y, _basePos.z);

            //Zoom in
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoomTo,
                Time.unscaledDeltaTime * _zoomSpeed * speedMult);

            yield return null;
        } while (new Vector2(transform.position.x - target.x,
        transform.position.y - target.y).sqrMagnitude > _margin ||
        Mathf.Abs(_camera.orthographicSize - zoomTo) > _margin);

        //Snap the camera to the correct values
        transform.position = new Vector3(target.x, target.y, transform.position.z);
        _camera.orthographicSize = zoomTo;

        _canShake = true;
    }
}
