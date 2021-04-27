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
    public bool falling;

    void Start() {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update() {
        // Flip character based on velocity / input
        float MoveSpeed = player.movement.MoveSpeed;

        transform.parent.localScale = new Vector2(player.movement.facing, transform.localScale.y);

        // Return to running / walking when animation complete
        animator.SetBool("sprinting", player.movement.sprinting);

        SetIdle();
        SetFalling();
        SetJumping();
        SetLanding();
    }

    // Sets animation state
    public void SetState(string state) 
    {
        if (state != this.state)
        {
            animator.SetTrigger(state);
            this.state = state;
            Debug.Log(state);
        }
    }

    public void SetIdle()
    {
        if (player.physics.velocity.magnitude == 0)
        {
            SetState("Idle");

        }

        else if (state == "Idle")
        {
            if (player.physics.isTouchingGround)
            {
                SetState(player.movement.GetWalkingState());
            }
        }
    }

    public void SetFalling()
    {
        if (player.physics.velocity.y <= 0.01 && !player.physics.isTouchingGround)
        {
            SetState("Falling");
            falling = true;
        }
    }

    public void SetJumping()
    {
        if (player.physics.velocity.y > .01 && !player.physics.isTouchingGround)
        {
            SetState("Jumping");
        }
    }

    public void SetLanding()
    {
        if (player.physics.isTouchingGround && falling)
        {
            falling = false;
            SetState("Landing");
        }
    }
}
