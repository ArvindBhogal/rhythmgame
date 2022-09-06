using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject fadeEffect;

    public void PlayGame() {
        StartCoroutine(DelaySecondLoad());
    }

    public IEnumerator DelaySecondLoad() {
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("songSelect");
    }
}
