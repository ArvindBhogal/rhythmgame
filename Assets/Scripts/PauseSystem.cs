using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SonicBloom.Koreo;

public class PauseSystem : MonoBehaviour
{
    public GameObject[] pauseObjects;
    private GameManager instance;
    public GameObject fadeEffect;
    public float globalOffset;
    public Text globalOffsetDisplay; 
    public int noteSpeed;
    public Text noteSpeedDisplay;

    public float originalTime;

    public Slider globalOffsetSlider;

    // Start is called before the first frame update
    void Start() {
        instance = gameObject.GetComponent<GameManager>();
        pauseObjects = GameObject.FindGameObjectsWithTag("Show On Pause");
        globalOffsetSlider.onValueChanged.AddListener(delegate {setNewOffset();});
        if (instance) {
            hidePaused();
        }
    }

    // Update is called once per frame
    void Update() {
        if (instance) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (Time.timeScale == 1) {
                    showPaused();
                }
                else if (Time.timeScale == 0) {
                    hidePaused();
                }   
            }    
        }
    }

    public void showPaused(){
        globalOffset = PlayerPrefs.GetFloat("GlobalOffset", 0);
        Debug.Log(globalOffset + " globalOffset");
        originalTime = Koreographer.Instance.EventDelayInSeconds - globalOffset;
        Debug.Log(originalTime + " originalTime");
        globalOffsetDisplay.text = globalOffset.ToString();
        Debug.Log("Paused");


        noteSpeed = PlayerPrefs.GetInt("NoteSpeed", (int)instance.noteSpeed);
        noteSpeedDisplay.text = noteSpeed.ToString();

        globalOffsetSlider.value = globalOffset;

		foreach(GameObject g in pauseObjects){
            Time.timeScale = 0;
			g.SetActive(true);
            instance.song.Pause();
		}
	}

    public void hidePaused(){
        Debug.Log ("Resumed");
		foreach(GameObject g in pauseObjects){
			Time.timeScale = 1;
			g.SetActive(false);
            instance.song.UnPause();
		}
	}

    public void setNewOffset() {
        globalOffsetSlider.value = MathF.Round(globalOffsetSlider.value, 3);

        Koreographer.Instance.EventDelayInSeconds = originalTime + globalOffsetSlider.value;
        PlayerPrefs.SetFloat("GlobalOffset", globalOffset);

        globalOffset = globalOffsetSlider.value;

        Debug.Log(Koreographer.Instance.EventDelayInSeconds + "Koreo");
		globalOffsetDisplay.text = globalOffsetSlider.value.ToString();
    }

    public void Restart() {
        hidePaused();
        StartCoroutine(DelaySecondLoad(SceneManager.GetActiveScene().name));
    }

    public void Replay() {
        StartCoroutine(DelaySecondLoad(PlayerPrefs.GetString("songName")));
    }

    public void returnToMenu() {
        hidePaused();
        StartCoroutine(DelaySecondLoad("songSelect"));
    }

    public void increaseGlobalOffset() {
        globalOffset = globalOffset + 0.001f;
        globalOffsetSlider.value += 0.001f;

        if (globalOffset > 0.1f) {
            globalOffset = 0.1f;
        }

        globalOffset = (float)Math.Round(globalOffset, 3);
        PlayerPrefs.SetFloat("GlobalOffset", globalOffset);
        globalOffsetDisplay.text = globalOffset.ToString();

        Koreographer.Instance.EventDelayInSeconds = originalTime + globalOffset;
        Debug.Log(Koreographer.Instance.EventDelayInSeconds + "Koreo");
    }

    public void decreaseGlobalOffset() {
        globalOffset = globalOffset - 0.001f;
        globalOffsetSlider.value -= 0.001f;

        if (Koreographer.Instance.EventDelayInSeconds <= 0) {
            globalOffset = globalOffset + 0.001f;
            Debug.Log("Can go no further" + Koreographer.Instance.EventDelayInSeconds);
            return;
        }
        if (globalOffset < -0.1f) {
            globalOffset = -0.1f;
        }
        globalOffset = (float)Math.Round(globalOffset, 3);
        PlayerPrefs.SetFloat("GlobalOffset", globalOffset);
        globalOffsetDisplay.text = globalOffset.ToString();

        Koreographer.Instance.EventDelayInSeconds = originalTime + globalOffset;
        Debug.Log(Koreographer.Instance.EventDelayInSeconds + "Koreo");
    }

    public void increaseNoteSpeed() {
        noteSpeed += 1;
        if (noteSpeed < 1) {
            noteSpeed = 1;
            return;
        } 

        if (noteSpeed > 15) {
            noteSpeed = 15;
            return;
        }

        PlayerPrefs.SetInt("NoteSpeed", noteSpeed);
        noteSpeedDisplay.text = noteSpeed.ToString();
        instance.noteSpeed += 1f;
    }

        public void decreaseNoteSpeed() {
        noteSpeed -= 1;
        if (noteSpeed < 1) {
            noteSpeed = 1;
            return;
        } 

        if (noteSpeed > 15) {
            noteSpeed = 15;
            return;
        }

        PlayerPrefs.SetInt("NoteSpeed", noteSpeed);
        noteSpeedDisplay.text = noteSpeed.ToString();
        instance.noteSpeed -= 1f;
    }



    IEnumerator DelaySecondLoad(string sceneName) {
        if (instance) {
            instance.song.Stop();
        }
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);

    }
}
