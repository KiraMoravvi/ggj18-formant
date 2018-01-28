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
        Vector3 worldPos = Camera.main.WorldToScreenPoint(rigidBody.position);
        Vector3 worldPosLeft = Camera.main.WorldToScreenPoint(rigidBody.position - mesh.mesh.bounds.size);
        Vector3 worldPosRight = Camera.main.WorldToScreenPoint(rigidBody.position + mesh.mesh.bounds.size);
        if (worldPosRight.x < 0)
            updatePosition = true;
        else if (worldPosLeft.x > Camera.main.pixelWidth)
            updatePosition = true;

        if (updatePosition)
        {
            if (Random.Range(0, 100) > 50)
            {
                worldPos.x = (worldPosLeft.x - worldPos.x) + 5;
                left = false;
            }
            else
            {
                worldPos.x = (worldPosRight.x - worldPos.x) + Camera.main.pixelWidth - 5;
                left = true;
            }

            rigidBody.position = Camera.main.ScreenToWorldPoint(worldPos);
        }

        float moveHorizontal = (left ? speed : -speed) / 2.0f;
        float moveVertical = player.position.y - transform.position.y;

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rigidBody.velocity = movement.normalized * speed;
        rigidBody.rotation = Quaternion.Euler(0.0f, rigidBody.velocity.x * -tilt, left ? -Mathf.PI/2.0f : Mathf.PI/2.0f);
    }
}
