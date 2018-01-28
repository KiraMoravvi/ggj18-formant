using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody rb;

    public Rigidbody RB
    {
        get { return rb; }
    }

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
	}

    void OnBecameInvisible()
    {
        Kill();
    }

    void Update()
    {
        Vector3 worldPos = Camera.main.WorldToScreenPoint(rb.position);

        if (worldPos.y < 0 || worldPos.y > Camera.main.pixelHeight)
            Kill();
        if (worldPos.x < 0 || worldPos.x > Camera.main.pixelWidth)
            Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e)
        {
            e.Hurt();
            Kill();
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
