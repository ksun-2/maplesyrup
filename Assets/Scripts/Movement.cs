//https://github.com/prime31/CharacterController2D

using UnityEngine;
using System.Collections;
using Prime31;


public class Movement : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
    private bool attackActionable = true;
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    public SpriteRenderer _renderer;


	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
        _renderer = GetComponent<SpriteRenderer>();

		_controller.onControllerCollidedEvent += onControllerCollider;
	}

	void onControllerCollider( RaycastHit2D hit )
	{
		if( hit.normal.y == 1f )
			return;
	}

	void Update()
	{
        attackActionable = GameObject.Find("Player").GetComponent<Combat>().actionable;

		if( _controller.isGrounded ){
			_velocity.y = 0;
        }

        var moveInput = Input.GetAxisRaw("Horizontal");
		if(attackActionable && moveInput != 0){
			normalizedHorizontalSpeed = 1;
			if (moveInput > 0){
                normalizedHorizontalSpeed = 1;
                _renderer.flipX = true;
            }
            else{
                normalizedHorizontalSpeed = -1;
                _renderer.flipX = false;
            }
			if( _controller.isGrounded )
				_animator.Play(Animator.StringToHash("Run"));
		}
		else{
			normalizedHorizontalSpeed = 0;
			if( _controller.isGrounded && attackActionable)
				_animator.Play(Animator.StringToHash("Idle"));
		}

		if(attackActionable && _controller.isGrounded && Input.GetButtonDown("Jump"))
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}

		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// jump down
        if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) && Input.GetButtonDown("Jump") && attackActionable)
		{
			_controller.ignoreOneWayPlatformsThisFrame = true;
			Invoke("IgnoreOneWayPlatformsFalse", 0.5f);

			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity ) / 4;
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
		Debug.Log(_controller.ignoreOneWayPlatformsThisFrame);

	}
	void IgnoreOneWayPlatformsFalse(){
		_controller.ignoreOneWayPlatformsThisFrame = false;
	}
}
