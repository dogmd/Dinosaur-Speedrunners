using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour {
    string state = "Idle";
    public Animator animator;
    public Player player;

    void Start() {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update() {

    }

    // Sets animation state
    public void SetState(string state) {
        if (this.state != state) {
            animator.SetTrigger(state);
            Debug.Log(state);
        }
        this.state = state;
    }
}
