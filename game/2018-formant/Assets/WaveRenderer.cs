using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveRenderer : MonoBehaviour {
    public Texture2D Texture;
    public float Radius;

    private Material MeshRenderer;

	// Use this for initialization
	void Start () {
        MeshRenderer = GetComponent<MeshRenderer>().material;
        MeshRenderer.SetTexture("_MainTex", Texture);
    }
	
	// Update is called once per frame
	void Update () {
        MeshRenderer.SetFloat("_Position", Time.time * 0.25f);
        MeshRenderer.SetFloat("_Radius", Radius);
        MeshRenderer.SetFloat("_Amplitude", 0.5f);
        MeshRenderer.SetFloat("_Thickness", 0.25f);
    }

    public float RadiusAt(float position)
    {
        return Radius;
    }
}
