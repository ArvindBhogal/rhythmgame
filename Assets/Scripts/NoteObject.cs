using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class NoteObject : MonoBehaviour
{
    public SpriteRenderer visuals;
    public bool canBePressed;
    public KeyCode keyToPress;
    public ParticleSystem explosionEffect;
    public ParticleSystem explosionEffect2;
    public ParticleSystem explosionEffect3;
    public ParticleSystem explosionEffect4;
    KoreographyEvent trackedEvent;
    LaneController laneController;
    GameManager gameManager;

    static Vector3 Lerp(Vector3 from, Vector3 to, float t)
    {
        return new Vector3 (from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
    }

    // Start is called before the first frame update
    void Start() {
        explosionEffect = GameObject.Find("ParticleExplosion").GetComponent<ParticleSystem>();
        explosionEffect2 = GameObject.Find("ParticleExplosion (1)").GetComponent<ParticleSystem>();
        explosionEffect3 = GameObject.Find("ParticleExplosion (2)").GetComponent<ParticleSystem>();
        explosionEffect4 = GameObject.Find("ParticleExplosion (3)").GetComponent<ParticleSystem>();
    }

    public void Initialize(KoreographyEvent evt, LaneController laneCont, GameManager manager) {
        trackedEvent = evt;
        laneController = laneCont;
        gameManager = manager;

        UpdatePosition();
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetKeyDown(keyToPress)) {
        //     if (canBePressed) {
        //         canBePressed = false;
        //         Destroy(this);
        //         Destroy(gameObject);
        //         GameManager.instance.NoteHit();
        //         OnParticleExplode(keyToPress);
        //     }
        // }

        UpdateHeight();

        UpdatePosition();

        if (transform.position.y <= laneController.DespawnY)
        {
            gameManager.ReturnNoteObjectToPool(this);
            Reset();
        }
    }

    void Reset()
    {
        trackedEvent = null;
        laneController = null;
        gameManager = null;
    }

    // Updates the height of the Note Object.  This is relative to the speed at which the notes fall and 
    //  the specified Hit Window range.
    void UpdateHeight()
    {
        // float baseUnitHeight = visuals.sprite.rect.height / visuals.sprite.pixelsPerUnit;
        // float targetUnitHeight = gameManager.WindowSizeInUnits * 2f;	// Double it for before/after.

        Vector3 scale = transform.localScale;
        scale.y = 0.20f;	
        scale.x = 0.20f;
        transform.localScale = scale;
    }

    // Updates the position of the Note Object along the Lane based on current audio position.
    void UpdatePosition()
    {
        // Get the number of samples we traverse given the current speed in Units-Per-Second.
        float samplesPerUnit = gameManager.SampleRate / gameManager.noteSpeed;

        // Our position is offset by the distance from the target in world coordinates.  This depends on
        //  the distance from "perfect time" in samples (the time of the Koreography Event!).
        Vector3 pos = laneController.TargetPosition;
        pos.y -= (gameManager.DelayedSampleTime - trackedEvent.StartSample) / samplesPerUnit;
        transform.position = pos;
    }

    public bool IsNoteHittable()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gameManager.DelayedSampleTime;
        int hitWindow = gameManager.HitWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    // Checks to see if the note is no longer hittable based on the configured hit window width in
    //  samples.
    public bool IsNoteMissed()
    {
        bool bMissed = true;

        if (enabled)
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = gameManager.DelayedSampleTime;
            int hitWindow = gameManager.HitWindowSampleWidth;

            bMissed = (curTime - noteTime > hitWindow);
        }
        
        return bMissed;
    }

    // Returns this Note Object to the pool which is controlled by the Rhythm Game Controller.  This
    //  helps reduce runtime allocations.
    void ReturnToPool()
    {
        gameManager.ReturnNoteObjectToPool(this);
        Reset();
    }

    // Performs actions when the Note Object is hit.
    public void OnHit()
    {
        OnParticleExplode(laneController.keyboardButton);
        gameManager.NoteHit();
        ReturnToPool();
    }

    // Performs actions when the Note Object is cleared.
    public void OnClear()
    {
        ReturnToPool();
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (other.tag == "Activator") {
    //         canBePressed = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.tag == "Activator" && canBePressed) {
    //         canBePressed = false;
    //         GameManager.instance.NoteMissed();
    //     }
    // }

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
