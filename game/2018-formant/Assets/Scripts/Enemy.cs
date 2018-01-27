using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform player;
    public float speed = 5;
    protected Rigidbody rigidBody;

	// Use this for initialization
	protected virtual void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
}
