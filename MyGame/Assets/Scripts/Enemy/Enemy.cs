using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator _animator;
    [SerializeField] private Transform _playerTransform;

    public int _maxHelth = 100;
    private int _currentHelth;
    private bool _lookAtRight = false;
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
    }

    public void TakeDamage(int damage)
    {
        _currentHelth -= damage;
        ++_hurtCount;
        if(_hurtCount == 1)
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
                if (_lookAtRight) Flip();
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
                if (!_lookAtRight) Flip();
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
    private void Flip()
    {
        _lookAtRight = !_lookAtRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
