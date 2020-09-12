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
    public float ladderSpeed = 6f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;
    private float normalizedLadderSpeed = 0;
    private bool attackActionable = true;
    private bool canClimb = false;
    private bool isClimbing;
    private Combat _combat;
    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    private SpriteRenderer _renderer;

    public GameObject feetBox;
    private FeetBox _feetBox; //lol
    public GameObject downClimb;
    private DownClimb _downClimb;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _combat = GetComponent<Combat>();
        
        _feetBox = feetBox.GetComponent<FeetBox>();
        _downClimb = downClimb.GetComponent<DownClimb>();

        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += OnTriggerExitEvent;
    }

    void onControllerCollider(RaycastHit2D hit) //useless for now
    {
        if (hit.normal.y == 1f)
            return;
    }
    void onTriggerEnterEvent(Collider2D col)
    {
        if (col.IsTouching(this.GetComponent<BoxCollider2D>()) //can't be downclimb hitbox
        && col.gameObject.layer == LayerMask.NameToLayer("Ladders"))
        {
            canClimb = true;
        }
    }
    void OnTriggerExitEvent(Collider2D col)
    {
        stopClimbing();
    }

    void Update()
    {
        //ladder
        //no climb down for now
        if ((canClimb && Input.GetKey("up")) || (_downClimb.canDownClimb && Input.GetKey("down")) && attackActionable)
        {
            isClimbing = true;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }
        if (isClimbing)
        {
            _animator.Play(Animator.StringToHash("Jump")); //placeholder...
            //jump off
            var side = Input.GetAxisRaw("Horizontal");
            if (side != 0 && Input.GetButton("Jump"))
            {
                if (side > 0)
                {
                    _velocity.x = runSpeed;
                }
                else
                {
                    _velocity.x = -runSpeed;
                }
                Jump(0.4f);
                isClimbing = false;
            }
            else
            {
                //move on ladder
                var moveInput = Input.GetAxisRaw("Vertical");
                if (moveInput != 0)
                {
                    if (moveInput > 0)
                    {
                        normalizedLadderSpeed = 1;
                    }
                    else
                    {
                        normalizedLadderSpeed = -1;
                    }
                }
                else
                {
                    normalizedLadderSpeed = 0;
                }
                _velocity.x = 0;
                _velocity.y = normalizedLadderSpeed * ladderSpeed;
                //snap to ladder
                var currentPos = transform.position;
                int rounded = (int)(currentPos.x / .32);
                if (transform.position.x < 0)
                {
                    transform.position = new Vector2(rounded * .32f - 0.16f, currentPos.y);
                }
                else
                {
                    transform.position = new Vector2(rounded * .32f + 0.16f, currentPos.y);
                }
            }
        }
        //not climbing:
        else if (_feetBox.actionable)
        {
            attackActionable = _combat.actionable;
            if (_controller.isGrounded)
            {
                _velocity.y = 0;
            }
            // move L/R
            var moveInput = Input.GetAxisRaw("Horizontal");
            if (attackActionable && moveInput != 0)
            {
                if (moveInput > 0)
                {
                    normalizedHorizontalSpeed = 1;
                    _renderer.flipX = true;
                }
                else
                {
                    normalizedHorizontalSpeed = -1;
                    _renderer.flipX = false;
                }
                if (_controller.isGrounded)
                    _animator.Play(Animator.StringToHash("Run"));
            }
            else
            {
                normalizedHorizontalSpeed = 0;
                if (_controller.isGrounded && attackActionable)
                    _animator.Play(Animator.StringToHash("Idle"));
            }
            // jump
            if (attackActionable && _controller.isGrounded && Input.GetButton("Jump"))
                Jump(1f);
            // change velocity
            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
            _velocity.y += gravity * Time.deltaTime;
            // jump down
            if (_controller.isGrounded && Input.GetKey(KeyCode.DownArrow) && Input.GetButton("Jump") && attackActionable)
            {
                _controller.ignoreOneWayPlatformsThisFrame = true;
                Invoke("IgnoreOneWayPlatformsFalse", 0.3f);
                Jump(0.2f);
            }
        }
        // apply
        _controller.move(_velocity * Time.deltaTime);
        _velocity = _controller.velocity;
        if (_controller.isGrounded)
        {
            isClimbing = false;
        }
        else if (!_controller.isGrounded && attackActionable)
        {
            _animator.Play(Animator.StringToHash("Jump"));
            //always jump animation if in air
            //lol not the best way to do this
        }
    }
    void IgnoreOneWayPlatformsFalse()
    {
        _controller.ignoreOneWayPlatformsThisFrame = false;
    }
    // 1 = jumpHeight
    void Jump(float height)
    {
        _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity * height);
        _animator.Play(Animator.StringToHash("Jump"));
    }
    public void stopClimbing(){
        //ladder workaround... ladder edge collider needs to be 0.03 less than it's
        //supposed to be visually.
        canClimb = false;
        isClimbing = false;
        _controller.ignoreOneWayPlatformsThisFrame = false;
    }
}
