using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFollower : MonoBehaviour {
    public Texture2D Wave;

    public float Radius;
    public float WavePosition;
    public float Amplitude;
    public float ShipPosition;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        ShipPosition += Input.GetAxis("Horizontal") * Time.deltaTime;
        WavePosition = Time.time * 0.25f;
        var radius = Wave.GetPixelBilinear(WavePosition - (ShipPosition / Mathf.PI) + 0.5f, 0).r * Amplitude + Radius;
        GetComponent<MeshFilter>().transform.position = new Vector3(Mathf.Sin(ShipPosition) * radius, Mathf.Cos(ShipPosition) * radius, 0.0f);
	}
}
