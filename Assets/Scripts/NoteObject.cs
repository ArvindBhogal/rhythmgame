using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;
    public ParticleSystem explosionEffect;
    public ParticleSystem explosionEffect2;
    public ParticleSystem explosionEffect3;
    public ParticleSystem explosionEffect4;

    // Start is called before the first frame update
    void Start() {
        explosionEffect = GameObject.Find("ParticleExplosion").GetComponent<ParticleSystem>();
        explosionEffect2 = GameObject.Find("ParticleExplosion (1)").GetComponent<ParticleSystem>();
        explosionEffect3 = GameObject.Find("ParticleExplosion (2)").GetComponent<ParticleSystem>();
        explosionEffect4 = GameObject.Find("ParticleExplosion (3)").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(keyToPress)) {
            if (canBePressed) {
                canBePressed = false;
                Destroy(this);
                Destroy(gameObject);
                GameManager.instance.NoteHit();
                OnParticleExplode(keyToPress);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Activator") {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Activator" && canBePressed) {
            canBePressed = false;
            GameManager.instance.NoteMissed();
        }
    }

    private void OnParticleExplode(KeyCode keyToPress) {
        if (keyToPress == KeyCode.L) {
            explosionEffect.Play();
        }
        if (keyToPress == KeyCode.K) {
            explosionEffect2.Play();
        }
        if (keyToPress == KeyCode.F) {
            explosionEffect3.Play();
        }
        if (keyToPress == KeyCode.D) {
            explosionEffect4.Play();
        }
    }
}
