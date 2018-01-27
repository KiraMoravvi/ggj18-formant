using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform player;
    public float speed = 5;
    protected Rigidbody rigidBody;
    protected Mesh mesh;

	// Use this for initialization
	protected virtual void Start () {
        rigidBody = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshFilter>().mesh;
	}
}
