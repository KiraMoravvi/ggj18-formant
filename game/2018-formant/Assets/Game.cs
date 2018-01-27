using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject WavePrefab;

    public List<Texture2D> WaveTextures = new List<Texture2D>();
    public readonly List<WaveRenderer> Waves = new List<WaveRenderer>();

	// Use this for initialization
	void Start () {
        foreach (var waveTexture in WaveTextures)
        {
            var waveRenderer = Instantiate(WavePrefab).GetComponent<WaveRenderer>();
            waveRenderer.Radius = Waves.Count + 1;
            waveRenderer.Texture = waveTexture;
            Waves.Add(waveRenderer);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
