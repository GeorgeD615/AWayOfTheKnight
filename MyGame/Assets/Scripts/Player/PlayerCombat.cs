using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    public Animator _animator;
    public Transform _attackPoint;
    public float _attackRadius = 0.5f;
    public LayerMask _enemies;
    public int _damage = 20;
    public float _attackRate = 2f; // 2 атаки в секунду
    float _nextAttackTime = 0f;

    void Update()
    {
        if(_controller._isGrounded && !_controller._isSliding)
        {
            if (Time.time >= _nextAttackTime)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                    _nextAttackTime = Time.time + 1f / _attackRate;
                }
            }
        }
    }

    void Attack()
    {
        _controller._blockMoveAttack = true;
        _animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _enemies);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_damage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
