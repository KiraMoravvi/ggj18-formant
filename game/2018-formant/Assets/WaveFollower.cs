using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFollower : MonoBehaviour {
    public Texture2D Wave;

    public float Radius;
    public float WavePosition;
    public float Amplitude;
    public float ShipPosition;

    private Rigidbody Rigidbody;

    public Game Game;

	// Use this for initialization
	void Start () {
        Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody.AddForce(new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * 1000.0f * Time.deltaTime);

        var magnitude = Rigidbody.position.magnitude;
        var normal = Rigidbody.position.normalized;

        var closestWave = Game.Waves.First();
        var distanceToClosestWave = Mathf.Abs(closestWave.RadiusAt(ShipPosition) - magnitude);
        foreach (var wave in Game.Waves.Skip(1))
        {
            var distanceToWave = Mathf.Abs(wave.RadiusAt(ShipPosition) - magnitude);
            if (distanceToWave >= distanceToClosestWave) continue;
            closestWave = wave;
            distanceToClosestWave = distanceToWave;
        }

        var wavePullRolloff = (closestWave.RadiusAt(ShipPosition) - magnitude) * 3.0f;
        wavePullRolloff += Mathf.Sign(wavePullRolloff);
        Rigidbody.AddForce(normal * 1000.0f * Time.deltaTime / wavePullRolloff);
    }
}
