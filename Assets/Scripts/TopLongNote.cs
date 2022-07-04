using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLongNote : MonoBehaviour
{
    public LongNoteObject longN;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(longN.keyToPress)) {
            
            // if (longN.canBeReleased) {
            //     longN.currentlyPressed = false;
            //     longN.canBeReleased = false;
            //     longN.canBePressed = false;
            //     Destroy(this);
            //     Destroy(gameObject);
            // } 
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Activator") {
            longN.canBePressed = false;
            longN.canBeReleased = true;
            if (!longN.currentlyPressed && longN.canBeReleased) {
                GameManager.instance.NoteMissed();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Activator") {
            longN.canBeReleased = false;
            if (longN.currentlyPressed && !longN.canBeReleased) {
                GameManager.instance.NoteMissed();
            }
            Destroy(gameObject);
        }
    }
}
