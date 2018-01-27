using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float xMin = -1, xMax = 1, yMin = -1, yMax = 1;
    public float speed = 10, tilt = 1;
    private Rigidbody rb;
    private BulletGroup bg;

    public float fireRatePerSec = 0.5f;
    float fireTimer = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bg = GetComponent<BulletGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, xMin, xMax),
            Mathf.Clamp(rb.position.y, yMin, yMax),
            0.0f
        );

        rb.rotation = Quaternion.Euler(0.0f, rb.velocity.x * -tilt, 0.0f);

        Vector3 fireDirection = new Vector3();
        if (Input.GetKey(KeyCode.I))
            fireDirection.y = 1;
        if (Input.GetKey(KeyCode.K))
            fireDirection.y = -1;
        if (Input.GetKey(KeyCode.J))
            fireDirection.x = -1;
        if (Input.GetKey(KeyCode.L))
            fireDirection.x = 1;

        if (fireDirection != Vector3.zero)
        {
            if (fireTimer == 0)
            {
                fireDirection.Normalize();
                bg.Fire(rb.position, fireDirection, 50);
            }

            fireTimer += Time.fixedDeltaTime;
            if (fireTimer > 1.0f / fireRatePerSec)
                fireTimer = 0;
        }
        else
        {
            fireTimer = 0;
        }
    }
}
