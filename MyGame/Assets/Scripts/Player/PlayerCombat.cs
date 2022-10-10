using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    public Animator _animator;
    private Rigidbody2D _rigidbody;

    public Transform _attackPoint;
    public float _attackRadius = 0.5f;

    public LayerMask _enemies;
    public int _damage = 20;
    public float _attackRate = 2f; // 2 атаки в секунду
    private float _nextAttackTime = 0f;

    private float _lastClickTime = 0f;
    private float _doubleClickTime = 0.2f;
    int _clickCount = 0;

    public int _maxHelth = 100;
    private int _currentHelth;

    public bool _isDead = false;

    void Start()
    {
        _currentHelth = _maxHelth;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(_controller._isGrounded && !_controller._isSliding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttackCombo();
            }
        }
    }
    void AttackCombo()
    {
        float timeSinceLastClick = Time.time - _lastClickTime;
        if (Time.time >= _nextAttackTime)
        {
            Attack1();
            _nextAttackTime = Time.time + 1f / _attackRate;
            _clickCount = 1;
        }
        if ((timeSinceLastClick <= _doubleClickTime) && _clickCount == 1)
        {
            Attack2();
        }
        if (_clickCount == 2)
        {
            Attack3();
            _clickCount = 1;
        }
        if (timeSinceLastClick <= _doubleClickTime)
            ++_clickCount;
        else
            _clickCount = 1;
        _lastClickTime = Time.time;
    }
    void Attack1()
    {
        _controller._blockMoveAttack1 = true;
        _animator.SetTrigger("Attack1");
        _animator.SetBool("Attack2", false);
        _animator.SetBool("Attack3", false);
        MakeDamage();

    }
    void Attack2()
    {
        _controller._blockMoveAttack2 = true;
        _animator.SetBool("Attack2",true);
        MakeDamage();
    }
    void Attack3()
    {
        _controller._blockMoveAttack3 = true;
        _animator.SetBool("Attack3", true);
        MakeDamage();
    }
    void MakeDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _enemies);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyCombat>().TakeDamage(_damage);
        }
    }
    public void TakeDamage(int damage)
    {
        _animator.SetTrigger("Hurt");
        _currentHelth -= damage;
        if (_currentHelth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _isDead = true;
        _animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        _rigidbody.velocity = new Vector2 (0f, 0f);
        this.enabled = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
