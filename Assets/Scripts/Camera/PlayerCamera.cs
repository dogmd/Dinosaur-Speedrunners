using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerCamera : MonoBehaviour {
    public Transform player;

    void Start() {}

    void Update() {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
