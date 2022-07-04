using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteObject : MonoBehaviour
{

    public bool canBePressed;
    public bool currentlyPressed;
    public bool canBeReleased;
    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start() {
        canBePressed = false;
        currentlyPressed = false;
        canBeReleased = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(keyToPress)) {
            if (canBePressed) {
                currentlyPressed = true;
                GameManager.instance.NoteHit();
            }
        }

        if (Input.GetKeyUp(keyToPress) && currentlyPressed) {
            if (!canBeReleased) {
                GameManager.instance.NoteMissed();
            }
            else {
                GameManager.instance.NoteHit();
            }
            currentlyPressed = false;
            Destroy(this);
            Destroy(gameObject);
        }
    }
}
