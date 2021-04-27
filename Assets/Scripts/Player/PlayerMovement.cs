using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour {
    public Player player;
    public float baseSpeed = 2f;
    public float sprintMult = 2f;
    public bool sprinting = true, rolling;
    public int jumps, numJumps = 2;
    public float jumpSpeed = 8f;
    public int facing = 1;

    // Use this for initialization
    void Start() {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        if (player.physics.isTouchingGround) {
            jumps = 0;
        }

        //Determine direction of flip
        float determiner = MoveSpeed != 0 ? MoveSpeed : player.physics.velocity.x;

        if (determiner == 0)
        {
            determiner = facing;
        }

        facing = determiner > 0 ? 1 : -1;
    }

    // Acceleration due to user input
    public float MoveSpeed {
        get {
            return baseSpeed * player.controls.XInput * (sprinting ? sprintMult : 1);
        }
    }

    public Vector2 GetRollVelocity() {
        return Vector2.zero;
    }
    
    public string GetWalkingState() {
        return sprinting ? "Running" : "Walking";
    }
}
