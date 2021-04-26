using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour {
    string state = "Idle";
    public Animator animator;
    public Player player;
    public string returnState;

    void Start() {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update() {
        // Flip character based on velocity / input
        float MoveSpeed = player.movement.MoveSpeed;
        float determiner = MoveSpeed != 0 ? MoveSpeed : player.physics.velocity.x;
        transform.parent.localScale = new Vector2(determiner > 0 ? 1 : -1, transform.localScale.y);

        // Return to running / walking when animation complete
        animator.SetBool("sprinting", player.movement.sprinting);
    }

    // Sets animation state
    public void SetState(string state) {
        animator.SetTrigger(state);
        this.state = state;
    }
}
