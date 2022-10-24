using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    public bool _isActive = false;
    Vector2 _startPosition;
    Vector2 _activePosition;
    float _activeSpeed = 20f;
    float _inActiveSpeed = 2f;
    float delayTime = 1f;
    float timeToActive;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _activePosition = new Vector2(_startPosition.x, _startPosition.y + 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive )
        {
            if (Time.time > timeToActive)
            {
                transform.position = Vector2.MoveTowards(transform.position, _startPosition, _inActiveSpeed * Time.deltaTime);
                if (transform.position.y == _startPosition.y)
                {
                    _isActive = false;
                    timeToActive = Time.time + delayTime;
                }
            }
        }
        else
        {
            if(Time.time > timeToActive)
            {
                transform.position = Vector2.MoveTowards(transform.position, _activePosition, _activeSpeed * Time.deltaTime);
                if (transform.position.y == _activePosition.y)
                {
                    _isActive = true;
                    timeToActive = Time.time + delayTime;
                }

            }
        }
    }
}
