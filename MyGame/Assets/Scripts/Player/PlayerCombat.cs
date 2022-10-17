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
    public float _attackRate = 2f; // 2 атаки в секунду
    private float _nextAttackTime = 0f;

    private float _lastClickTime = 0f;
    private float _doubleClickTime = 0.2f;
    int _clickCount = 0;

    public HealthBarScript _healthBar;
    public StaminaScript _stamina;

    public int _maxHelth = 100;
    public int _currentHelth;
    public int _maxStamina = 10000;
    public int _currentStamina;

    public bool _isBlock = false;
    private bool _unBlock = false;

    float staminaRecoveryTime;
    bool staminaRecoveryBlock;

    void Start()
    {
        _currentHelth = _maxHelth;
        _currentStamina = _maxStamina;
        _healthBar.SetMaxHelth(_maxHelth);
        _stamina.SetMaxStamina(_maxStamina);
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(!staminaRecoveryBlock && _currentStamina <= 0)
        {
            staminaRecoveryTime = Time.time + 3f;
            staminaRecoveryBlock = true;
        }
        if ((_currentStamina < _maxStamina) && !_isBlock && Time.time > staminaRecoveryTime)
        {
            _currentStamina += 5;
            staminaRecoveryBlock = false;
            _stamina.SetStamina(_currentStamina);
        }
        if (_controller._isGrounded && !_controller._isSliding && !_blockAttackForHurt)
        {
            if (Input.GetMouseButtonDown(0) && _currentStamina > 3000)
            {
                _currentStamina -= 3000;
                AttackCombo();
            }
            if (Input.GetMouseButtonDown(1))
                _unBlock = false;
            if (Input.GetMouseButton(1) && _currentStamina > 0 && !_unBlock)
            {
                _currentStamina -= 5;
                _stamina.SetStamina(_currentStamina);
                Block();
            }
            if (Input.GetMouseButtonUp(1) || _currentStamina <= 0)
            {
                UnBlock();
            }
        }
        blockAttackForHurt();
    }

    public bool _blockAttackForHurt = false;
    private float hurtTime = 0.2f;
    private float timerHurt = 0;
    private void blockAttackForHurt()
    {
        if (_blockAttackForHurt)
        {
            if ((timerHurt += Time.deltaTime) >= hurtTime)
            {
                _blockAttackForHurt = false;
                hurtTime = 0;
            }
        }
    }

    public bool _blockHurtForAttack = false;
    private float attackTime = 0.2f;
    private float timerAttack = 0;
    private void blockHurtForAttack()
    {
        if (_blockHurtForAttack)
        {
            if ((timerAttack += Time.deltaTime) >= attackTime)
            {
                _blockHurtForAttack = false;
                attackTime = 0;
            }
        }
    }
    void Block()
    {
        if (!_isBlock)
        {
            _animator.SetTrigger("Block");
            _animator.SetBool("UnBlock", false);
        }
        _unBlock = false;
        _isBlock = true;
        _controller._isBlocking = true;
    }
    void UnBlock()
    {
        _isBlock = false;
        _unBlock = true;
        _controller._isBlocking = false;
        _animator.SetBool("UnBlock", true);
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
        if ((timeSinceLastClick <= _doubleClickTime) && _clickCount == 1 )
        {
            Attack2();
        }
        if (_clickCount == 2 )
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
        MakeDamage(20);
    }
    void Attack2()
    {
        _controller._blockMoveAttack2 = true;
        _animator.SetBool("Attack2",true);
        MakeDamage(30);
    }
    void Attack3()
    {
        _controller._blockMoveAttack3 = true;
        _animator.SetBool("Attack3", true);
        MakeDamage(40);
    }
    private void MakeDamage(int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _enemies);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyCombat>().TakeDamage(damage);
        }
    }
    public void TakeDamage(int damage)
    {
        if (_isBlock)
        {
            _animator.SetTrigger("BlockDamage");
            if (_currentStamina > 2000)
                _currentStamina -= 2000;
            else
                _currentStamina = 0;
            _stamina.SetStamina(_currentStamina);
        }
        else if(!_blockHurtForAttack){
            _animator.SetTrigger("Hurt");
            _controller._blockMoveForHurt = true;
            _currentHelth -= damage;
            _healthBar.SetHelth(_currentHelth);
            if (_currentHelth <= 0)
            {
                Die();
            }
        }
        blockHurtForAttack();
    }
    private void Die()
    {
        _animator.SetBool("isDead", true);
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = new Vector2(0, 0);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        this.enabled = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
