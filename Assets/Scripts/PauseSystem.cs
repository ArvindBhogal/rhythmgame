using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    public GameObject[] pauseObjects;
    private GameManager instance;
    public GameObject fadeEffect;

    // Start is called before the first frame update
    void Start() {
        instance = gameObject.GetComponent<GameManager>();
        pauseObjects = GameObject.FindGameObjectsWithTag("Show On Pause");
        hidePaused();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale == 1) {
                showPaused();
            }
            else if (Time.timeScale == 0) {
			    hidePaused();
		    }   
        } 
    }

    public void showPaused(){
		foreach(GameObject g in pauseObjects){
            Time.timeScale = 0;
            Debug.Log("Paused");
			g.SetActive(true);
            instance.song.Pause();
		}
	}

    public void hidePaused(){
		foreach(GameObject g in pauseObjects){
            Debug.Log ("Resumed");
			Time.timeScale = 1;
			g.SetActive(false);
            instance.song.UnPause();
		}
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

    IEnumerator DelaySecondLoad(string sceneName) {
        if (instance) {
            instance.song.Stop();
        }
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);

    }
}
