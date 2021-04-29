using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerGroundHandler : MonoBehaviour {
    Player player;
    public Vector2 lastGroundNormal;
    public Vector2 groundDirection;
    public LayerMask ground;
    CapsuleCollider2D groundCheck;

    // Start is called before the first frame update
    void Start() {
        player = GetComponentInParent<Player>();
        groundCheck = GetComponent<CapsuleCollider2D>();
        ground = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update() {
        if (player.physics.isTouchingGround) {
            BoxCollider2D hitbox = player.physics.hitbox;

            Vector2 left = (Vector2)transform.position + new Vector2(-hitbox.size.x / 1.5f, hitbox.size.y / 2);
            Vector2 right = (Vector2)transform.position + new Vector2(hitbox.size.x / 1.5f, hitbox.size.y / 2);

            RaycastHit2D leftHit = Physics2D.Raycast(left, -transform.parent.up, Mathf.Infinity, ground);
            RaycastHit2D rightHit = Physics2D.Raycast(right, -transform.parent.up, Mathf.Infinity, ground);
            Debug.DrawRay(leftHit.point, leftHit.normal);
            Debug.DrawRay(rightHit.point, rightHit.normal);
            Debug.DrawLine(left, right);

            lastGroundNormal = (leftHit.normal + rightHit.normal).normalized;
            groundDirection = Quaternion.AngleAxis(90, Vector3.forward) * -lastGroundNormal;
        }
    }
}
