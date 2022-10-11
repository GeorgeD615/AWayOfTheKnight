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
    private float _nextAttackTime = 0f;


    public int _maxHelth = 100;
    private int _currentHelth;

    public int _hurtCount = 0;


    public bool _isDead = false;

    private bool _blockAttackForHurt = false;

    void Start()
    {
        _currentHelth = _maxHelth;
    }
    void Update()
    {
        if(Time.time >= _nextAttackTime && !_blockAttackForHurt)
        {
            if (Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _player).Length != 0)
            {
                Attack();
            }
        }
        BlockAttackForHurt();
    }

    public bool _blockMoveAttack;
    private float hurtTime = 0.5f;
    private float timerHurt = 0;
    private void BlockAttackForHurt()
    {
        if (_blockAttackForHurt)
        {
            if ((timerHurt += Time.deltaTime) >= hurtTime)
            {
                _blockAttackForHurt = false;
                timerHurt = 0;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        _blockAttackForHurt = true;
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
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
        if(hitPlayer.GetComponent<Rigidbody2D>().velocity.y == 0)
            hitPlayer.GetComponent<PlayerCombat>().TakeDamage(_damage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
