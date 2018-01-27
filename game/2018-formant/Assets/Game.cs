using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject WavePrefab;
    public GameObject TrackerWavePrefab;
    public GameObject ShipPrefab;

    [Serializable]
    public sealed class Ring
    {
        public Texture2D WaveTexture;
        public Texture2D SequenceTexture;
        public AudioClip[] AudioClips = new AudioClip[3];
    }

    public List<Ring> Rings = new List<Ring>();

    private double StartedAt;
    private int LastLoopId;

    public readonly List<IWaveRenderer> Waves = new List<IWaveRenderer>();
    private readonly List<WaveRenderer> WaveRenderers = new List<WaveRenderer>();

	// Use this for initialization
	void Start () {
        StartedAt = AudioSettings.dspTime;
        foreach (var ring in Rings)
        {
            var waveRenderer = Instantiate(WavePrefab).GetComponent<WaveRenderer>();
            waveRenderer.Radius = Waves.Count + 2;
            waveRenderer.WaveTexture = ring.WaveTexture;
            waveRenderer.SequenceTexture = ring.SequenceTexture;
            waveRenderer.AudioClips = ring.AudioClips;
            waveRenderer.StartAt = StartedAt;
            Waves.Add(waveRenderer);
            WaveRenderers.Add(waveRenderer);
        }
        var trackerWaveRenderer = Instantiate(TrackerWavePrefab).GetComponent<TrackerWaveRenderer>();
        trackerWaveRenderer.Radius = Waves.Count + 2;
        Waves.Add(trackerWaveRenderer);
        var ship = Instantiate(ShipPrefab);
        ship.GetComponent<WaveFollower>().Game = this;
        ship.transform.position = new Vector3(0.0f, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
        var loopId = (int)Mathf.Floor((float)((AudioSettings.dspTime - StartedAt) * 140.0f / 60.0f / 32.0f));
        if (loopId != LastLoopId)
        {
            for (var i = 0; i < UnityEngine.Random.Range(2, 10); i++)
            {
                WaveRenderers[UnityEngine.Random.Range(0, WaveRenderers.Count)].Toggle();
            }
        }
        LastLoopId = loopId;
	}
}
