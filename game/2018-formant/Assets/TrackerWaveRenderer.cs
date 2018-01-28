using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerWaveRenderer : MonoBehaviour, IWaveRenderer
{
    public float Radius = 2.0f;

    private Material Material;
    public bool IsClosest { get; set; }

    private bool WasClosestLastFrame;
    public float NewlyClosestIntensity;
    public float ClosestIntensity;

    // Use this for initialization
    void Start()
    {
        Material = GetComponent<MeshRenderer>().material;
    }

    public Vector4 Amplitudes;
    public Vector4 Positions;
    public Vector4 Widths;

    float posFrequencyRangeMin = 261.63F;        //C4
    float posFrequencyRangeMax = 523.25F;         //C5

    private float clampFrequency(float freq) {
        while(freq > posFrequencyRangeMax) {
            freq = freq / 2;
        }

        while(freq < posFrequencyRangeMin) {
            freq = freq * 2;
        }

        return freq;
    }

    // Update is called once per frame
    void Update()
    {
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

        // TODO: This here just redefines the variables used by this class.
        // These should be filled in when the tracker is ready.

        //GET OSCILATOR
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        osc curOsc = camera.GetComponent<osc>();

        float[] oscPositions = { 0.0f, 0.0f, 0.0f, 0.0f };
        float[] oscWidth = { 0.0f, 0.0f, 0.0f, 0.0f };
        float[] oscAmplitudes = { 1.0f, 0.0f, 0.0f, 0.0f };

        //ALL FREQUENCIES SHOULD BE IN HERE
        float posFrequencyRangeDelta = posFrequencyRangeMax - posFrequencyRangeMin;
        var oscCount = 4;

        if(curOsc.oscillators.Length < 4) {
            oscCount = curOsc.oscillators.Length;
        }

        for(var x = 0; x < oscCount; ++x) {
            oscPositions[x] = ((clampFrequency((float)curOsc.oscillators[x].frequency * curOsc.play_frequency / curOsc.base_frequency) - posFrequencyRangeMin) / posFrequencyRangeMax) + 0.25F;
            oscWidth[x] = (float)curOsc.oscillators[x].gain;

            //COMPRESS - SO LOUDER ONES ARE SMALLER AND SMALLER ONES ARE LARGER
            oscAmplitudes[x] = (float)curOsc.gain_trigger.current  * 2;
        }

        /*
        Debug.Log(oscPositions[0]);
        Debug.Log(oscPositions[1]);
        Debug.Log(oscPositions[2]);
        Debug.Log(oscPositions[3]);
    /*
        Debug.Log(oscWidth[0]);
        Debug.Log(oscWidth[1]);
        Debug.Log(oscWidth[2]);
        Debug.Log(oscWidth[3]);/**/
        /*
        Debug.Log(oscAmplitudes[0]);
        Debug.Log(oscAmplitudes[1]);
        Debug.Log(oscAmplitudes[2]);
        Debug.Log(oscAmplitudes[3]);
        */
        //curOsc.oscillators

        Amplitudes = new Vector4(oscAmplitudes[0], oscAmplitudes[1], oscAmplitudes[2], oscAmplitudes[3]);
        //Positions = new Vector4(Mathf.Sin(Time.time) * 0.25f + 0.5f, 0.3f, 0.0f, Mathf.Sin(Time.time * 0.25f) * 0.5f + 0.5f);
        Positions = new Vector4(oscPositions[0], oscPositions[1], oscPositions[2], -oscPositions[3] * 3);
        Widths = new Vector4(0.01f, 0.01f, 0.01f, 0.2f);

        Material.SetVector("_Amplitudes", Amplitudes);
        Material.SetVector("_Positions", Positions);
        Material.SetVector("_Widths", Widths);
        Material.SetColor("_Color", Color.Lerp(Color.Lerp(new Color(0.25f, 0.5f, 1.0f), new Color(1.0f, 1.5f, 4.0f), ClosestIntensity), new Color(8.0f, 8.0f, 8.0f), NewlyClosestIntensity));
        Material.SetFloat("_NoiseAmplitude", 0.1f);
        Material.SetFloat("_NoisePosition", Time.time * 0.5f);
        Material.SetFloat("_Radius", Radius);
        Material.SetFloat("_Thickness", 0.25f);
    }

    public float RadiusAt(float position)
    {
        var output = Radius;
        output += (Mathf.Cos(Mathf.Min(Mathf.Abs((-position / Mathf.PI) + 0.5f - Positions.x) / Widths.x, Mathf.PI)) * 0.5f + 0.5f) * Amplitudes.x;
        output += (Mathf.Cos(Mathf.Min(Mathf.Abs((-position / Mathf.PI) + 0.5f - Positions.y) / Widths.y, Mathf.PI)) * 0.5f + 0.5f) *Amplitudes.y;
        output += (Mathf.Cos(Mathf.Min(Mathf.Abs((-position / Mathf.PI) + 0.5f - Positions.z) / Widths.z, Mathf.PI)) * 0.5f + 0.5f) *Amplitudes.z;
        output += (Mathf.Cos(Mathf.Min(Mathf.Abs((-position / Mathf.PI) + 0.5f - Positions.w) / Widths.w, Mathf.PI)) * 0.5f + 0.5f) *Amplitudes.w;
        return output;
    }
}
