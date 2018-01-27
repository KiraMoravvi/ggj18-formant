using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFollower : MonoBehaviour {
    private Rigidbody Rigidbody;

    public float ControllerForce;
    public float WavePull;
    public float WavePullRolloff;

    public Game Game;

	// Use this for initialization
	void Start () {
        Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody.AddForce(new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * ControllerForce * Time.deltaTime);

        var magnitude = Rigidbody.position.magnitude;
        var normal = Rigidbody.position.normalized;
        var angle = Mathf.Atan2(Rigidbody.position.x, Rigidbody.position.y);

        var closestWave = Game.Waves.First();
        var distanceToClosestWave = Mathf.Abs(closestWave.RadiusAt(angle) - magnitude);
        foreach (var wave in Game.Waves.Skip(1))
        {
            var distanceToWave = Mathf.Abs(wave.RadiusAt(angle) - magnitude);
            if (distanceToWave >= distanceToClosestWave) continue;
            closestWave = wave;
            distanceToClosestWave = distanceToWave;
        }

        var wavePullRolloff = (closestWave.RadiusAt(angle) - magnitude) * WavePullRolloff;
        wavePullRolloff += Mathf.Sign(wavePullRolloff);
        Rigidbody.AddForce(normal * WavePull * Time.deltaTime / wavePullRolloff);
    }
}
