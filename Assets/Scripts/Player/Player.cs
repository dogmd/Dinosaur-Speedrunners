using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(PlayerPhysics))]
public class Player : MonoBehaviour {
    public PlayerMovement movement;
    public PlayerControls controls;
    public PlayerPhysics physics;
    public PlayerAnimation animationControl;

    // Start is called before the first frame update
    void Start() {
        movement = GetComponent<PlayerMovement>();
        controls = GetComponent<PlayerControls>();
        physics = GetComponent<PlayerPhysics>();
        animationControl = GetComponentInChildren<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update() {

    }
}
