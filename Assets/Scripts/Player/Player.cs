using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAnimation))]
public class Player : MonoBehaviour {
    public PlayerMovement movement;
    public PlayerControls controls;
    public PlayerPhysics physics;
    public PlayerAnimation animator;

    // Start is called before the first frame update
    void Start() {
        movement = GetComponent<PlayerMovement>();
        controls = GetComponent<PlayerControls>();
        physics = GetComponent<PlayerPhysics>();
        animator = GetComponent<PlayerAnimation>();
    }
}
