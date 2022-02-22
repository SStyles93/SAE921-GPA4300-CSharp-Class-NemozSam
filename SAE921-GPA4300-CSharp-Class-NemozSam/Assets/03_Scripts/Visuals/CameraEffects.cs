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

    private float _shakeAmount = 1.0f;
    [SerializeField] float _shakeCoeff = 0.5f;
    [SerializeField] float _shakeSpeed = 2.0f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _baseZoom = _camera.orthographicSize;
        _basePos = transform.position;
        _baseRot = transform.rotation;
    }

    public void AddShake(float shake)
    {
        _shakeAmount += shake;
    }

    private void Update()
    {
        //TODO Make this work
        //if (_shakeAmount > 0.0f)
        //{
        //    float direction = 1.0f;
        //    float goal = _baseZoom + _shakeAmount + _shakeCoeff * Mathf.Sin(Time.time * _shakeSpeed);
        //    _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, goal, 0.5f);

        //    _shakeAmount -= Time.unscaledDeltaTime;
        //}
        //else
        //{
        //    _shakeAmount = 0.0f;
        //}
    }

    public IEnumerator ResetCamera()
    {
        yield return ZoomOnTarget(_basePos, false);
    }

    public IEnumerator ZoomOnTarget(Vector3 target, bool zoomIn = true, float speedMult = 1.0f)
    {
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
    }
}
