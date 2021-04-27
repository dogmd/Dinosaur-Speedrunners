using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
// Handles changing the player's state in response to the user's input
public class PlayerControls : MonoBehaviour {
    // Base movement actions
    public Player player;
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction toggleWalkAction;
    public InputAction rollAction;

    // Use this for initialization
    void Start() {
        player = GetComponent<Player>();
        moveAction.Enable();
        jumpAction.performed += ctx => JumpPressed();
        jumpAction.Enable();
        toggleWalkAction.performed += ctx => ToggleWalk();
        toggleWalkAction.Enable();
        rollAction.performed += ctx => RollPressed();
        rollAction.Enable();

    }

    // Walk
    void Update() {
        player.physics.ApplyAcceleration(new Vector2(player.movement.MoveSpeed * Time.fixedDeltaTime, 0));
    }

    // ToggleWalk Action callback
    public void ToggleWalk() {
        player.movement.sprinting = !player.movement.sprinting;
        player.animationControl.SetState(player.movement.GetWalkingState());
    }

    // Jump Action callback
    public void JumpPressed() {
        // If not touching ground while jumping for the first time, lose your ground jump
        if (player.movement.jumps == 0 && !player.physics.isTouchingGround) {
            player.movement.jumps = 1;
        }


        // Make the character jump if they have the jumps required
        if (player.movement.jumps < player.movement.numJumps) {
            player.physics.velocity = new Vector2(player.physics.velocity.x, player.movement.jumpSpeed);
            player.movement.jumps++;
            player.animationControl.SetState("Jumped");
        }
    }

    public void RollPressed() {
        player.movement.rolling = !player.movement.rolling;
        player.animationControl.SetState("Rolled");
    }


    // Acceleration due to user input
    public float XInput {
        get {
            return moveAction.ReadValue<Vector2>().x;
        }
    }

    public float YInput {
        get {
            return moveAction.ReadValue<Vector2>().y;
        }
    }
}
