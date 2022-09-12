using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;
    public float scale;
    public float offset;
    public int numberOfNotes;
    public float additionalSongOffset;

    // Start is called before the first frame update
    void Start() {
        numberOfNotes = 0;
        beatTempo = beatTempo / 60f;
        scale = 5f;
        offset = -(-3f * scale) - -(-3f);
        additionalSongOffset = additionalSongOffset * scale;
        scaleNoteMap(offset);
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        QualitySettings.vSyncCount = 0;
        // Debug.Log(Screen.currentResolution.refreshRate);
    }

    void Update() {
        if (!hasStarted) {
            // if (Input.anyKeyDown) {
            //     hasStarted = true;
            // }
        }
        else {
            // transform.position-= new Vector3(0f, beatTempo * Time.deltaTime * scale, 0f);
            transform.Translate(Vector3.down * Time.deltaTime * beatTempo * scale);
        }
    }

    void scaleNoteMap(float offset) {
        SpriteRenderer[] allNotes = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer go in allNotes) {
            SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
            if (sprite.tag == "Long Note Bridge") {
                sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, (sprite.transform.localPosition.y * scale) + offset + additionalSongOffset, sprite.transform.localPosition.z);
                sprite.transform.localScale = new Vector3(sprite.transform.localScale.x, (sprite.transform.localScale.y * scale), sprite.transform.localScale.z);
            }
            else if (sprite.tag != "Activator") {
                sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, (sprite.transform.localPosition.y * scale) + offset + additionalSongOffset, sprite.transform.localPosition.z);
                numberOfNotes++;
            }
        }
    }
}
