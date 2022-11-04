using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextSceneTrigger : MonoBehaviour
{

    [Header("Events")]
    [Space]

    public UnityEvent ChangeLevel;


    private void Awake()
    {
        if (ChangeLevel == null)
            ChangeLevel = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ChangeLevel.Invoke();
    }
}
