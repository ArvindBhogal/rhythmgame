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

    private int modifiedSongNumber;
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

    private bool isScrolling = false;

    private float elapsedTime;
    private Vector3 endPosition;

    public RectTransform songList;   

    private float percentageComplete;

    // Start is called before the first frame update
    void Start() {
        // menuTheme = GameObject.FindGameObjectWithTag("Menu Theme").GetComponent<AudioSource>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();
        instance = GameObject.Find("Currently Selected").GetComponent<CurrentlySelectedObject>();
        songConfirm = false;
        Debug.Log(PlayerPrefs.GetInt("collectionNumber"));
        songList = instance.songList;
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
            // Debug.Log(songDiamond.transform.localPosition.x);
            // Debug.Log(instance.transform.localPosition.x);
            instance.ScrollToSong(songDiamond);
            DataPersistenceManager.instance.LoadGame();
        }
        else {
            if (songNumber == 8 && PlayerPrefs.GetString("difficulty") == "easy") {
                Debug.Log(songNumber);
            } else {
                StartCoroutine(DelaySecondLoad(songNumber));
            }
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


    public void SaveData(ref GameData data) {
        // data.scoreSong1 = PlayerPrefs.GetInt("notesHit");
        string tmpName;
        // string tmpMoreMagicShit;
        string difficulty = PlayerPrefs.GetString("difficulty");
        int nameLength;
        int tmpSong; 
        tmpName = PlayerPrefs.GetString("songName");
        nameLength = tmpName.Length;
        Debug.Log("Running Save");

        if (difficulty == "easy") {
            modifiedSongNumber = songNumber + 1000;
        }  else if (difficulty == "normal") {
            modifiedSongNumber = songNumber;
        }

        // one time run for adding a song to the dictionary
        if (!data.songList.ContainsKey(modifiedSongNumber)) {
            data.songList.Add(modifiedSongNumber, 0);
        }

        if (char.IsDigit(tmpName[nameLength-1])) {

            if (nameLength == 6) {
                char[] chars = {tmpName[nameLength-2], tmpName[nameLength-1]};
                string tmpString = new String(chars);
                tmpSong = Int32.Parse(tmpString);
            } else {
                tmpSong = tmpName[nameLength-1] - '0';
            }

            if (difficulty == "easy") {
                tmpSong = tmpSong + 1000;
            } else if (difficulty == "normal") {
                // tmpSong = songNumber;
            }


            if (tmpSong == modifiedSongNumber && data.songList[tmpSong] < PlayerPrefs.GetInt("notesHit")) {
                Debug.Log("That one!");
                data.songList.Remove(modifiedSongNumber);
                data.songList.Add(modifiedSongNumber, PlayerPrefs.GetInt("notesHit"));
                PlayerPrefs.SetInt("notesHit", 0);
            }
        }

        data.storyProgression = data.songList.Sum(x => x.Value);


        // else {
        //     data.songList.Add(songNumber, PlayerPrefs.GetInt("notesHit"));
        // }
    }

}
