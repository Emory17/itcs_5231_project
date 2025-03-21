using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;
using Utilities;


namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] GroundChecker groundChecker;
        //[SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] InputReader input;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;

        [Header("Dash Settings")]
        [SerializeField] float dashForce = 5f;
        [SerializeField] float dashDuration = 0.5f;
        [SerializeField] float dashCooldown = 2f;

        [Header("Bounce Settings")]
        [SerializeField] float bounceForce = 10f;

        Transform mainCam;

        float currentSpeed;
        float velocity;
        float jumpVelocity;
        float dashVelocity = 1f;

        bool groundOverride = false;
        bool canDash = true;

        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        CountdownTimer dashTimer;
        CountdownTimer dashCooldownTimer;

        void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - freeLookVCam.transform.position - Vector3.forward);

            rb.freezeRotation = true;

            SetupTimers();
        }

        void SetupTimers()
        {
            // Setup timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            dashTimer = new CountdownTimer(dashDuration);
            dashCooldownTimer = new CountdownTimer(dashCooldown);

            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            dashTimer.OnTimerStart += () => dashVelocity = dashForce;
            dashTimer.OnTimerStop += () => {
                dashVelocity = 1f;
                dashCooldownTimer.Start();
            };

            timers = new(4) { jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer };
        }

        private void Start()
        {
            input.EnablePlayerActions();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bounce"))
            {
                jumpVelocity = bounceForce;
                dashTimer.Stop();
                groundOverride = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Bounce"))
            {
                groundOverride = false;
            }
        }

        void OnEnable()
        {
            input.Jump += OnJump;
            input.Dash += OnDash;
        }

        void OnDisable()
        {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
        }

        void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }

        void OnDash(bool performed)
        {
            if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning && canDash)
            {
                dashTimer.Start();
                canDash = false;
            }
            else if (!performed && dashTimer.IsRunning)
            {
                dashTimer.Stop();
            }
        }

        void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
        }

        private void FixedUpdate()
        {
            HandleJump();
            HandleMovement();
            HandleTimers();
            HandleStates();
        }

        void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        void HandleStates()
        {
            if (groundChecker.IsGrounded)
            {
                canDash = true;
            }
        }

        public void HandleJump()
        {
             // If not jumping and grounded, keep jump velocity at 0
             if (!jumpTimer.IsRunning && groundChecker.IsGrounded && !groundOverride)
             {
                 jumpVelocity = 0f;
                 return;
             }

             if(dashTimer.IsRunning)
             {
                jumpVelocity = 0f;
                rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
                return;
             }

             if (!jumpTimer.IsRunning)
             {
                 // Gravity takes over
                 jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
             }

             // Apply velocity
             rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
         }

        void HandleMovement()
        {
            if (dashTimer.IsRunning)
            {
                HandleHorizontalMovement(transform.forward);
            }
            else
            {
                //Rotate Direction to match camera
                var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

                if (adjustedDirection.magnitude > 0f)
                {
                    HandleRotation(adjustedDirection);
                    HandleHorizontalMovement(adjustedDirection);
                    SmoothSpeed(adjustedDirection.magnitude);
                }
                else
                {
                    SmoothSpeed(0f);

                    //Reset Velocity
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }
            }
        }

        void HandleRotation(Vector3 adjustedDirection)
        {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            // Move the player
            Vector3 velocity = adjustedDirection * (moveSpeed * dashVelocity * Time.fixedDeltaTime);
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }

        void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }
}
