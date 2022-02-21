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

    Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _baseZoom = _camera.orthographicSize;
        _basePos = transform.position;
    }

    public IEnumerator ResetCamera()
    {
        yield return ZoomOnTarget(_basePos, false);
    }

    public IEnumerator ZoomOnTarget(Vector3 target, bool zoomIn = true)
    {
        float zoomTo = zoomIn ? _zoomedSize : _baseZoom;

        do
        {
            //Move to position
            transform.position = Vector3.Lerp(transform.position, target, Time.unscaledDeltaTime * _moveSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, _basePos.z);

            //Zoom in
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoomTo, Time.unscaledDeltaTime * _zoomSpeed);
            yield return null;
        } while (new Vector2(transform.position.x - target.x,
        transform.position.y - target.y).sqrMagnitude > _margin ||
        Mathf.Abs(_camera.orthographicSize - zoomTo) > _margin);

        //Snap the camera to the correct values
        transform.position = new Vector3(target.x, target.y, transform.position.z);
        _camera.orthographicSize = zoomTo;
    }
}
