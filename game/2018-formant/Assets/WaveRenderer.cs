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
    public AudioClip[] AudioClips;
    public double StartAt;

    private Material MeshRenderer;

    private bool WasClosestLastFrame;
    public float NewlyClosestIntensity;
    public float ClosestIntensity;

	// Use this for initialization
	void Start () {
        MeshRenderer = GetComponent<MeshRenderer>().material;
        MeshRenderer.SetTexture("_WaveTex", WaveTexture);
        MeshRenderer.SetTexture("_SequenceTex", SequenceTexture);
        foreach (var audioClip in AudioClips)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.PlayScheduled(StartAt);
        }
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

        var time = AudioSettings.dspTime - StartAt;
        time *= (140.0 / 60.0) / 8.0;
        MeshRenderer.SetFloat("_LoopProgress", ((float)time) - Mathf.Floor((float)time));
        MeshRenderer.SetFloat("_Position", 0.0f);
        MeshRenderer.SetFloat("_NoisePosition", Time.time);
        MeshRenderer.SetFloat("_Radius", Radius);
        Amplitude = Time.time * new Vector3(2.0f, 1.0f, 4.0f);
        Amplitude -= new Vector3(Mathf.Floor(Amplitude.x), Mathf.Floor(Amplitude.y), Mathf.Floor(Amplitude.z));
        Amplitude = new Vector3(Amplitude.x * 2.0f, Amplitude.y * 1.0f, Amplitude.z * 4.0f);
        Amplitude = new Vector3(1.0f - Mathf.Clamp01(Amplitude.x), 1.0f - Mathf.Clamp01(Amplitude.y), 1.0f - Mathf.Clamp01(Amplitude.z));
        MeshRenderer.SetVector("_Amplitude", new Vector4(Amplitude.x, Amplitude.y, Amplitude.z, 0.1f));
        MeshRenderer.SetFloat("_Thickness", 0.25f);
        MeshRenderer.SetColor("_Color", Color.Lerp(Color.Lerp(new Color(0.25f, 0.5f, 1.0f), new Color(1.0f, 1.5f, 4.0f), ClosestIntensity), new Color(8.0f, 8.0f, 8.0f), NewlyClosestIntensity));
    }

    public float RadiusAt(float position)
    {
        var time = AudioSettings.dspTime - StartAt;
        time *= (140.0 / 60.0) / 8.0;
        var loopedTime = ((float)time) - Mathf.Floor((float)time);
        var sequenceSample = SequenceTexture.GetPixelBilinear(loopedTime, 0.0f);
        var waveSample = WaveTexture.GetPixelBilinear((-position / Mathf.PI) + 0.5f, 0);
        return Radius + Vector3.Dot(new Vector3(sequenceSample.r, sequenceSample.g, sequenceSample.b), new Vector3(waveSample.r, waveSample.g, waveSample.b));
    }
}
