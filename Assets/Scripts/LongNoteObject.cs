using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteObject : MonoBehaviour
{

    public bool canBePressed;
    public bool currentlyPressed;
    public bool canBeReleased;
    public KeyCode keyToPress;

    public ParticleSystem particleHeld;
    public ParticleSystem particleHeld2;
    public ParticleSystem particleHeld3;
    public ParticleSystem particleHeld4;

    // Start is called before the first frame update
    void Start() {
        canBePressed = false;
        currentlyPressed = false;
        canBeReleased = false;

        particleHeld = GameObject.Find("ParticleHeld").GetComponent<ParticleSystem>();
        particleHeld2 = GameObject.Find("ParticleHeld (1)").GetComponent<ParticleSystem>();
        particleHeld3 = GameObject.Find("ParticleHeld (2)").GetComponent<ParticleSystem>();
        particleHeld4 = GameObject.Find("ParticleHeld (3)").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(keyToPress)) {
            if (canBePressed) {
                currentlyPressed = true;
                GameManager.instance.NoteHit();
                OnParticleHold(keyToPress);
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
            OnParticleLetGo(keyToPress);
            Destroy(this);
            Destroy(gameObject);
        }
    }

    private void OnParticleHold(KeyCode keyToPress) {
        if (keyToPress == KeyCode.L) {
            particleHeld.Play();
        }
        if (keyToPress == KeyCode.K) {
            particleHeld2.Play();
        }
        if (keyToPress == KeyCode.F) {
            particleHeld3.Play();
        }
        if (keyToPress == KeyCode.D) {
            particleHeld4.Play();
        }
    }

    private void OnParticleLetGo(KeyCode keyToPress) {
        if (keyToPress == KeyCode.L) {
            particleHeld.Stop();
        }
        if (keyToPress == KeyCode.K) {
            particleHeld2.Stop();
        }
        if (keyToPress == KeyCode.F) {
            particleHeld3.Stop();
        }
        if (keyToPress == KeyCode.D) {
            particleHeld4.Stop();
        }
    }
}
