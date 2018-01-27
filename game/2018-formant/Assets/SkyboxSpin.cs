using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSpin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0.25f, 0.1f, -0.1f);
	}
}
