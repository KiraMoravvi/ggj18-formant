using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveRenderer : MonoBehaviour {
    public Texture2D WaveTexture;
    public Texture2D SequenceTexture;
    public float Radius;
    public Vector3 Amplitude;
    public float NoiseAmplitude;
    public bool IsClosest;

    private Material MeshRenderer;

    private bool WasClosestLastFrame;
    public float NewlyClosestIntensity;
    public float ClosestIntensity;

	// Use this for initialization
	void Start () {
        MeshRenderer = GetComponent<MeshRenderer>().material;
        MeshRenderer.SetTexture("_WaveTex", WaveTexture);
        MeshRenderer.SetTexture("_SequenceTex", SequenceTexture);
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

        MeshRenderer.SetFloat("_Position", 0.0f);
        MeshRenderer.SetFloat("_NoisePosition", Time.time);
        MeshRenderer.SetFloat("_Radius", Radius);
        Amplitude = Time.time * new Vector3(2.0f, 1.0f, 4.0f);
        Amplitude -= new Vector3(Mathf.Floor(Amplitude.x), Mathf.Floor(Amplitude.y), Mathf.Floor(Amplitude.z));
        Amplitude = new Vector3(Amplitude.x * 2.0f, Amplitude.y * 1.0f, Amplitude.z * 4.0f);
        Amplitude = new Vector3(1.0f - Mathf.Clamp01(Amplitude.x), 1.0f - Mathf.Clamp01(Amplitude.y), 1.0f - Mathf.Clamp01(Amplitude.z));
        MeshRenderer.SetVector("_Amplitude", new Vector4(Amplitude.x, Amplitude.y, Amplitude.z, 0.25f));
        MeshRenderer.SetFloat("_Thickness", 0.25f);
        MeshRenderer.SetColor("_Color", Color.Lerp(Color.Lerp(new Color(0.25f, 0.5f, 1.0f), new Color(1.0f, 1.5f, 4.0f), ClosestIntensity), new Color(8.0f, 8.0f, 8.0f), NewlyClosestIntensity));
    }

    public float RadiusAt(float position)
    {
        return WaveTexture.GetPixelBilinear(Time.time * 0.25f - (position / Mathf.PI) + 0.5f, 0).r * 0.5f + Radius;
    }
}
