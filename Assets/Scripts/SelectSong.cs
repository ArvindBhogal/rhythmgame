using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSong : MonoBehaviour
{

    // private AssetBundle myLoadedAssetBundle; 
    // private string[] scenePaths;
    public int songNumber;
    public GameObject fadeEffect;
    private AudioSource menuTheme;

    // Start is called before the first frame update
    void Start() {
        // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        menuTheme = GameObject.FindGameObjectWithTag("Menu Theme").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        StartCoroutine(DelaySecondLoad(songNumber));
    }
    
    IEnumerator DelaySecondLoad(int songNumber) {
        menuTheme.Stop();
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("song" + songNumber);
    }
}
