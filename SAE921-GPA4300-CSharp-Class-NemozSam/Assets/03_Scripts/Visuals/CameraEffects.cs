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
    [SerializeField] float _shakeCoeff = 0.2f;
    [SerializeField] float _shakeRotCoeff = 2.0f;
    [SerializeField] float _maxShakeRot = 15.0f;
    [SerializeField] float _maxShakeZoom = 2.0f;
    [SerializeField] float _shakeSpeed = 2.0f;
    Quaternion _shakeGoalRot;
    float _shakeGoalSize;
    bool _resetShakeGoals = true;

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
        //if (_shakeAmount > 0.0f)
        //{
        //    //randomly select a goal rotation and orthographic size based on _shakeAmount if we don't have one
        //    if(_resetShakeGoals)
        //    {
        //        _resetShakeGoals = false;

        //        //Find a goal rotation
        //        Vector3 rot = _baseRot.eulerAngles;
        //        _shakeGoalRot = Quaternion.Euler(rot.x, rot.y,
        //            rot.z + Mathf.Clamp(Random.Range(-_shakeAmount * _shakeRotCoeff, _shakeAmount * _shakeRotCoeff), -_maxShakeRot, _maxShakeRot));

        //        //Find a goal orthographic size

        //    }

        //    //Lerp to it

        //    //null the goal if we're close to them

        //    _shakeAmount -= Time.deltaTime;
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
