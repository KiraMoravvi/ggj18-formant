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

        Vector3 worldPos = Camera.main.WorldToScreenPoint(rigidBody.position);
        Vector3 worldPosTop = Camera.main.WorldToScreenPoint(rigidBody.position + mesh.mesh.bounds.size);
        
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rigidBody.velocity = movement.normalized * speed;
        rigidBody.rotation = Quaternion.Euler(0.0f, rigidBody.velocity.x * -tilt, 0.0f);

        if (worldPosTop.y < 0)
        {
            worldPos.x = Random.Range(-50, 50) + (Camera.main.pixelWidth / 2.0f);
            worldPos.y = Camera.main.pixelHeight + (worldPosTop.y - worldPos.y);
            rigidBody.position = Camera.main.ScreenToWorldPoint(worldPos);
        }
    }
}
