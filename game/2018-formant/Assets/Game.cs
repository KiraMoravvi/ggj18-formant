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

    public readonly List<IWaveRenderer> Waves = new List<IWaveRenderer>();

	// Use this for initialization
	void Start () {
        var startedAt = AudioSettings.dspTime;
        foreach (var ring in Rings)
        {
            var waveRenderer = Instantiate(WavePrefab).GetComponent<WaveRenderer>();
            waveRenderer.Radius = Waves.Count + 2;
            waveRenderer.WaveTexture = ring.WaveTexture;
            waveRenderer.SequenceTexture = ring.SequenceTexture;
            waveRenderer.AudioClips = ring.AudioClips;
            waveRenderer.StartAt = startedAt;
            Waves.Add(waveRenderer);
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
		
	}
}
