using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongCompleted : MonoBehaviour
{
    private int notesHit;
    private int totalNotes;
    private string imageName;

    public Text notesHitText;
    public Text totalNotesText;
    public SpriteRenderer songImage;

    // Start is called before the first frame update
    void Start() {
        notesHit = PlayerPrefs.GetInt("notesHit");
        totalNotes = PlayerPrefs.GetInt("totalNotes");
        imageName = PlayerPrefs.GetString("songImage");
        notesHitText.text = notesHit.ToString();
        totalNotesText.text = totalNotes.ToString();
        songImage.sprite = Resources.Load<Sprite>(imageName);
        DataPersistenceManager.instance.SaveGame();
    }
}
