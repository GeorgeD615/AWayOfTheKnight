using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private EnemyMovement _enemyMovement;
    public PlayerCombat _playerCombat;
    public Animator _animator;
    public Transform _attackPoint;
    public float _attackRadius = 0.4f;
    public LayerMask _player;
    public int _damage = 20;
    public float _attackRate = 1f; // 1 атака в секунду
    private float _nextAttackTime = 0f;


    public int _maxHelth = 100;
    private int _currentHelth;

    public int _hurtCount = 0;


    public bool _isDead = false;

    void Start()
    {
        _currentHelth = _maxHelth;
        _hurtCount = 0;
    }
    void Update()
    {
        if(Time.time >= _nextAttackTime /*&& !_playerCombat._isDead */)
        {
            if (Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _player).Length != 0)
            {
                Attack();
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (_controller._currentState != EnemyController.State.COMBAT)
            _controller._currentState = EnemyController.State.COMBAT;
        _animator.SetBool("CombatIdle", true);
        _currentHelth -= damage;
        ++_hurtCount;
        _controller.blockMoveForHurt(_hurtCount);
        if (_hurtCount == 1)
        {
            _enemyMovement._prevPosition = transform.position;
        }
        _animator.SetTrigger("Hurt");
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
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<EnemyController>().enabled = false;
        this.enabled = false;
    }
    private void Attack()
    {
        _nextAttackTime = Time.time + 2f;
        _animator.SetTrigger("Attack");
        _controller._blockMoveAttack = true;
        MakeDamage();
    }
    void MakeDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _player);
        hitPlayer.GetComponent<PlayerCombat>().TakeDamage(_damage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
