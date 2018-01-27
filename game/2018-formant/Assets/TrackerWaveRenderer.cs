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
        Amplitudes = new Vector4(1.0f, 0.0f, 0.0f, 2.0f);
        Positions = new Vector4(Mathf.Sin(Time.time) * 0.25f + 0.5f, 0.0f, 0.0f, Mathf.Sin(Time.time * 0.25f) * 0.5f + 0.5f);
        Widths = new Vector4(0.01f, 1.0f, 1.0f, 0.05f);

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
