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

	private const float _groundCheckRadius = 0.2f;
	private const float _wallCheckRadius = 0.2f;
	private bool _lookAtRight = true;

	public bool _isGrounded;
	public bool _isSliding;

	private Rigidbody2D _rigidbody;
	private Vector2 _velocity = Vector2.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent IsFallingEvent;
	public UnityEvent WallSliding;


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

	private bool _blockMoveSlide;
	private float jumpWallTime = 0.2f;
	private float timerJumpWall = 0;
	public Vector2 jumpAngle = new Vector2(3.5f, 10);
	public bool _blockMoveAttack;
	private float attackTime = 0.5f;
	private float timerAttack = 0;
	public void Move(float move, bool jump)
	{
        if (!_blockMoveSlide && !_blockMoveAttack)
        {
			Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
			_rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);

			ChangeDirection(move);
			Jump(jump);
			Slide(jump);
        }
		blockMoveForSlideJump();
		blockMoveForAttack();
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
	public void blockMoveForAttack()
    {
		if (_blockMoveAttack)
        {
			_rigidbody.velocity = new Vector2(0f, 0f);
			if ((timerAttack += Time.deltaTime) >= attackTime)
			{
				_blockMoveAttack = false;
				timerAttack = 0;
			}
		}
	}
}
