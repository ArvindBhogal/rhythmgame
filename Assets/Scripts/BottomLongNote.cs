using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLongNote : MonoBehaviour
{
    public LongNoteObject longN;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetKeyDown(longN.keyToPress)) {
        //     if (longN.canBePressed) {
        //         Destroy(this);
        //         Destroy(gameObject);
        //     } 
        // }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Activator") {
            longN.canBePressed = true;
            longN.canBeReleased = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Activator" && longN.canBePressed) {
            longN.canBePressed = false;
            if (!longN.currentlyPressed) {
                GameManager.instance.NoteMissed();
            }
            Destroy(gameObject);
        }
    }
}
