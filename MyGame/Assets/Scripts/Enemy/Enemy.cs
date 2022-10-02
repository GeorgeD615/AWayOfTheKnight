using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator _animator;

    public int _maxHelth = 100;
    private int _currentHelth;
    void Start()
    {
        _currentHelth = _maxHelth;
    }

    void Update()
    {
     
    }

    public void TakeDamage(int damage)
    {
        _currentHelth -= damage;

        _animator.SetTrigger("Hurt");

        Vector2 newPosition = new Vector2(transform.position.x + 0.1f, transform.position.y);
        transform.position = newPosition;

        if (_currentHelth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
