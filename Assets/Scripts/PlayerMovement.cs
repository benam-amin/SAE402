using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // init position : 0.36 -0.1
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private BoxCollider2D bc;

    private float moveDirectionX;
    private bool isGamePaused = false;

    private bool isFacingRight = true;

    [SerializeField]
    private Animator animator;


    [Tooltip("Running system"), SerializeField]
    private float moveSpeed = 10;
    public bool isStunned = false;

    [Header("Position")]
    public bool isGrounded = false;
    public bool isFloatingGrounded = false;

    public bool isOnFallingPlatform = false;

    [SerializeField]
    private LayerMask listGroundLayers;
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask listFloatingPlatformsLayers;

    public bool hasCrossedFloatingPlatforms;
    private float offsetFloatingPlaformsLayer = 0.2f;

    public PlayerContacts playerContacts;
    private PlatformEffector2D platformEffector;

    [Header("Jump system"), ReadOnlyInspector]
    public int jumpCount = 0;
    [SerializeField]
    private int nbMaxJumpsAllowed = 2;
    private float groundCheckRadius = 0.95f;
    [SerializeField, Tooltip("How high the player will jump")]
    private float jumpForce;
    private bool isJumping = false;

    private bool isLandingFast = false;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [Header("Broadcast event channels"), SerializeField]
    private CameraShakeEventChannel onLandingFastSO;
    [SerializeField]
    private ShakeTypeVariable landingFastShakeInfo;
    [SerializeField, Header("Listen to event channels")]
    private BoolEventChannel onTogglePauseEvent;

    [Header("Debug"), SerializeField]
    private VectorEventChannel onDebugTeleportEvent;

    private void OnEnable()
    {
        onTogglePauseEvent.OnEventRaised += OnPauseEvent;
        onDebugTeleportEvent.OnEventRaised += OnDebugTeleport;
    }

    private void OnPauseEvent(bool value)
    {
        isGamePaused = value;
    }

    void Update()
    {
        if (isGamePaused)
        {
            return;
        }

        if (!isStunned)
        {
            Controls();
        }

        if (isLandingFast && isGrounded)
        {
            LandingImpact();
        }

        if (isGrounded && !isJumping)
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        Flip();
        Animations();
    }

    private void Controls()
    {
        moveDirectionX = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (isJumping && rb.linearVelocity.y < 0)
        {
            isJumping = false;
        }

        if (
            jumpBufferCounter > 0f && jumpCount < nbMaxJumpsAllowed && (coyoteTimeCounter > 0f || jumpCount >= 1)
        )
        {
            jumpBufferCounter = 0f;
            Jump(false);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            Jump(true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (isFloatingGrounded && !hasCrossedFloatingPlatforms)
            {
                StartCoroutine(CrossFloatingPlatforms());
            }

            if (!isFloatingGrounded && !isGrounded)
            {
                isLandingFast = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
            }
        }
    }

    private IEnumerator CrossFloatingPlatforms()
    {
        platformEffector = playerContacts.GetTilePlatformEffector(listFloatingPlatformsLayers);
        platformEffector.colliderMask &= ~(1 << gameObject.layer);
        yield return new WaitUntil(() => hasCrossedFloatingPlatforms);
        platformEffector.colliderMask |= 1 << gameObject.layer;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        isFloatingGrounded = IsFloatingGrounded();
        hasCrossedFloatingPlatforms = HasCrossedFloatingPlatforms();

        if (!isStunned)
        {
            Move();
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveDirectionX * moveSpeed, rb.linearVelocity.y);
    }

    private void Animations()
    {
        animator.SetFloat("VelocityX", Mathf.Abs(moveDirectionX));
        animator.SetFloat("VelocityY", rb.linearVelocity.y);
        animator.SetBool("IsOnFallingPlatform", isOnFallingPlatform);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Flip()
    {
        if (moveDirectionX > 0 && !isFacingRight || moveDirectionX < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void Jump(bool shortJump = false)
    {
        float jumpPower = shortJump ? rb.linearVelocity.y * 0.5f : jumpForce;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        if (!shortJump)
        {
            jumpCount++;
            isJumping = true;

            if (jumpCount > 1)
            {
                animator.SetTrigger("DoubleJump");
            }
        }
        else
        {
            coyoteTimeCounter = 0f;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            bc.bounds.size.x / 2 * groundCheckRadius,
            listGroundLayers
        );
    }

    private bool IsFloatingGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            bc.bounds.size.x / 2 * groundCheckRadius,
            listFloatingPlatformsLayers
        );
    }

    private bool HasCrossedFloatingPlatforms()
    {
        Collider2D hitFloatingPlatformsLayer = Physics2D.OverlapBox(
            new Vector2(bc.bounds.center.x, bc.bounds.max.y + offsetFloatingPlaformsLayer),
            new Vector2(bc.size.x, 0.05f),
            0,
            listFloatingPlatformsLayers
        );

        return hitFloatingPlatformsLayer != null;
    }

    public bool IsFalling()
    {
        return rb.linearVelocity.y <= -jumpForce;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(groundCheck.position, bc.bounds.size.x / 2 * groundCheckRadius);
        }

        if (bc != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x, bc.bounds.max.y + offsetFloatingPlaformsLayer),
                new Vector2(bc.size.x, 0.05f)
            );
        }
    }

    public void ToggleState(bool state)
    {
        enabled = !state;
    }

    private void LandingImpact()
    {
        isLandingFast = false;
        onLandingFastSO.Raise(landingFastShakeInfo);
    }

    private void OnDebugTeleport(Vector3 newPos)
    {
#if UNITY_EDITOR
        transform.position = newPos;
#endif
    }

    private void OnDisable()
    {
        onTogglePauseEvent.OnEventRaised -= OnPauseEvent;
        onDebugTeleportEvent.OnEventRaised -= OnDebugTeleport;
    }
}
