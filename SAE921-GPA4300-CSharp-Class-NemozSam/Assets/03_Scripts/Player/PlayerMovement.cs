using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject rotationPoint;
    [SerializeField] float _speed = 2.0f;
    [SerializeField] float _maxSpeed = 5.0f;

    [SerializeField] float _turnSpeed = 2.0f;

    Rigidbody2D _rb;
    PlayerInput _input;

    PlayerVisuals _visuals;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();

        _visuals = GetComponentInChildren<PlayerVisuals>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        Vector2 movement = _input.actions["Movement"].ReadValue<Vector2>();
        _rb.velocity = Vector2.Lerp(_rb.velocity, movement * _maxSpeed, _speed * Time.fixedDeltaTime);

        //Update the visuals
        _visuals.UpdateVisuals(movement);

        //Look direction
        Vector2 look = _input.actions["Look"].ReadValue<Vector2>();
        if (look != Vector2.zero)
        {
            //Find the relative direction we want to look at
            Vector3 relativeDirection = rotationPoint.transform.InverseTransformDirection(new Vector3(look.x, look.y, 0.0f));

            //Calulate the angle of that look direction (and convert it to degrees)
            float angle = Mathf.Atan2(relativeDirection.x, relativeDirection.y) * Mathf.Rad2Deg;

            //Smoothly rotate to it at a defined speed
            rotationPoint.transform.Rotate(0, 0, Mathf.LerpAngle(rotationPoint.transform.rotation.z,
                rotationPoint.transform.rotation.z - angle, _turnSpeed * Time.fixedDeltaTime));
        }
    }
}