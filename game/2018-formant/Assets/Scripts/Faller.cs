using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : Enemy {

    public float tilt = 2.0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = player.position.x - transform.position.x;
        float moveVertical = -speed;

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rigidBody.velocity = movement.normalized * speed;
        rigidBody.rotation = Quaternion.Euler(0.0f, rigidBody.velocity.x * -tilt, 0.0f);

        if (rigidBody.position.y < -7.0f)
            rigidBody.position = new Vector3(Random.Range(-5.0f, 5.0f), 7.0f, 0.0f);
    }
}
