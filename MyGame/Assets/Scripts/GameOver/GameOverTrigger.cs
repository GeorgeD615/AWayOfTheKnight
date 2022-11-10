using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverTrigger : MonoBehaviour
{
    [Header("Events")]
    [Space]

    public UnityEvent GameOver;


    private void Awake()
    {
        if (GameOver == null)
            GameOver = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger");
            GameOver.Invoke();
        }
    }
}
