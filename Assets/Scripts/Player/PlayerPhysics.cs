using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
// Handles acceleration and any other physics that might come up from player interation
public class PlayerPhysics : MonoBehaviour {
    private Rigidbody2D rigidBody;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
    public bool isTouchingGround;
    public Vector2 maxVelocity;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called once per physics timestep
    void FixedUpdate() {
        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // change velocity due to acceleration and clamp value
        rigidBody.velocity = ClampAbsVector(rigidBody.velocity, maxVelocity);
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

    public Vector2 velocity {
        get {
            return rigidBody.velocity;
        }
        set {
            rigidBody.velocity = value;
        }
    }
}
