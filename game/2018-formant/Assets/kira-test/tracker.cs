using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class tracker_note {
    public float frequency;
    public float triggerOn;
    public float triggerLength;
}
[System.Serializable]
public struct tracker_note_found {
    public int noteIndex;
    public float distanceFromT;
}

[System.Serializable]
public class score {
    public osc instrument;
    public tracker_note[] notes;
    public bool loop;
}

public class tracker : MonoBehaviour {
    public float bpm = 1.0F;
    float t = 0.0F;
    float totalT = 0;
    float tPerSample = 0;
    float beat;

    private double sampling_frequency = 48000;
    private double sample_per_ms;

    int currentNote;

    public score notesToPlay;

    private SortedDictionary<float, List<int>> notes_per_beat;

    float startT;
    float endT;

    public tracker_note_found lastNote;

    public float tToBeat(float t) {
        return (beat / 60) * t;
    }

    public float beatToT(float beat) {
        return (beat / bpm) * 60;
    }

    tracker_note_found findNextNote(float t, bool tIsBeat = false) {
        if(!tIsBeat) {
            //COVERT T TO REALTIME
            t = this.tToBeat(t);
        }

        float curBeat = Mathf.Floor(t);

        tracker_note_found noteFound = new tracker_note_found();
        noteFound.noteIndex = -1;
        noteFound.distanceFromT = 0;

        if(t < startT) {
            //RETURN FIRST NOTE
            noteFound.noteIndex = 0;
            noteFound.distanceFromT = startT - t;
        }

        if(t <= endT) {
            //FIND THE BEAT - LOOP THROUGH THE NOTES IN THE BEAT
            float lowestNoteT = endT;
            int lowestNote = -1;

            foreach(var curBeatInfo in notes_per_beat[curBeat]) {
                var curNote = notesToPlay.notes[curBeatInfo];
                if(curNote.triggerOn < lowestNote || lowestNote == -1) {
                    lowestNoteT = curNote.triggerOn;
                    lowestNote = curBeatInfo;
                }
            }

            if(lowestNote != -1) {
                noteFound.noteIndex = lowestNote;
                noteFound.distanceFromT = t - lowestNoteT;
            }
        }

        return noteFound;
    }

    tracker_note_found findPrevNote(float t, bool tIsBeat = false) {
        if(!tIsBeat) {
            //COVERT T TO REALTIME
            t = this.tToBeat(t);
        }

        float curBeat = Mathf.Floor(t);
        tracker_note_found noteFound = new tracker_note_found();
        noteFound.noteIndex = -1;
        noteFound.distanceFromT = 0;

        if(t > endT) {
            if(notesToPlay.loop) {
                var totalPlays = Mathf.Floor(t / endT);
                t = t - (totalPlays * endT);
                curBeat = Mathf.Floor(t);
            } else {
                curBeat = Mathf.Floor(endT);
            }
        } 
        
        //RETURN LAST NOTE
        float highestNoteT = startT;
        int highestNote = -1;

        if(notes_per_beat.ContainsKey(curBeat)) {
            foreach(var curBeatInfo in notes_per_beat[curBeat]) {
                var curNote = notesToPlay.notes[curBeatInfo];
                if((curNote.triggerOn > highestNoteT || highestNote == -1) && curNote.triggerOn <= t) {
                    highestNoteT = curNote.triggerOn;
                    highestNote = curBeatInfo;
                }
            }

            if(highestNote != -1) {
                noteFound.noteIndex = highestNote;
                noteFound.distanceFromT = t - highestNoteT;
            }
        }

        return noteFound;
    }

    // Use this for initialization
    void Start() {
        
        notes_per_beat = new SortedDictionary<float, List<int>>();

        beat = 0;
        sampling_frequency = AudioSettings.outputSampleRate;

        float start = notesToPlay.notes[0].triggerOn;
        float end = notesToPlay.notes[0].triggerOn;

        int noteN = 0;
        foreach(tracker_note note in notesToPlay.notes) {
            float beat = Mathf.Floor(note.triggerOn);
            if(!notes_per_beat.ContainsKey(beat)) {
                notes_per_beat.Add(beat,new List<int>());
            }

            notes_per_beat[beat].Add(noteN);
            
            if(note.triggerOn < start) {
                start = note.triggerOn;
            }
            if(note.triggerOn + note.triggerLength > end) {
                end = note.triggerOn + note.triggerLength;
            }

            ++noteN;
        }

        startT = start;
        endT = end;

        //LOOP THROUGH AND ADD ALL MISSING BEATS
        float curBeatT = 0;
        while(curBeatT < endT) {
            if(!notes_per_beat.ContainsKey(beat)) {
                notes_per_beat.Add(beat, new List<int>());
            }
            curBeatT += 1;
        }

        
    }

    // Update is called once per frame
    void Update() {
        //BEAT PER T
        beat += (bpm / 60) * Time.deltaTime;
        //lastNote = findPrevNote(beat,true);
        lastNote = findPrevNote(beat, true);
        if(lastNote.noteIndex < notesToPlay.notes.Length && lastNote.noteIndex >= 0) {
            var curNoteInfo = notesToPlay.notes[lastNote.noteIndex];

            notesToPlay.instrument.play_frequency = notesToPlay.notes[lastNote.noteIndex].frequency;

            if(lastNote.distanceFromT < curNoteInfo.triggerLength) {
                notesToPlay.instrument.trigger = true;
            } else {
                notesToPlay.instrument.trigger = false;
            }
        }
    }

    //USE ONAUDIOFILTEREAD TO MANAGE THE TRACKERS
    void OnAudioFilterRead(float[] data, int channels) {

        int samples = data.Length / channels;

        //HOW MANY MS THIS COVERS
        float ms = samples / (float)sampling_frequency * 1000;
        tPerSample = 1000 / (float)sampling_frequency;

        totalT += ms;

        
    }
}