using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSong : MonoBehaviour
{

    // private AssetBundle myLoadedAssetBundle; 
    // private string[] scenePaths;
    public int songNumber;

    // Start is called before the first frame update
    void Start() {
        // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        SceneManager.LoadScene("song" + songNumber);
    }


}
