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
    public bool jumpHeld = false;

    // Use this for initialization
    void Start() {
        player = GetComponent<Player>();
        moveAction.Enable();
        jumpAction.performed += JumpPressedReleased;
        jumpAction.Enable();
        toggleWalkAction.performed += ctx => player.movement.sprinting = !player.movement.sprinting;
        toggleWalkAction.Enable();
        rollAction.performed += ctx => player.movement.TryRoll();
        rollAction.Enable();
    }

    // called once when jump pressed and once when released
    void JumpPressedReleased(InputAction.CallbackContext ctx) {
        jumpHeld = !jumpHeld;
        if (jumpHeld) {
            player.movement.TryJump();
        }
    }

    // X axis user input
    public float XInput {
        get {
            return moveAction.ReadValue<Vector2>().x;
        }
    }

    // Y axis user input
    public float YInput {
        get {
            return moveAction.ReadValue<Vector2>().y;
        }
    }
}
