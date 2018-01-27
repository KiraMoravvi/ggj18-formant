using System;
using UnityEngine;

[System.Serializable]
public class _gain : System.Object {
    public double value = 0;
    public double current = 0;
    public double direction = 0;
    public double sample_delta = 0;
}

[System.Serializable]
public class oscillator : System.Object {
    public double frequency;
    public double gain;
    public _gain gain_target = new _gain();

    [HideInInspector]
    public double increment;
    [HideInInspector]
    public double phase;
    [HideInInspector]
    public double avg_gain;

}

public class osc : MonoBehaviour {

    public oscillator[] oscillators;

    private double sampling_frequency = 48000;
    private double sample_per_ms;

    public float gain;
    public float attack;            //delay in ms until on fully
    public float decay;             //delay in ms until off fully

    public bool trigger;            //DETERMINE IF TRIGGER IS ON/OFF
    public _gain gain_trigger;


    bool lastTriggerState;

    float totalT = 0;

    void triggerGainCalc(float speed) {

        if(gain_trigger.value < gain_trigger.current) { gain_trigger.direction = -1; }
        if(gain_trigger.value > gain_trigger.current) { gain_trigger.direction = 1; }
        if(gain_trigger.value == gain_trigger.current) { gain_trigger.direction = 0; }

        gain_trigger.sample_delta = Math.Abs(gain_trigger.value - gain_trigger.current) / (sample_per_ms * speed);
    }

    void osc_gain_clamp() {
        double totalGain = 0;
        int oscCount = oscillators.Length;
        foreach(oscillator curOsc in oscillators) {
            totalGain += curOsc.gain;
        }



        foreach(oscillator curOsc in oscillators) {
            curOsc.avg_gain = curOsc.gain / totalGain;
            curOsc.gain_target.value = curOsc.avg_gain;

            if(curOsc.gain_target.value < curOsc.gain_target.current) { curOsc.gain_target.direction = -1; }
            if(curOsc.gain_target.value > curOsc.gain_target.current) { curOsc.gain_target.direction = 1; }
            if(curOsc.gain_target.value == curOsc.gain_target.current) { curOsc.gain_target.direction = 0; }

            curOsc.gain_target.sample_delta = Math.Abs(curOsc.gain_target.value - curOsc.gain_target.current) / (sample_per_ms * 30);
        }
    }

    // Use this for initialization
    void Start() {
        if(gain < 0) {
            gain = 0;
        }

        if(gain > 1) {
            gain = 1;
        }

        sampling_frequency = AudioSettings.outputSampleRate;

        osc_gain_clamp();

        foreach(oscillator curOsc in oscillators) {
            curOsc.phase = 0;
            curOsc.increment = 0;
            curOsc.gain_target.current = curOsc.gain_target.value;
        }

        sample_per_ms = sampling_frequency / 1000;
        lastTriggerState = trigger;
        
    }

    // Update is called once per frame
    void Update() {
        osc_gain_clamp();

        //CLAMP ATTACK/DELAY AT 30MS TO ATTEMPT TO PREVENT CLIPPING
        if(decay < 30) {decay = 30;}
        if(attack < 30) {attack = 30;}

        if(trigger != lastTriggerState) {
            if(trigger) {
                //RESET OSCILATOR PHASES
                foreach(oscillator curOsc in oscillators) {
                    curOsc.phase = 0;
                    curOsc.increment = 0;
                }

                gain_trigger.value = gain;
                triggerGainCalc(attack);
            } else {
                gain_trigger.value = 0;
                triggerGainCalc(decay);
            }
            lastTriggerState = trigger;
        }

        
    }

    void OnAudioFilterRead(float[] data, int channels) {

        int samples = data.Length / channels;

        //HOW MANY MS THIS COVERS
        float ms = samples / (float)sampling_frequency * 1000;
        totalT += ms;

        //ADJUST GAIN PER SAMPLE
        

        // update increment in case frequency has changed
        for(var j = 0; j < oscillators.Length; ++j) {
            oscillators[j].increment = oscillators[j].frequency * 2 * Math.PI / sampling_frequency;
        }

        for(var i = 0; i < data.Length; i = i + channels) {
            //CALCULATE GLOBAL GAIN VALUE BEFORE WE LOOP THROUGH OSCILLATORS
            gain_trigger.current += gain_trigger.sample_delta * gain_trigger.direction;

            if(gain_trigger.direction == 1 && gain_trigger.current > gain_trigger.value - 0.001) {
                gain_trigger.current = gain_trigger.value;
                gain_trigger.direction = 0;
                gain_trigger.sample_delta = 0;
            }

            if(gain_trigger.direction == -1 && gain_trigger.current < gain_trigger.value - 0.001) {
                gain_trigger.current = gain_trigger.value;
                gain_trigger.direction = 0;
                gain_trigger.sample_delta = 0;
            }


            for(var j = 0; j < oscillators.Length; ++j) {
                oscillator curOsc = oscillators[j];
                //MOVE GAIN CURRENT TOWARDS GAIN TARGET
                curOsc.gain_target.current += curOsc.gain_target.sample_delta * curOsc.gain_target.direction;
            
                if(curOsc.gain_target.direction == 1 && curOsc.gain_target.current > curOsc.gain_target.value - 0.001 ) {
                    curOsc.gain_target.current = curOsc.gain_target.value;
                    curOsc.gain_target.direction = 0;
                    curOsc.gain_target.sample_delta = 0;
                }

                if(curOsc.gain_target.direction == -1 && curOsc.gain_target.current < curOsc.gain_target.value - 0.001) {
                    curOsc.gain_target.current = curOsc.gain_target.value;
                    curOsc.gain_target.direction = 0;
                    curOsc.gain_target.sample_delta = 0;
                }

                curOsc.phase = curOsc.phase + curOsc.increment;
                // this is where we copy audio data to make them “available” to Unity
                data[i] = data[i] + (float)(curOsc.gain_target.current * gain_trigger.current * Math.Sin(curOsc.phase));

                // if we have stereo, we copy the mono data to each channel
                if(channels == 2) data[i + 1] = data[i];
                if(curOsc.phase > 2 * Math.PI) curOsc.phase -= 2 * Math.PI;

            }
        }

        //CHECK INTERNAL GAIN AND AND ADJUST PER VOLUME


    }
}
