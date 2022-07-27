using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongCompleted : MonoBehaviour
{
    private int notesHit;
    private int totalNotes;

    public Text notesHitText;
    public Text totalNotesText;

    // Start is called before the first frame update
    void Start() {
        notesHit = PlayerPrefs.GetInt("notesHit");
        totalNotes = PlayerPrefs.GetInt("totalNotes");
        notesHitText.text = notesHit.ToString();
        totalNotesText.text = totalNotes.ToString();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
