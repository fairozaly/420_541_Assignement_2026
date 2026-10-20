using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    private Rigidbody rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // sned movement speed to the animator
        Vector3 characterSpeed = rb.linearVelocity;
        characterSpeed.y = 0f;

        animator.SetFloat(
            "CharacterSpeed",
            characterSpeed.magnitude
        );

        // ground check
        bool grounded = movement.IsGrounded;
        animator.SetBool("IsGrounded", grounded);

        // roll
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetTrigger("doRoll");
        }

        // double jump/flip
        if (grounded)
        {
            animator.ResetTrigger("doDoubleJump");
        }
        else
        {
            // only animate when the double jump is allowed
            if (Input.GetButtonDown("Jump") &&
                movement.TryDoubleJump())
            {
                animator.SetTrigger("doDoubleJump");
            }
        }
    }
}
