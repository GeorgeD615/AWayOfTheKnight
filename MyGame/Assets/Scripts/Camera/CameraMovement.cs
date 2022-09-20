using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;

    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(_player.position.x, _player.position.y, transform.position.z), 0.5f);
    }
}
