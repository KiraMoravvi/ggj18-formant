using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveRenderer : MonoBehaviour {
    public Texture2D Texture;

    private Material MeshRenderer;

	// Use this for initialization
	void Start () {
        MeshRenderer = GetComponent<MeshRenderer>().material;
        MeshRenderer.SetTexture("_MainTex", Texture);
    }
	
	// Update is called once per frame
	void Update () {
        MeshRenderer.SetFloat("_Position", Time.time);
        MeshRenderer.SetFloat("_Radius", 1.2f);
        MeshRenderer.SetFloat("_Amplitude", 0.1f);
        MeshRenderer.SetFloat("_Thickness", 0.05f);
    }
}
