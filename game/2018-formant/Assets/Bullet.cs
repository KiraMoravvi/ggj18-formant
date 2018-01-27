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
