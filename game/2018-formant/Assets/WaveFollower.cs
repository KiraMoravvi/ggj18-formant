using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFollower : MonoBehaviour {
    public Texture2D Wave;

    public float Radius;
    public float WavePosition;
    public float Amplitude;
    public float ShipPosition;

    private Rigidbody Rigidbody;

    public Game Game;

	// Use this for initialization
	void Start () {
        Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody.AddForce(new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * 1000.0f * Time.deltaTime);
	}
}
