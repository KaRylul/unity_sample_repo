using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnikiBasicControls : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 2f;
    [SerializeField]
    private float runSpeed = 4f;
    [SerializeField]
    private float speedSmoothVelocity = 0.01f;
    [SerializeField]
    private float speedSmoothTime = 0.01f;
    [SerializeField]
    private float turnSmoothVelocity = 0.01f;
    [SerializeField]
    private float turnSmoothTime = 0.04f;
    [SerializeField]
    private float jumpForce = 5.2f;

    [SerializeField]
    private Transform playerCamera;

    private float currentSpeed;
    private float velocityY;
    private float gravity = 15f;
    private CharacterController controller;
    private Animator animator;
    private float forwardPercent;
    private bool isRunning;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        // input
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 inputDir = input.normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            MovePlayer(inputDir, true);
        }
        else
        {
            MovePlayer(inputDir, false);
        }


    }

    private void MovePlayer(Vector3 inputDir, bool running)
    {
        #region Movement raw script
        if (inputDir != Vector3.zero)
        {
            // this is used to rotate player relatively to camera rotation
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetRotation;
        }

        float targetSpeed = walkSpeed;
        targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        #region Jump mechanics
        if (controller.isGrounded)
        {
            velocityY = -gravity * Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                velocityY = jumpForce;
            }
        }
        else
        {
            velocityY -= gravity * Time.deltaTime;
        }
        #endregion

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        #endregion

        #region Animation
        forwardPercent = currentSpeed;
        Debug.Log(forwardPercent);
        animator.SetFloat("forwardPercent", currentSpeed);


        #endregion
    }



}
