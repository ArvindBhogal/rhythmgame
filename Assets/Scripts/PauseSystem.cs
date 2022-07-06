using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    public GameObject[] pauseObjects;
    private GameManager instance;

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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("songSelect");
    }
}
