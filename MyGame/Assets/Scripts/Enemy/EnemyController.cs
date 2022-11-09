using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _leftBord;
    [SerializeField] private float _rightBord;

    [Range(0, 0.3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    [SerializeField] private BoxCollider2D boxCollider;
    private float _rangeX = 20;
    private float _rangeY = 2.5f;
    private float _colliderDistanceX = 0.21f;
    private float _colliderDistanceY = 0.4f;

    public bool _lookAtRight = false;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity = Vector2.zero;

    public enum State {IDLE, IDLECOMBAT, COMBAT, DEATH};
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
        switch (_currentState)
        {
            case State.IDLE:
                if (PlayerInSight())
                {
                    _currentState = State.COMBAT;
                    _animator.SetBool("CombatIdle", true);
                }
                break;
            case State.COMBAT:
                if ((_playerTransform.position.x <= transform.position.x && _lookAtRight) || (_playerTransform.position.x > transform.position.x && !_lookAtRight))
                    Flip();
                if (!PlayerInSight() || !PlayerInRadius())
                    _currentState = State.IDLECOMBAT;
                break;
            case State.IDLECOMBAT:
                if (PlayerInSight() && PlayerInRadius())
                    _currentState = State.COMBAT;
                break;
            case State.DEATH:
                break;
            
        }

    }
    public void MoveToPlayer(float move)
    {
        if(_currentState == State.COMBAT)
        {
            if (Time.time > timerHurt && !_blockMoveAttack)
            {
                Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
                _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);
            }
            else
            {
                _rigidbody.velocity = new Vector2(0f, 0f);
            }
            blockMoveForAttack();
        }
        else
        {
            _rigidbody.velocity = new Vector2(0f, 0f);
        }
        ChangeDirection(move);
    }
    private float HurtTime = 0.5f;
    private float timerHurt = 0;
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
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
                    Physics2D.BoxCast(boxCollider.bounds.center - transform.right * _rangeX * transform.localScale.x * _colliderDistanceX + transform.up * _rangeY * _colliderDistanceY,
                    new Vector3(boxCollider.bounds.size.x * _rangeX, boxCollider.bounds.size.y * _rangeY, boxCollider.bounds.size.z),
                    0, Vector2.left, 0, _playerLayer);
        return hit.collider != null;
    }

    private bool PlayerInRadius()
    {
        return ((_playerTransform.transform.position.x > _leftBord) && (_playerTransform.transform.position.x < _rightBord));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * _rangeX * transform.localScale.x * _colliderDistanceX + transform.up * _rangeY * _colliderDistanceY,
            new Vector3(boxCollider.bounds.size.x * _rangeX, boxCollider.bounds.size.y*_rangeY, boxCollider.bounds.size.z));
    }
}
