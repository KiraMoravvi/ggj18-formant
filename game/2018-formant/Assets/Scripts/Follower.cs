using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Enemy {

	// Update is called once per frame
	void FixedUpdate () {
        float moveHorizontal = player.position.x - transform.position.x;
        float moveVertical = player.position.y - transform.position.y;

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        movement.Normalize();
        rigidBody.velocity = movement * speed;

        rigidBody.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(movement.y, movement.x) * (180.0f / Mathf.PI));
    }
}
