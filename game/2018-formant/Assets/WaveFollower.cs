using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFollower : MonoBehaviour {
    public Texture2D Wave;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<MeshFilter>().transform.position = new Vector3(0.0f, Wave.GetPixelBilinear((Time.time) + 0.5f, 0).r, 0.0f);
	}
}
