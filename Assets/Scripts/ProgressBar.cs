using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;

    private float songProgress;
    public float fillSpeed;
    public bool hasStarted; 
    public float songLength;
    public GameObject fadeEffect;

    private void Awake() {
        slider = gameObject.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start() {
        incrementProgress(1);
    }

    // Update is called once per frame
    void Update() {
        if (!hasStarted) {
            // Literally nothing right now
        }
        else {
            if (slider.value < songProgress) {
                slider.value += fillSpeed * Time.deltaTime;
            }

            if (slider.value == 1) {
                StartCoroutine(DelaySecondLoad("songComplete"));
            }
        }
    }

    public void incrementProgress(float newProgress) {
        songProgress = slider.value + newProgress;
    }

    IEnumerator DelaySecondLoad(string sceneName) {
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);

    }
}
