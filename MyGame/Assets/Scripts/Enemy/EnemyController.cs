using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _playerTransform;

    [Range(0, 0.3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    public bool _lookAtRight = false;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity = Vector2.zero;

    public enum State {IDLE, IDLECOMBAT, COMBAT};
    public State _currentState;


    public bool _blockMoveHurt;
    private float HurtTime = 0.5f;
    private float timerHurt = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _currentState = State.IDLE;
    }
    void Update()
    {
        switch (_currentState)
        {
            case State.IDLE:
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), -transform.right, 5f, _playerLayer);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1f), -transform.right * 5f, Color.red);

                if (hit.collider != null)
                {
                    _currentState = State.COMBAT;
                    _animator.SetBool("CombatIdle", true);
                }
                break;
            case State.IDLECOMBAT:
                break;
            case State.COMBAT:

                break;
        }

    }
    public void MoveToPlayer(float move)
    {
        if (Time.time > timerHurt && !_blockMoveAttack)
        {      
            Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);

            ChangeDirection(move);
        }
        else
        {
            _rigidbody.velocity = new Vector2(0f, 0f);
        }
        blockMoveForAttack();
    }
    public void blockMoveForHurt(int hitCount)
    {
        if (hitCount == 1)
            timerHurt = Time.time + HurtTime;
        else
            timerHurt += HurtTime;

    }
    public void Flip()
    {
        _lookAtRight = !_lookAtRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void ChangeDirection(float move)
    {
        if (move > 0 && !_lookAtRight)
        {
            Flip();
        }
        else if (move < 0 && _lookAtRight)
        {
            Flip();
        }
    }

    public bool _blockMoveAttack;
    private float attackTime = 0.7f;
    private float timerAttack = 0;

    public void blockMoveForAttack()
    {
        if (_blockMoveAttack)
        {
            _rigidbody.velocity = new Vector2(0f, 0f);
            if ((timerAttack += Time.deltaTime) >= attackTime)
            {
                _blockMoveAttack = false;
                timerAttack = 0;
            }
        }
    }
}
