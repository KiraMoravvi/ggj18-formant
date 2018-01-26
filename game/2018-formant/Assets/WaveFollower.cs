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
        WavePosition = Time.time;
        GetComponent<MeshFilter>().transform.position = new Vector3(0.0f, Wave.GetPixelBilinear(WavePosition + 0.5f, 0).r * Amplitude + Radius, 0.0f);
	}
}
