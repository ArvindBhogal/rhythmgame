using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class NoteObject : MonoBehaviour
{
    public SpriteRenderer visuals;
    public bool canBePressed;
    public bool currentlyPressed;
    public bool canBeReleased;
    public KeyCode keyToPress;
    public ParticleSystem explosionEffect;
    public ParticleSystem explosionEffect2;
    public ParticleSystem explosionEffect3;
    public ParticleSystem explosionEffect4;

    public ParticleSystem heldEffect;
    public ParticleSystem heldEffect2;
    public ParticleSystem heldEffect3;
    public ParticleSystem heldEffect4;
    KoreographyEvent trackedEvent;
    LaneController laneController;
    GameManager gameManager;
    public bool noteSpan;

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

        heldEffect = GameObject.Find("ParticleHeld").GetComponent<ParticleSystem>();
        heldEffect2 = GameObject.Find("ParticleHeld (1)").GetComponent<ParticleSystem>();
        heldEffect3 = GameObject.Find("ParticleHeld (2)").GetComponent<ParticleSystem>();
        heldEffect4 = GameObject.Find("ParticleHeld (3)").GetComponent<ParticleSystem>();
    }

    public void Initialize(KoreographyEvent evt, LaneController laneCont, GameManager manager) {
        trackedEvent = evt;
        laneController = laneCont;
        gameManager = manager;

        if (!trackedEvent.IsOneOff()) {
            visuals.sprite = gameManager.holdObjectArchytype.GetComponent<SpriteRenderer>().sprite;
            visuals.drawMode = SpriteDrawMode.Sliced;
            noteSpan = true;
        } else {
            visuals.sprite = gameManager.noteObjectArchetype.GetComponent<SpriteRenderer>().sprite;
            visuals.drawMode = SpriteDrawMode.Simple;
            noteSpan = false;
        }

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

        if (IsNoteHittable()) {
            canBePressed = true;
        } else {
            canBePressed = false;
        }

        if (IsNoteReleasable()) {
            canBeReleased = true;
        } else {
            canBeReleased = false;
        }

        // if (Input.GetKeyUp(keyToPress) && currentlyPressed) {
        //     if (!IsNoteHittable()) {

        //         GameManager.instance.NoteMissed();
        //     }
        //     else {
        //         GameManager.instance.NoteHit();
        //     }
        //     currentlyPressed = false;
        //     OnParticleLetGo(keyToPress);
        // }
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
        float baseUnitHeight = visuals.sprite.rect.height / visuals.sprite.pixelsPerUnit;
        float fullHeight = gameManager.GetVerticalUnitOffsetForSampleTime(trackedEvent.StartSample) - gameManager.GetVerticalUnitOffsetForSampleTime(trackedEvent.EndSample);

        Vector3 scale = transform.localScale;

        Vector2 size = visuals.size;

        if (noteSpan == true) {
            size.x = 0.37f;
            size.y = (fullHeight / baseUnitHeight) / 5.15625f;
        }
        scale.y = 1.65f;
        scale.x = 1.65f;
        
        transform.localScale = scale;
        visuals.size = size;
    }

    // Updates the position of the Note Object along the Lane based on current audio position.
    void UpdatePosition()
    {
        // Get the number of samples we traverse given the current speed in Units-Per-Second.
        float fullHeight = 0.5f * (gameManager.GetVerticalUnitOffsetForSampleTime(trackedEvent.StartSample) - gameManager.GetVerticalUnitOffsetForSampleTime(trackedEvent.EndSample));
        // Our position is offset by the distance from the target in world coordinates.  This depends on
        //  the distance from "perfect time" in samples (the time of the Koreography Event!).
        Vector3 pos = laneController.TargetPosition;
        pos.y -= gameManager.GetVerticalUnitOffsetForSampleTime(trackedEvent.StartSample) - fullHeight;
        transform.position = pos;
    }

    public bool IsNoteHittable()
    {
        bool bHittable = false;

        if (trackedEvent.IsOneOff()) {
            int noteTime = trackedEvent.StartSample;
            int curTime = gameManager.DelayedSampleTime;
            int hitWindow = gameManager.HitWindowSampleWidth;

            bHittable = Mathf.Abs(noteTime - curTime) <= hitWindow;
        } else {
            int startTime = trackedEvent.StartSample;
            int curTime = gameManager.DelayedSampleTime;
            int hitWindow = gameManager.HitWindowSampleWidth;

            bHittable = Mathf.Abs(startTime - curTime) <= hitWindow;

        }

        return bHittable;
    }

    public bool IsNoteReleasable() {
        bool bReleasable = false;

        int endTime = trackedEvent.EndSample;
        int curTime = gameManager.DelayedSampleTime;
        int hitWindow = gameManager.HitWindowSampleWidth;

        bReleasable = Mathf.Abs(endTime - curTime) <= hitWindow;

        return bReleasable;
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
        if (!noteSpan) {
            OnParticleExplode(laneController.keyboardButton);
            gameManager.NoteHit();
            ReturnToPool();
        } else {
            gameManager.NoteHit();
            currentlyPressed = true;
            OnParticleHeld(laneController.keyboardButton);
        }
    }

    public void OnRelease(bool result) {
        if (result) {
            gameManager.NoteHit();
            currentlyPressed = false;
            OnParticleLetGo(laneController.keyboardButton);
        } else {
            gameManager.NoteMissed();
            currentlyPressed = false;
            OnParticleLetGo(laneController.keyboardButton);
        }
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

    private void OnParticleHeld(KeyCode keyToPress) {
        if (keyToPress == KeyCode.L) {
            heldEffect.Play();
        }
        if (keyToPress == KeyCode.K) {
            heldEffect2.Play();
        }
        if (keyToPress == KeyCode.F) {
            heldEffect3.Play();
        }
        if (keyToPress == KeyCode.D) {
            heldEffect4.Play();
        }
    }

    private void OnParticleLetGo(KeyCode keyToPress) {
        if (keyToPress == KeyCode.L) {
            heldEffect.Stop();
        }
        if (keyToPress == KeyCode.K) {
            heldEffect2.Stop();
        }
        if (keyToPress == KeyCode.F) {
            heldEffect3.Stop();
        }
        if (keyToPress == KeyCode.D) {
            heldEffect4.Stop();
        }
    }
}
