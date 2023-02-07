using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SonicBloom.Koreo;

public class GameManager : MonoBehaviour
{
    public AudioSource song;
    public bool startPlaying;
    public NoteScroller noteS;
    public ProgressBar progressB;
    public int numberOfNotesHit;
    public int numberOfNotesMissed;
    public int noteCombo;
    public int totalNumberOfNotes;

    public static GameManager instance;
    public Text notesHitDisplay;

    Koreography playingKoreo;
    public int hitWindowRangeInSamples;
    public List<LaneController> noteLanes = new List<LaneController>();

    public string eventID;
    public float noteSpeed = 1f;
    public float hitWindowRangeInMS = 80;
    public NoteObject noteObjectArchetype;
    public NoteObject holdObjectArchytype;
    public float leadInTime;
    float leadInTimeLeft;
    float timeLeftToPlay;
    Stack<NoteObject> noteObjectPool = new Stack<NoteObject>();

    public float delayTime; 

    public int HitWindowSampleWidth {
		get {
			return hitWindowRangeInSamples;
		}
	}

	public float WindowSizeInUnits {
        get {
			return noteSpeed * (hitWindowRangeInMS * 0.001f);
		}
	}

	public int SampleRate {
		get {
			return playingKoreo.SampleRate;
		}
	}

	public int DelayedSampleTime {
        get {
			return playingKoreo.GetLatestSampleTime() - (int)(song.pitch * leadInTimeLeft * SampleRate);
		}
	}

    public float GetVerticalUnitOffsetForSampleTime(int sampleTime)
{
	    // Get the number of samples we traverse given the current speed in Units-Per-Second.
	    float samplesPerUnit = SampleRate / noteSpeed;

	    return (DelayedSampleTime - sampleTime) / samplesPerUnit;
    }

    void InitializeLeadIn() {
		// Initialize the lead-in-time only if one is specified.
		if (leadInTime > 0f) {
			// Set us up to delay the beginning of playback.
			leadInTimeLeft = leadInTime;
			timeLeftToPlay = leadInTime - Koreographer.Instance.EventDelayInSeconds;
		}
		else {
			// Play immediately and handle offsetting into the song.  Negative zero is the same as
			//  zero so this is not an issue.
			song.time = -leadInTime;
			song.Play();
		}
	}

    // Start is called before the first frame update
    void Start() {
        instance = this;
        notesHitDisplay.text = " ";
        numberOfNotesHit = 0;
        numberOfNotesMissed = 0;
        noteCombo = 0;

        Koreographer.Instance.EventDelayInSeconds = delayTime;


        for (int i = 0; i < noteLanes.Count; ++i){
			noteLanes[i].Initialize(this);
		}

        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);
		List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

        UpdateInternalValues();


        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string payload = evt.GetTextValue();
            
            // Find the right lane.
            for (int j = 0; j < noteLanes.Count; ++j)
            {
                LaneController lane = noteLanes[j];
                if (lane.DoesMatchPayload(payload))
                {
                    // Add the object for input tracking.
                    lane.AddEventToLane(evt);

                    // Break out of the lane searching loop.
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (!startPlaying) {
            if (Input.anyKeyDown) {
                startPlaying = true;
                // noteS.hasStarted = true;
                progressB.hasStarted = true;
                progressB.songLength = song.clip.length;
                progressB.fillSpeed = 1f / progressB.songLength;
                song.Play();
            }
        }
    }

    void UpdateInternalValues() {
        Debug.Log(hitWindowRangeInSamples);
		hitWindowRangeInSamples = (int)(0.001f * hitWindowRangeInMS * SampleRate);
	}

    public NoteObject GetFreshNoteObject()
    {
        NoteObject retObj;

        if (noteObjectPool.Count > 0)
        {
            retObj = noteObjectPool.Pop();
        }
        else
        {
            retObj = GameObject.Instantiate<NoteObject>(noteObjectArchetype);
        }
        
        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }

    public void ReturnNoteObjectToPool(NoteObject obj)
    {
        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);

            noteObjectPool.Push(obj);
        }
    }

    public void NoteHit() {
        // Debug.Log("Hit on Time");
        numberOfNotesHit++;
        noteCombo++;
        notesHitDisplay.text = noteCombo.ToString();
    }

    public void NoteMissed() {
        // Debug.Log("Missed Note");
        numberOfNotesMissed++;
        noteCombo = 0;
        notesHitDisplay.text = noteCombo.ToString();
    }

    public void OnDisable() {
        SaveGamePrefs();
    }

    public void SaveGamePrefs() {
        PlayerPrefs.SetInt("notesHit", numberOfNotesHit);
        PlayerPrefs.SetInt("totalNotes", totalNumberOfNotes);
        PlayerPrefs.SetString("songName", SceneManager.GetActiveScene().name);
    }

}
