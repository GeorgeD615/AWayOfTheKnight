using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private float _jumpHight = 400f;
	public float _slideSpeed = -3f;
	[Range(0, 0.3f)] [SerializeField] private float _movementSmoothing = 0.05f;  

	[SerializeField] private LayerMask _groundLayerMask;
	[SerializeField] private LayerMask _wallLayerMask;
	[SerializeField] private Transform _groundCheckTransform;
	[SerializeField] private Transform _wallCheckTransform;

	const float _groundCheckRadius = 0.2f;
	const float _wallCheckRadius = 0.2f;
	private bool _isGrounded;

	public Rigidbody2D _rigidbody;
	private Vector2 _velocity = Vector2.zero;

	//[Header("Events")] // - подзаголовок в инспекторе 
	//[Space] // - интервал в инспекторе 

	public UnityEvent OnLandEvent;
	public UnityEvent IsFallingEvent;
	public UnityEvent WallSliding;

	

	public bool _lookAtRight = true;



	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = _isGrounded;
		_isGrounded = false;

		if (Physics2D.OverlapCircleAll(_groundCheckTransform.position, _groundCheckRadius, _groundLayerMask).Length != 0)
		{
			_isGrounded = true;
			if (wasGrounded)
				OnLandEvent.Invoke();
		}

		if(_rigidbody.velocity.y < 0)
        {
			if(Physics2D.OverlapCircleAll(_wallCheckTransform.position, _wallCheckRadius, _wallLayerMask).Length != 0)
            {
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

		Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
		_rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);
		if (move > 0 && !_lookAtRight)
		{
			Flip();
		}
		else if (move < 0 && _lookAtRight)
		{
			Flip();
		}
		
		if (_isGrounded && jump)
		{
			_isGrounded = false;
			_rigidbody.AddForce(new Vector2(0f, _jumpHight));
		}
	}


	private void Flip()
	{
		_lookAtRight = !_lookAtRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
