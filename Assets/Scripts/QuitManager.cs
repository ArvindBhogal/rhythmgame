using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitManager : MonoBehaviour
{

    bool quitConfirm = false;
    public GameObject[] pauseObjects;
    public AudioSource bgm;
    public GameObject fadeEffect;
    public CurrentlySelectedObject instance;

    public ParticleSystem particles;

    public SpriteRenderer background; 
    public Text songScore;

    private Coroutine fade;

    void Start() {
        pauseObjects = GameObject.FindGameObjectsWithTag("Show On Pause");
        hideQuit();
        PlayerPrefs.SetString("difficulty", "normal");
        
    }

    void Update() {
        if (instance == null) {
            instance = GameObject.Find("Currently Selected").GetComponent<CurrentlySelectedObject>();
        }

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

    public void switchToEasy() {
        var main = particles.main;
        main.startColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        PlayerPrefs.SetString("difficulty", "easy");
        instance.difficulty = "easy";
        DataPersistenceManager.instance.LoadGame();
        startFadeInEasy();
    }

    public void switchToNormal() {
        var main = particles.main;
        main.startColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        PlayerPrefs.SetString("difficulty", "normal");
        instance.difficulty = "normal";
        DataPersistenceManager.instance.LoadGame();
        startFadeInNormal();
    }

    public void startFadeInEasy() {
        if (fade != null) {
            return;
        }
        fade = StartCoroutine(FadeEasy());
    }

    public void startFadeInNormal() {
        if (fade != null) {
            return;
        }
        fade = StartCoroutine(FadeNormal());
    }

    IEnumerator FadeEasy() {
        float target = 0.5f;
        Color curColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        while(curColor.r > target+0.01f && curColor.b > target+0.01f) {
            curColor.r = Mathf.Lerp(curColor.r, target, 10f * Time.deltaTime);
            curColor.b = Mathf.Lerp(curColor.b, target, 10f * Time.deltaTime);
            yield return new WaitForSeconds(0.0025f);
            background.color = curColor;
        }
        curColor.r = 0.5f;
        curColor.b = 0.5f;
        background.color = curColor;
        Debug.Log("Done");  
        fade = null;
    }

    IEnumerator FadeNormal() {
        float target = 1.0f;
        Color curColor = background.color;
        while(curColor.r < target-0.01f && curColor.b < target-0.01f) {
            curColor.r = Mathf.Lerp(curColor.r, target, 10f * Time.deltaTime);
            curColor.b = Mathf.Lerp(curColor.b, target, 10f * Time.deltaTime);
            yield return new WaitForSeconds(0.0025f);
            background.color = curColor;
        }
        curColor.r = 1f;
        curColor.b = 1f;
        background.color = curColor;
        Debug.Log("Done");  
        fade = null;
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
