using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sideways : Enemy {

    public float range = 2.0f;
    public float tilt = 2.0f;
    bool left = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        bool updatePosition = false;
        if (rigidBody.position.x < -8.0f)
            updatePosition = true;
        else if (rigidBody.position.x > 8.0f)
            updatePosition = true;

        if (updatePosition)
        {
            if (Random.Range(0, 100) > 50)
            {
                rigidBody.position = new Vector3(7.9f, player.position.y + Random.Range(-range / 2.0f, range / 2.0f));
                left = true;
            }
            else
            {
                rigidBody.position = new Vector3(-7.9f, player.position.y + Random.Range(-range / 2.0f, range / 2.0f));
                left = false;
            }
        }

        float moveHorizontal = (left ? -speed : speed) / 2.0f;
        float moveVertical = player.position.y - transform.position.y;

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rigidBody.velocity = movement.normalized * speed;
        rigidBody.rotation = Quaternion.Euler(0.0f, rigidBody.velocity.x * -tilt, left ? -Mathf.PI/2.0f : Mathf.PI/2.0f);
    }
}
