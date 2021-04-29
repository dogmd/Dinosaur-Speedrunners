using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundHandler : MonoBehaviour {
    Player player;
    public Vector2 avgGroundNormal;
    public Vector2 groundDirection;
    public LayerMask ground;
    public float groundCheckRadius = 0.15f;
    public int rayCount;
    CapsuleCollider2D groundCheck;
    public Vector2 lastCalcPos;

    // Start is called before the first frame update
    void Start() {
        player = GetComponentInParent<Player>();
        groundCheck = GetComponent<CapsuleCollider2D>();
        ground = LayerMask.GetMask("Ground");
        if (rayCount < 2) {
            rayCount = 2;
        }
    }

    // Send rays down to contact ground and get a normal vector between the left and right of the player
    // Average those vectors to get the direction the player should be rotated.
    void Update() {
        CapsuleCollider2D hitbox = player.physics.hitbox;

        // Where to start raycasts relative to ground check point
        Vector2 leftOff = new Vector2(-hitbox.size.x / 2, hitbox.size.y / 2);
        Vector2 rightOff = new Vector2(hitbox.size.x / 2, hitbox.size.y / 2);
        leftOff = transform.parent.rotation * leftOff;
        rightOff = transform.parent.rotation * rightOff;

        Vector2 left = (Vector2)transform.position + leftOff;
        Vector2 right = (Vector2)transform.position + rightOff;

        // only calculate if rays could have changed
        float xOff = transform.parent.position.x - lastCalcPos.x;
        Vector2 step = (right - left) / (rayCount - 1);
        if (Mathf.Abs(xOff) > step.x) {
            // Send rays down
            avgGroundNormal = Vector2.zero;
            Vector2 pos = left;

            player.physics.isTouchingGround = false;
            // fire rays
            for (int i = 0; i < rayCount; i++) {
                // Send raycast to find ground normals
                RaycastHit2D hit = Physics2D.Raycast(pos, -player.transform.up, hitbox.size.y / 2 + groundCheckRadius, ground);
                if (hit && hit.distance < groundCheckRadius + hitbox.size.y / 2) {
                    player.physics.isTouchingGround = true;
                }
                avgGroundNormal += hit.normal;
                pos += step;

                // if (i != rayCount - 1) {
                //     Debug.DrawLine(pos, pos - step, Color.cyan, 0.1f);
                // }
                // Debug.DrawRay(hit.point, hit.normal, player.physics.isTouchingGround ? Color.green : Color.blue);
            }

            // Average vectors
            avgGroundNormal = avgGroundNormal.normalized;
            // Rotate normal by 90 deg to indicate the direction of the slope in +x
            groundDirection = Quaternion.AngleAxis(90, Vector3.forward) * -this.avgGroundNormal;
            lastCalcPos = transform.parent.position;

            // Debug.DrawRay((player.transform.position - player.transform.rotation * new Vector2(0, hitbox.size.y / 2)), this.avgGroundNormal, Color.red);
        }
    }
}
