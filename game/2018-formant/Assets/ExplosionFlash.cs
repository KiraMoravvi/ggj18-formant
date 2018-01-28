using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFlash : MonoBehaviour {

    private double Started;

	// Use this for initialization
	void Start () {
        transform.Rotate(90.0f, 0.0f, 0.0f);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Started = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        var elapsed = Time.time - Started;
        elapsed /= 0.4;
        if (elapsed > 1.0)
        {
            Destroy(gameObject);
            return;
        }
        var scale = (1.0f - (float)elapsed) * 0.25f;
        transform.localScale = new Vector3(scale, scale, scale);
        transform.Rotate(0.0f, 360.0f * Time.deltaTime, 0.0f);
	}
}
