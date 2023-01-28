using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitManager : MonoBehaviour
{

    bool quitConfirm = false;
    public GameObject[] pauseObjects;
    public AudioSource bgm;
    public GameObject fadeEffect;

    void Start() {
        pauseObjects = GameObject.FindGameObjectsWithTag("Show On Pause");
        hideQuit();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !quitConfirm) {
            showQuit();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && quitConfirm) {
            hideQuit();
        }
    }

    public void showQuit() {
        quitConfirm = true;
        foreach(GameObject g in pauseObjects){
			g.SetActive(true);
		}
    }

    public void hideQuit() {
        quitConfirm = false;
        foreach(GameObject g in pauseObjects){
			g.SetActive(false);
		}
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void returnToHub() {
        StartCoroutine(DelaySecondLoad("hub"));
    }

    
    IEnumerator DelaySecondLoad(string sceneName) {
        if (bgm) {
            bgm.Stop();
        }
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);

    }
}
