using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour {
    public Player player;
    public float baseAcceleration = 2f;
    public float sprintMult = 2f;
    public float rollSpeed = 7f;
    public bool sprinting = true, rolling, jumping;
    public int jumps, maxJumps = 2;
    public int rolls, maxRolls = 2;
    public float jumpHeight = 2f;
    public float jumpForce;
    public Vector2 noJumpHoldForce;
    public int facing = 1;

    // Use this for initialization
    void Start() {
        player = GetComponent<Player>();
        jumpForce = player.physics.CalcJumpForce(jumpHeight);
    }

    // Update is called once per frame
    void Update() {
        SetAnimationParams();
    }

    void FixedUpdate() {
        Rigidbody2D rb = player.physics.rigidBody;
        // Add speed due to wasd
        if (player.physics.isTouchingGround) {
            rb.AddForce(player.physics.groundHandler.groundDirection * MoveSpeed * rb.mass);
        } else {
            rb.AddForce(new Vector2(MoveSpeed * rb.mass, 0));
        }
        //player.physics.velocity += new Vector2(MoveSpeed * Time.fixedDeltaTime, 0);

        // Reset jumps and rolls
        if (player.physics.isTouchingGround) {
            jumps = 0;
            rolls = 0;
        }

        // Handle held jump going higher
        if (jumping) {
            if (!player.controls.jumpHeld && player.physics.velocity.y > 0) {
                rb.AddForce(noJumpHoldForce * rb.mass);
            }
        }

        if (!rolling) {
            SetFacing();
        } else {
            DoRoll();
        }
    }

    public void SetFacing() {
        float determiner = MoveSpeed != 0 ? MoveSpeed : facing;
        facing = (int)Mathf.Sign(determiner);
    }

    // Attempts to roll
    public void TryRoll() {
        if (rolls < maxRolls) {
            rolling = true;
            player.animator.SetTrigger(PlayerAnimation.ROLLED);
            SetFacing();
            rolls++;
        }
    }

    // Called every physics timestep while rolling
    public void DoRoll() {
        player.physics.velocity = RollVelocity;
    }

    public void TryJump() {
        // If not touching ground while jumping for the first time, lose your ground jump
        if (jumps == 0 && !player.physics.isTouchingGround) {
            jumps = 1;
        }

        PlayerAnimation anim = player.animator;
        // Make the character jump if they have the jumps required
        if (jumps < maxJumps) {
            player.animator.SetTrigger(PlayerAnimation.JUMPED);
            jumping = true;
            Rigidbody2D rb = player.physics.rigidBody;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * rb.mass, ForceMode2D.Impulse);
            rolling = false;
            jumps++;
        }
    }
    
    // Sets the movement related animation parameters currently applicable
    public void SetAnimationParams() {
        player.animator.sprinting = sprinting;
        player.animator.sliding = Mathf.Abs(player.physics.velocity.x) > 0.001f 
        && Mathf.Abs(player.controls.XInput) <= 0.01f
        && !jumping
        && player.physics.isTouchingGround;
    }

    // Acceleration due to user input
    public float MoveSpeed {
        get {
            return baseAcceleration * player.controls.XInput * SprintMult;
        }
    }

    public Vector2 RollVelocity {
        get {
            return new Vector2(rollSpeed * facing, 0);
        }
    }

    // Returns speed multiplier due to sprint for current state
    public float SprintMult {
        get {
            return (sprinting ? sprintMult : 1);
        }
    }
}
