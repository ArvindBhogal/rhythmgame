using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSong : MonoBehaviour
{

    public int songNumber;
    public GameObject fadeEffect;
    private SpriteRenderer currentlySelected;
    private SpriteRenderer songDiamond;
    private AudioSource menuTheme;
    public bool songConfirm;
    private CurrentlySelectedObject instance;

    // Start is called before the first frame update
    void Start() {
        menuTheme = GameObject.FindGameObjectWithTag("Menu Theme").GetComponent<AudioSource>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();
        instance = GameObject.Find("Currently Selected").GetComponent<CurrentlySelectedObject>();
        songConfirm = false;
    }

    // Update is called once per frame
    void Update() {
        if (songNumber != instance.currentlySelectedSong) {
            songConfirm = false;
        }
    }

    private void OnMouseDown() {
        if (!songConfirm) {
            songConfirm = true;
            instance.currentlySelectedSong = songNumber;
            songDiamond = GameObject.Find("Song " + songNumber).GetComponent<SpriteRenderer>();
            currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
            Debug.Log(songNumber);
        }
        else {
            Debug.Log(songNumber);
            StartCoroutine(DelaySecondLoad(songNumber));
        }
    }
    
    public IEnumerator DelaySecondLoad(int songNumber) {
        menuTheme.Stop();
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("song" + songNumber);
    }

    
}
