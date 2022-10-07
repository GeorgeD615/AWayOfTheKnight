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

    public enum State {IDLE, COMBAT};

    public State _currentState;

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
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), -transform.right, 5f, _playerLayer);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1f), -transform.right * 5f, Color.red);

        if (hit.collider != null)
        {
            _currentState = State.COMBAT;
            _animator.SetBool("CombatIdle", true);
        }
    }

    public bool _blockMoveHurt;
    private float HurtTime = 0.5f;
    private float timerHurt = 0;
    public bool _blockMoveAttack;
    private float attackTime = 0.5f;
    private float timerAttack = 0;

    public void MoveToPlayer(float move)
    {
        if (Time.time > timerHurt)
        {      
            Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);

            ChangeDirection(move);
        }
        else
        {
            _rigidbody.velocity = new Vector2(0f, 0f);
        }
        //blockMoveForHurt();
    }

    public void blockMoveForHurt(int hitCount)
    {
        if (hitCount == 1)
            timerHurt = Time.time + HurtTime;
        else
            timerHurt += HurtTime;

    }

    //void blockMoveForHurt()
    //{
    //    if (_blockMoveHurt)
    //    {
    //        _rigidbody.velocity = new Vector2(0f, 0f);
    //        if ((timerHurt += Time.deltaTime) >= HurtTime)
    //        {
    //            _blockMoveHurt = false;
    //            timerHurt = 0;
    //        }
    //    }
    //}

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
}
