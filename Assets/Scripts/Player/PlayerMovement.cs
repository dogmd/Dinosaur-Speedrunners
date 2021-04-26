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

    // Use this for initialization
    void Start() {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        if (player.physics.isTouchingGround) {
            jumps = 0;
        }
    }

    // Acceleration due to user input
    public float MoveSpeed {
        get {
            return baseSpeed * player.controls.XInput * (sprinting ? sprintMult : 1);
        }
    }
}
