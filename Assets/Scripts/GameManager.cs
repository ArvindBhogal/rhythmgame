using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource song;
    public bool startPlaying;
    public NoteScroller noteS;
    public ProgressBar progressB;
    public int numberOfNotesHit;
    public int numberOfNotesMissed;
    public int noteCombo;

    public static GameManager instance;
    public Text notesHitDisplay;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        notesHitDisplay.text = " ";
        numberOfNotesHit = 0;
        numberOfNotesMissed = 0;
        noteCombo = 0;
    }

    // Update is called once per frame
    void Update() {
        if (!startPlaying) {
            if (Input.anyKeyDown) {
                startPlaying = true;
                noteS.hasStarted = true;
                progressB.hasStarted = true;
                progressB.songLength = song.clip.length;
                progressB.fillSpeed = 1f / progressB.songLength;
                song.Play();
            }
        }
    }

    public void NoteHit() {
        Debug.Log("Hit on Time");
        numberOfNotesHit++;
        noteCombo++;
        notesHitDisplay.text = noteCombo.ToString();
    }

    public void NoteMissed() {
        Debug.Log("Missed Note");
        numberOfNotesMissed++;
        noteCombo = 0;
        notesHitDisplay.text = noteCombo.ToString();

    }
}
