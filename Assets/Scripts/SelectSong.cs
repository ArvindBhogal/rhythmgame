using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSong : MonoBehaviour, IDataPersistence
{

    [SerializeField] public int songNumber;
    public int songScore;
    public Text songScoreText;
    public GameObject fadeEffect;
    private SpriteRenderer currentlySelected;
    private SpriteRenderer songDiamond;
    public AudioSource songPlaying;
    [SerializeField] public AudioClip clip;
    public bool songConfirm;
    private CurrentlySelectedObject instance;
    public Sprite songImage;
    public ImageController image;

    // Start is called before the first frame update
    void Start() {
        // menuTheme = GameObject.FindGameObjectWithTag("Menu Theme").GetComponent<AudioSource>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();
        instance = GameObject.Find("Currently Selected").GetComponent<CurrentlySelectedObject>();
        songConfirm = false;
    }

    // Update is called once per frame
    void Update() {
        if (songNumber != instance.currentlySelectedSong) {
            songConfirm = false; 
        } else {
            songConfirm = true;
        }
    }

    private void OnMouseDown() {
        if (!songConfirm) {
            songConfirm = true;
            instance.currentlySelectedSong = songNumber;
            songDiamond = GameObject.Find("Song " + songNumber).GetComponent<SpriteRenderer>();
            instance.selectedSong = this;
            songPlaying.clip = clip;
            songPlaying.Play();
            image.OnSwitch();
            currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
            DataPersistenceManager.instance.LoadGame();
        }
        else {
            StartCoroutine(DelaySecondLoad(songNumber));
        }
    }
    
    public IEnumerator DelaySecondLoad(int songNumber) {
        songPlaying.Stop();
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("song" + songNumber);
    }

    public void LoadData(GameData data){         

    }


    // note, this only works for single digit songs
    public void SaveData(ref GameData data) {
        // data.scoreSong1 = PlayerPrefs.GetInt("notesHit");
        string tmpName;
        // string tmpMoreMagicShit;
        int nameLength;
        int tmpSong; 
        tmpName = PlayerPrefs.GetString("songName");
        nameLength = tmpName.Length;

        // one time run for adding a song to the dictionary
        if (!data.songList.ContainsKey(songNumber)) {
            data.songList.Add(songNumber, 0);
        }

        if (char.IsDigit(tmpName[nameLength-1])) {

            tmpSong = tmpName[nameLength-1] - '0';

            if (tmpSong == songNumber && data.songList[tmpSong] < PlayerPrefs.GetInt("notesHit")){
                Debug.Log("That one!");
                data.songList.Remove(songNumber);
                data.songList.Add(songNumber, PlayerPrefs.GetInt("notesHit"));
            }
        }

        data.storyProgression = data.songList.Sum(x => x.Value);


        // else {
        //     data.songList.Add(songNumber, PlayerPrefs.GetInt("notesHit"));
        // }
    }

}
