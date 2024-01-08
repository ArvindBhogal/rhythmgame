using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    public CurrentlySelectedObject instance;
    public SpriteRenderer songImage;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnSwitch() {
        Debug.Log(instance.selectedSong.songImage);
        try {
            songImage.sprite = instance.selectedSong.songImage;
            PlayerPrefs.SetString("songImage", songImage.sprite.name);
        } catch {
            songImage.sprite = null;
        }
        
        
    }

}
