using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private float _jumpHight = 700f;
	[SerializeField] private float _slideSpeed = -2f; 
	[Range(0, 0.3f)] [SerializeField] private float _movementSmoothing = 0.05f; 

	[SerializeField] private LayerMask _groundLayerMask;
	[SerializeField] private LayerMask _wallLayerMask;
	[SerializeField] private Transform _groundCheckTransform;
	[SerializeField] private Transform _wallCheckTransform;

	private const float _groundCheckRadius = 0.1f;
	private const float _wallCheckRadius = 0.1f;
	public bool _lookAtRight = true;

	public bool _isGrounded;
	public bool _isSliding;
	public bool _isBlocking;

	private Rigidbody2D _rigidbody;
	private Vector2 _velocity = Vector2.zero;


	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent IsFallingEvent;
	public UnityEvent WallSliding;

	public Vector2 jumpAngle = new Vector2(3.5f, 10);


    private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		if (IsFallingEvent == null)
			IsFallingEvent = new UnityEvent();
		if (WallSliding == null)
			WallSliding = new UnityEvent();
	}
	private void FixedUpdate()
	{
		bool wasGrounded = _isGrounded;
		_isGrounded = false;
		_isSliding = false;


		if (Physics2D.OverlapCircleAll(_groundCheckTransform.position, _groundCheckRadius, _groundLayerMask).Length != 0)
		{
			_isGrounded = true;
            if (wasGrounded)
            {
				OnLandEvent.Invoke();
			}
		} else if (_rigidbody.velocity.y < 0)
		{
			if (Physics2D.OverlapCircleAll(_wallCheckTransform.position, _wallCheckRadius, _wallLayerMask).Length != 0)
			{
				_isSliding = true;
				WallSliding.Invoke();
			}
			else
			{
				IsFallingEvent.Invoke();
			}
		}

	}
	public void Move(float move, bool jump)
	{
        if (!_blockMoveSlide && !_blockMoveAttack1 && !_blockMoveAttack2 ! && !_blockMoveAttack3 && !_blockMoveForHurt)
        {
            if (!_isBlocking)
            {
				Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
				_rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);

				ChangeDirection(move);
				Jump(jump);
				Slide(jump);
            }
            else
            {
				_rigidbody.velocity = new Vector2(0f, 0f);
			}
        }
		blockMoveForSlideJump();
		blockMoveForAttack();
		blockMoveForHurt();
	}
	private void Flip()
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
	private void Jump(bool jump)
    {
		if (_isGrounded && jump)
		{
			_isGrounded = false;
			_rigidbody.AddForce(new Vector2(0f, _jumpHight));
		}
	}
	private void Slide(bool jump)
    {
		if (_isSliding && !jump)
		{
			_rigidbody.velocity = new Vector2(0f, _slideSpeed);
		}
		else if (_isSliding && jump)
		{
			_blockMoveSlide = true;
			IsFallingEvent.Invoke();
			Flip();
			if(_lookAtRight)
				_rigidbody.velocity = new Vector2(transform.localScale.x + jumpAngle.x, jumpAngle.y);
			else
				_rigidbody.velocity = new Vector2(transform.localScale.x - jumpAngle.x, jumpAngle.y);
		}
	}

	private bool _blockMoveSlide;
	private float jumpWallTime = 0.2f;
	private float timerJumpWall = 0;
	private void blockMoveForSlideJump()
    {
		if (_blockMoveSlide && ((timerJumpWall += Time.deltaTime) >= jumpWallTime))
		{
			if (_isGrounded || _isSliding || Input.GetAxis("Horizontal") != 0)
			{
				_blockMoveSlide = false;
				timerJumpWall = 0;
			}
		}
	}

	public bool _blockMoveAttack1;
	public bool _blockMoveAttack2;
	public bool _blockMoveAttack3;
	private float attackTime = 0.5f;
	private float timerAttack = 0;
	public void blockMoveForAttack()
    {
		if (_blockMoveAttack1)
		{
			_rigidbody.velocity = new Vector2(0f, 0f);
			if ((timerAttack += Time.deltaTime) >= attackTime)
			{
				_blockMoveAttack1 = false;
				timerAttack = 0;
			}
		}
        if (_blockMoveAttack2)
        {
            _rigidbody.velocity = new Vector2(0f, 0f);
            if ((timerAttack += Time.deltaTime) >= attackTime)
            {
                _blockMoveAttack2 = false;
                timerAttack = 0;
            }
        }
		if(_blockMoveAttack3)
		{
			_rigidbody.velocity = new Vector2(0f, 0f);
			if ((timerAttack += Time.deltaTime) >= attackTime)
			{
				_blockMoveAttack3 = false;
				timerAttack = 0;
			}
		}
	}

	public bool _blockMoveForHurt;
	private float hurtTime = 0.3f;
	private float timerHurt = 0;
	private void blockMoveForHurt()
	{
		if (_blockMoveForHurt)
		{
			_rigidbody.velocity = new Vector2(0f, 0f);
			if ((timerHurt += Time.deltaTime) >= hurtTime)
			{
				_blockMoveForHurt = false;
				timerHurt = 0;
			}
		}
	}
}
