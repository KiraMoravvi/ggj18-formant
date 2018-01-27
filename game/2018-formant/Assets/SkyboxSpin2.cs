using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSpin2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0.15f, 0.05f, -0.05f);
    }
}
