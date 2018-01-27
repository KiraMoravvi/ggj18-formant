using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveRenderer : MonoBehaviour {
    public Texture2D Texture;
    public float Radius;
    public bool IsClosest;

    private Material MeshRenderer;

    private bool WasClosestLastFrame;
    public float NewlyClosestIntensity;
    public float ClosestIntensity;

	// Use this for initialization
	void Start () {
        MeshRenderer = GetComponent<MeshRenderer>().material;
        MeshRenderer.SetTexture("_MainTex", Texture);
    }
	
	// Update is called once per frame
	void Update () {
        if (IsClosest && !WasClosestLastFrame)
        {
            NewlyClosestIntensity = 1.0f;
        }
        else
        {
            NewlyClosestIntensity /= 1.0f + Time.deltaTime * 8.0f;
        }

        if (IsClosest)
        {
            ClosestIntensity = 1.0f;
        }
        else
        {
            ClosestIntensity /= 1.0f + Time.deltaTime * 8.0f;
        }
        WasClosestLastFrame = IsClosest;

        MeshRenderer.SetFloat("_Position", Time.time * 0.25f);
        MeshRenderer.SetFloat("_Radius", Radius);
        MeshRenderer.SetFloat("_Amplitude", 0.5f);
        MeshRenderer.SetFloat("_Thickness", 0.25f);
        MeshRenderer.SetColor("_Color", Color.Lerp(Color.Lerp(new Color(0.25f, 0.5f, 1.0f), new Color(1.0f, 1.5f, 4.0f), ClosestIntensity), new Color(8.0f, 8.0f, 8.0f), NewlyClosestIntensity));
    }

    public float RadiusAt(float position)
    {
        return Texture.GetPixelBilinear(Time.time * 0.25f - (position / Mathf.PI) + 0.5f, 0).r * 0.5f + Radius;
    }
}
