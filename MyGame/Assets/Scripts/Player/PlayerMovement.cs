using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController _controller;
    public Animator _animator;

    public float _runSpeed = 30f;
    public float _slideSpeed = -2f;
    float _horizontalMove = 0f;
    bool _isJumpButtonPressed = false;

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;
        _animator.SetFloat("Speed", Math.Abs(_horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            _isJumpButtonPressed = true;
            _animator.SetBool("Jumping", true);
        }

    }

    public void OnLanding()
    {
        _animator.SetBool("Falling", false);
        _animator.SetBool("Jumping", false);
        _animator.SetBool("Sliding", false);
    }

    public void IsFalling()
    {
        _animator.SetBool("Falling", true);
        _animator.SetBool("Jumping", false);
        _animator.SetBool("Sliding", false);
    }

    public void WallSlide()
    {
        if((_controller._lookAtRight && (Input.GetAxisRaw("Horizontal") == 1)) || (!_controller._lookAtRight && (Input.GetAxisRaw("Horizontal") == -1)))
        {
            _controller._rigidbody.velocity = new Vector2(0, _slideSpeed);
            _animator.SetBool("Sliding", true);
            _animator.SetBool("Falling", false);
            _animator.SetBool("Jumping", false);
        }
        else
        {
            IsFalling();
        }
    }


    private void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _isJumpButtonPressed);
        _isJumpButtonPressed = false;
    }
}
