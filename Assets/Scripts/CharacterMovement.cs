using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float runMultiplier = 2f;

    [Header("Jump Settings")]
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 10f;
    [SerializeField] private float maxChargeTime = 3f;
    [SerializeField] private float doubleJumpForce = 7f;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    [Header("Power Ups")]
    [SerializeField] private bool doubleJumpPowerUp = false;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float turnInput;
    private bool isGrounded;

    private bool jumpRequested = false;
    private bool doubleJumpRequested = false;
    private bool hasDoubleJumped = false;
    private bool hasJumped = false;

    private bool isChargingJump = false;
    private float jumpChargeTime = 0f;
    private float currentJumpForce;

    private float boostMultiplier = 1f;
    private Coroutine speedBoostCoroutine;
    private Coroutine doubleJumpCoroutine;

    public bool IsGrounded => isGrounded;
    public float SpeedMultiplier { get; set; } = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // ground check
        isGrounded = Physics.Raycast(
            transform.position + transform.up * groundDistance / 2,
            -transform.up,
            groundDistance,
            groundMask
        );

        // reset th jumps when grounded
        if (isGrounded && !jumpRequested && rb.linearVelocity.y <= 0f)
        {
            hasDoubleJumped = false;
            hasJumped = false;
        }

        // movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = cameraForward * moveZ + cameraRight * moveX;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // rotate toward movement direction
        if (moveDirection.magnitude > 0f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        // run while shift is held
        SpeedMultiplier =
            (Input.GetKey(KeyCode.LeftShift) ? runMultiplier : 1f)
            * boostMultiplier;

        // start charging first jump
        if (Input.GetButtonDown("Jump") && isGrounded && !hasJumped)
        {
            isChargingJump = true;
            jumpChargeTime = 0f;
        }

        // charge jump while holding space
        if (isChargingJump && Input.GetButton("Jump"))
        {
            jumpChargeTime += Time.deltaTime;
            jumpChargeTime = Mathf.Min(
                jumpChargeTime,
                maxChargeTime
            );

            // automaticaly jump after 3 seconds
            if (jumpChargeTime >= maxChargeTime)
            {
                RequestJump();
            }
        }

        // jump when space is released
        if (Input.GetButtonUp("Jump") && isChargingJump)
        {
            RequestJump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (jumpRequested)
        {
            Jump(currentJumpForce);
            jumpRequested = false;
        }

        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }

    // jump strength based on charge time
    private void RequestJump()
    {
        float chargePercent = jumpChargeTime / maxChargeTime;

        currentJumpForce = Mathf.Lerp(
            minJumpForce,
            maxJumpForce,
            chargePercent
        );

        jumpRequested = true;
        hasJumped = true;
        isChargingJump = false;
        jumpChargeTime = 0f;
    }

    // upwards velocity
    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        rb.AddForce(
            Vector3.up * force,
            ForceMode.VelocityChange
        );
    }

    // Move using rigidbody
    private void MovePlayer()
    {
        Vector3 targetVelocity =
            moveDirection * moveSpeed * SpeedMultiplier;

        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z
        );
    }

    // try a second jump  mid-air
    public bool TryDoubleJump()
    {
        if (isGrounded || !hasJumped ||
            hasDoubleJumped || !doubleJumpPowerUp)
        {
            return false;
        }

        doubleJumpRequested = true;
        hasDoubleJumped = true;

        return true;
    }

    // activate double jump power-up
    public void ActivateDoubleJump()
    {
        doubleJumpPowerUp = true;
    }

    // deactivate the double jump power-up
    public void DeactivateDoubleJump()
    {
        doubleJumpPowerUp = false;
    }

    
    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine =
            StartCoroutine(SpeedBoostTimer(multiplier, duration));
    }

    private IEnumerator SpeedBoostTimer(float multiplier, float duration)
    {
        boostMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        boostMultiplier = 1f;
        speedBoostCoroutine = null;
    }

    public void ActivateTimedDoubleJump(float duration)
    {
        if (doubleJumpCoroutine != null)
            StopCoroutine(doubleJumpCoroutine);

        doubleJumpCoroutine =
            StartCoroutine(DoubleJumpTimer(duration));
    }

    private IEnumerator DoubleJumpTimer(float duration)
    {
        ActivateDoubleJump();

        yield return new WaitForSeconds(duration);

        DeactivateDoubleJump();
        doubleJumpCoroutine = null;
    }
}