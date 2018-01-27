using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFollower : MonoBehaviour {
    private Rigidbody Rigidbody;

    public float ControllerForce;
    public float WavePull;

    public Game Game;

	// Use this for initialization
	void Start () {
        Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        var inputMagnitude = new Vector2(inputX, inputY).magnitude;
        if (inputMagnitude > 1.0f)
        {
            inputX /= inputMagnitude;
            inputY /= inputMagnitude;
            inputMagnitude = 1.0f;
        }

        Rigidbody.AddForce(new Vector3(-inputX, inputY, 0.0f) * ControllerForce * Time.deltaTime, ForceMode.Acceleration);

        
        if (inputMagnitude > 0.1f)
        {
            var inputAngle = Mathf.Atan2(-inputX, -inputY) + Mathf.PI;
            var currentAngle = Rigidbody.rotation.eulerAngles.z * Mathf.PI / 180.0f;
            var difference = inputAngle - currentAngle;
            if (difference > Mathf.PI) difference -= Mathf.PI * 2.0f;
            if (difference < -Mathf.PI) difference += Mathf.PI * 2.0f;
            difference *= 100.0f;
            Rigidbody.AddTorque(0.0f, 0.0f, difference, ForceMode.Acceleration);
        }

        transform.GetChild(0).localScale = new Vector3(inputMagnitude * 0.4f, inputMagnitude * 0.7f, 0.0f);
        transform.GetChild(1).localScale = new Vector3(inputMagnitude * 0.5f, inputMagnitude * 0.5f, 0.0f);
        transform.GetChild(0).GetComponent<AudioSource>().volume = inputMagnitude * 0.3f;

        var magnitude = Rigidbody.position.magnitude;
        var normal = Rigidbody.position.normalized;
        var angle = Mathf.Atan2(Rigidbody.position.x, Rigidbody.position.y);

        IWaveRenderer closestWave = null;
        var distanceToClosestWave = float.PositiveInfinity;
        foreach (var wave in Game.Waves) wave.IsClosest = false;
        foreach (var wave in Game.Waves)
        {
            var distanceToWave = Mathf.Abs(wave.RadiusAt(angle) - magnitude);
            if (distanceToWave >= distanceToClosestWave) continue;
            closestWave = wave;
            distanceToClosestWave = distanceToWave;
        }
        closestWave.IsClosest = true;

        var wavePullRolloff = (closestWave.RadiusAt(angle) - magnitude);
        Rigidbody.AddForce(normal * WavePull * Time.deltaTime * wavePullRolloff, ForceMode.Acceleration);
    }
}
