using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
// Handles acceleration and any other physics that might come up from player interation
public class PlayerPhysics : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.15f;
    public bool isTouchingGround, falling;
    public Vector2 maxVelocity;
    public Player player;
    public PlayerGroundHandler groundHandler;
    public BoxCollider2D hitbox;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        hitbox = GetComponent<BoxCollider2D>();
        groundHandler = GetComponentInChildren<PlayerGroundHandler>();
    }

    // FixedUpdate is called once per physics timestep
    void FixedUpdate() {
        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundHandler.ground);

        if (velocity.y <= 0 && !isTouchingGround) {
            falling = true;
        } else if (falling && isTouchingGround) {
            player.animator.SetTrigger(PlayerAnimation.LANDED);
            player.movement.jumping = false;
            falling = false;
        }

        // change velocity due to acceleration and clamp value
        velocity = ClampAbsVector(rigidBody.velocity, maxVelocity);
    }

    void Update() {
        SetAnimationParams();
    }

    // Clamps the abs input to bounds
    Vector2 ClampAbsVector(Vector2 input, Vector2 bounds) {
        float clampedX = Mathf.Abs(input.x) > bounds.x ? Mathf.Sign(input.x) * bounds.x : input.x;
        float clampedY = Mathf.Abs(input.y) > bounds.y ? Mathf.Sign(input.y) * bounds.y : input.y;
        return new Vector2(clampedX, clampedY);
    }

    public Vector2 ApplyAcceleration(Vector2 acc) {
        return rigidBody.velocity += acc;
    }

    public float CalcJumpForce(float jumpHeight) {
        return Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * rigidBody.gravityScale * jumpHeight);
    }

    // Sets the movement related animation parameters currently applicable
    public void SetAnimationParams() {
        player.animator.velX = velocity.x;
        player.animator.velY = velocity.y;
        player.animator.isTouchingGround = isTouchingGround;
        player.animator.falling = falling;
    }

    public Vector2 velocity {
        get {
            return rigidBody.velocity;
        }
        set {
            rigidBody.velocity = value;
        }
    }
}
