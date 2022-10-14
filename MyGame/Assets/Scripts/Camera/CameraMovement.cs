using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _currentPosition;
    [Range(0, 0.3f)] [SerializeField] private float _movementSmoothing = 0.3f;


    void Start()
    {

    }


    void Update()
    {
        if(_target != null)
        {
            Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y, -10);
            //Move(targetPosition);
        }
    }

    private void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentPosition, _movementSmoothing);
    }
}
