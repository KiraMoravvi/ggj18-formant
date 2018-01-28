using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveRenderer : MonoBehaviour, IWaveRenderer {
    public Texture2D WaveTexture;
    public Texture2D SequenceTexture;
    public float Radius;
    public float RadiusWrapLow;
    public float RadiusWrapHigh;
    public Vector3 Amplitude;
    public float NoiseAmplitude;
    public bool IsClosest { get; set; }
    public AudioClip[] AudioClips;
    public double StartAt;

    private Material MeshRenderer;

    private bool WasClosestLastFrame;
    public float NewlyClosestIntensity;
    public float ClosestIntensity;
    private readonly List<AudioSource> AudioSources = new List<AudioSource>();

    public UnityEngine.Audio.AudioMixerGroup audioOutput;

    private double[] FadingSince = new double[3];
    private bool[] FadingOut = new bool[3];

	// Use this for initialization
	void Start () {
        for (var i = 0; i < 3; i++) FadingSince[i] = StartAt;
        for (var i = 0; i < 3; i++) FadingOut[i] = Random.Range(0, 2) == 1;
        MeshRenderer = GetComponent<MeshRenderer>().material;
        MeshRenderer.SetTexture("_WaveTex", WaveTexture);
        MeshRenderer.SetTexture("_SequenceTex", SequenceTexture);
        foreach (var audioClip in AudioClips)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.PlayScheduled(StartAt);
            audioSource.outputAudioMixerGroup = audioOutput;
            AudioSources.Add(audioSource);
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
        MeshRenderer.SetFloat("_Radius", EffectiveRadius);
        Amplitude = Time.time * new Vector3(2.0f, 1.0f, 4.0f);
        Amplitude -= new Vector3(Mathf.Floor(Amplitude.x), Mathf.Floor(Amplitude.y), Mathf.Floor(Amplitude.z));
        Amplitude = new Vector3(Amplitude.x * 2.0f, Amplitude.y * 1.0f, Amplitude.z * 4.0f);
        Amplitude = new Vector3(1.0f - Mathf.Clamp01(Amplitude.x), 1.0f - Mathf.Clamp01(Amplitude.y), 1.0f - Mathf.Clamp01(Amplitude.z));
        for (var i = 0; i < 3; i++)
        {
            var volume = AudioSettings.dspTime - FadingSince[i];
            volume *= 140.0 / 60.0 / 16.0;
            if (FadingOut[i]) volume = 1.0 - volume;
            AudioSources[i].volume = Mathf.Clamp01((float)volume);
        }
        Amplitude = new Vector3(Amplitude.x * AudioSources[0].volume, Amplitude.y * AudioSources[1].volume, Amplitude.z * AudioSources[2].volume);
        MeshRenderer.SetVector("_Amplitude", new Vector4(Amplitude.x, Amplitude.y, Amplitude.z, 0.1f));
        MeshRenderer.SetFloat("_Thickness", 0.25f);
        MeshRenderer.SetColor("_Color", Color.Lerp(Color.Lerp(new Color(0.25f, 0.5f, 1.0f), new Color(1.0f, 1.5f, 4.0f), ClosestIntensity), new Color(8.0f, 8.0f, 8.0f), NewlyClosestIntensity));
    }

    public void Toggle()
    {
        var channel = Random.Range(0, 3);
        FadingSince[channel] = AudioSettings.dspTime;
        FadingOut[channel] = !FadingOut[channel];
    }

    public float RadiusAt(float position)
    {
        var time = AudioSettings.dspTime - StartAt;
        time *= (140.0 / 60.0) / 8.0;
        var loopedTime = ((float)time) - Mathf.Floor((float)time);
        var sequenceSample = SequenceTexture.GetPixelBilinear(loopedTime, 0.0f);
        var waveSample = WaveTexture.GetPixelBilinear((-position / Mathf.PI) + 0.5f, 0);
        return EffectiveRadius + Vector3.Dot(new Vector3(sequenceSample.r, sequenceSample.g, sequenceSample.b), new Vector3(waveSample.r, waveSample.g, waveSample.b));
    }

    public float EffectiveRadius
    {
        get
        {
            var radius = Radius - RadiusWrapLow;
            radius /= RadiusWrapHigh - RadiusWrapLow;
            radius += Time.time * 0.1f;
            radius -= Mathf.Floor(radius);
            radius *= RadiusWrapHigh - RadiusWrapLow;
            radius += RadiusWrapLow;
            return radius;
        }
    }
}
