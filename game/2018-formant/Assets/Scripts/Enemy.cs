using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform player;
    public float speed = 5;
    protected Rigidbody rigidBody;
    protected Mesh mesh;

    public int Health = 3;

	// Use this for initialization
	protected virtual void Start () {
        rigidBody = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshFilter>().mesh;
	}

    public void Hurt()
    {
        Debug.Log("Ouch");

        Health--;
        if (Health <= 0)
            Destroy(gameObject);
    }
}
