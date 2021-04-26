using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    // Movement
    public float baseSpeed = 2f;
    public float sprintMult = 2f;
    private bool sprinting = true;
    public int jumps, numJumps = 2;
    public float jumpSpeed = 8f;

    // Animation
    public float idleTime = 2f;
    Animator animator;
    string state;

    // Controls
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction toggleWalkAction;
    public InputAction rollAction;

    // Physics
    private Rigidbody2D rigidBody;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
    public bool isTouchingGround;
    public Vector2 velocity;
    public Vector2 maxVelocity;
    public Vector2 gravity = new Vector2(0, 9.81f);


    // Use this for initialization
    void Start() {
        moveAction.Enable();
        jumpAction.performed += ctx => JumpPressed();
        jumpAction.Enable();
        toggleWalkAction.performed += ctx => ToggleWalk();
        toggleWalkAction.Enable();
        rollAction.performed += ctx => RollPressed();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // FixedUpdate is called once per physics timestep
    void FixedUpdate() {
        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        if (isTouchingGround) {
            jumps = 0;
        }


        // change velocity due to acceleration and clamp value
        this.velocity += new Vector2(MoveSpeed * Time.fixedDeltaTime, 0);
        this.velocity += gravity * Time.fixedDeltaTime;
        this.velocity = ClampAbsVector(this.velocity, maxVelocity);

        rigidBody.velocity = this.velocity;
    }

    // Clamps the abs input to bounds
    Vector2 ClampAbsVector(Vector2 input, Vector2 bounds) {
        float clampedX = Mathf.Abs(input.x) > bounds.x ? Mathf.Sign(input.x) * bounds.x : input.x;
        float clampedY = Mathf.Abs(input.y) > bounds.y ? Mathf.Sign(input.y) * bounds.y : input.y;
        return new Vector2(clampedX, clampedY);
    }

    // Update is called once per frame
    void Update() {
        // Flip character based on velocity / input
        float determiner = MoveSpeed != 0 ? MoveSpeed : rigidBody.velocity.x;
        transform.localScale = new Vector2(determiner > 0 ? 1 : -1, transform.localScale.y);

        if (rigidBody.velocity.magnitude != 0) {
            CancelInvoke("GoIdle");
            if (sprinting) {
                SetState("Running");
            } else {
                SetState("Walking");
            }
        } else if (!IsInvoking("GoIdle")) {
            Invoke("GoIdle", idleTime);
        }
    }

    void GoIdle() {
        SetState("Idle");
    }

    // Sets animation state
    void SetState(string state) {
        if (this.state != state) {
            animator.SetTrigger(state);
            Debug.Log(state);
        }
        this.state = state;
    }

    // ToggleWalk Action callback
    public void ToggleWalk() {
        sprinting = !sprinting;
    }

    // Jump Action callback
    public void JumpPressed() {
        // If not touching ground while jumping for the first time, lose your ground jump
        if (jumps == 0 && !isTouchingGround) {
            jumps = 1;
        }

        // Make the character jump if they have the jumps required
        if (jumps < numJumps - 1) {
            this.velocity = new Vector2(this.velocity.x, jumpSpeed);
            jumps++;
            SetState("Jumped");
        }
    }

    public void RollPressed() {

    }

    // Acceleration due to user input
    public float MoveSpeed {
        get {
            return baseSpeed * moveAction.ReadValue<Vector2>().x * (sprinting ? sprintMult : 1);
        }
    }
}
