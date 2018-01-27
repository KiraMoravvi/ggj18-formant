using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float xMin = -1, xMax = 1, yMin = -1, yMax = 1;
    public float speed = 10, tilt = 1;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rigidBody.velocity = movement * speed;

        rigidBody.position = new Vector3
        (
            Mathf.Clamp(rigidBody.position.x, xMin, xMax),
            Mathf.Clamp(rigidBody.position.y, yMin, yMax),
            0.0f
        );

        rigidBody.rotation = Quaternion.Euler(0.0f, rigidBody.velocity.x * -tilt, 0.0f);
    }
}
