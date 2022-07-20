using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlySelectedObject : MonoBehaviour
{
    public int currentlySelectedSong;
    public static CurrentlySelectedObject instance;
    // Start is called before the first frame update
    void Start() {
        instance = this;
        currentlySelectedSong = 1;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
