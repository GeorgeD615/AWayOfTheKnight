using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public Animator _animator;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private EnemyController _controller;

    public float _runSpeed = 30f;
    float _horizontalMove = 0f;


    public int _maxHelth = 100;
    private int _currentHelth;
    public float _biasHurtSpeed = 6f;
    private int _hurtCount = 0;
    public float _biasHurt = 0.2f;
    private Vector3 _prevPosition;


    void Start()
    {
        _currentHelth = _maxHelth;
        _hurtCount = 0;
    }

    void Update()
    {
        HurtBias();
        CombatBehavior();
    }

    private void FixedUpdate()
    {
        _controller.MoveToPlayer(_horizontalMove * Time.fixedDeltaTime);
    }
    public void TakeDamage(int damage)
    {
        if(_controller._currentState != EnemyController.State.COMBAT)
            _controller._currentState = EnemyController.State.COMBAT;
        //_controller._blockMoveHurt = true;
        _currentHelth -= damage;
        ++_hurtCount;
        _controller.blockMoveForHurt(_hurtCount);
        if (_hurtCount == 1)
        {
            _prevPosition = transform.position;
        }
        _animator.SetTrigger("Hurt");
        if (_currentHelth <= 0)
        {
            Die();
        }
    }
    private void HurtBias()
    {
        if (_hurtCount > 0)
        {
            if (_playerTransform.position.x < transform.position.x)
            {
                if (_controller._lookAtRight) _controller.Flip();
                transform.position = Vector3.Lerp(transform.position, new Vector3(_prevPosition.x + _biasHurt, _prevPosition.y, _prevPosition.z), _biasHurtSpeed * Time.deltaTime);
                if (transform.position.x >= _prevPosition.x + (_biasHurt - 0.01f))
                {
                    --_hurtCount;
                    if (_hurtCount != 0)
                        _prevPosition = transform.position;
                }
            }
            else
            {
                if (!_controller._lookAtRight) _controller.Flip();
                transform.position = Vector3.Lerp(transform.position, new Vector3(_prevPosition.x - _biasHurt, _prevPosition.y, _prevPosition.z), _biasHurtSpeed * Time.deltaTime);
                if (transform.position.x <= _prevPosition.x - (_biasHurt - 0.01f))
                {
                    --_hurtCount;
                    if (_hurtCount != 0)
                        _prevPosition = transform.position;
                }
            }
        }
    }
    private void Die()
    {
        _animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
    private void CombatBehavior()
    {
        if (_controller._currentState == EnemyController.State.COMBAT)
        {
            if (Math.Abs(_playerTransform.position.x - transform.position.x) <= 1)
            {
                _horizontalMove = 0f;
            }
            else
            {
                if (_playerTransform.position.x < transform.position.x)
                    _horizontalMove = (-1) * _runSpeed;
                else
                    _horizontalMove = _runSpeed;
            }
            _animator.SetFloat("Speed", Math.Abs(_horizontalMove));
        }
    }
}
