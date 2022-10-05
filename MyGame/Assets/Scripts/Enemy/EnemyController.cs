using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Animator _animator;

    public enum State {IDLE, COMBATIDLE, COMBAT };

    public State _currentState;

    void Start()
    {
        _currentState = State.IDLE;
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), -transform.right, 5f, _playerLayer);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1f), -transform.right * 5f, Color.red);

        if (hit.collider != null)
        {
            _currentState = State.COMBATIDLE;
            _animator.SetBool("CombatIdle", true);
        }
    }
}
