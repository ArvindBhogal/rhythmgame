using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlySelectedObject : MonoBehaviour, IDataPersistence 
{
    public int currentlySelectedSong;
    public Text songScore;
    public string difficulty;
    private SpriteRenderer songDiamond;
    public static CurrentlySelectedObject instance;
    private SpriteRenderer currentlySelected;
    public AudioSource songPlaying;
    public SelectSong selectedSong;
    public SwitchSongBracket songBracket;
    public ImageController image;
    private int[] songsScoresList;
    public ScrollRect scrollRect;

    public RectTransform songList;   

    // Start is called before the first frame update
    void Start() {
        int tmpNumber = PlayerPrefs.GetInt("collectionNumber");

        instance = this;
        currentlySelectedSong = 1;
        if (tmpNumber == 1) {
            currentlySelectedSong = 1;
        } else if (tmpNumber == 2) {
            currentlySelectedSong = 11;
        }
        selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();
        songPlaying.clip = selectedSong.clip;
        songPlaying.Play();
        image.OnSwitch();
        difficulty = "normal";
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (instance.currentlySelectedSong == 1) {
                Debug.Log("first song");
                return;
            }
            
            instance.currentlySelectedSong = instance.currentlySelectedSong - 1;
            songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
            // currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
            selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
            songList.localPosition = new Vector3(songList.localPosition.x + 330, songList.localPosition.y, songList.localPosition.z);
            currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
            DataPersistenceManager.instance.LoadGame();
            songPlaying.clip = selectedSong.clip;
            songPlaying.Play();
            image.OnSwitch();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Debug.Log(instance.currentlySelectedSong);
            Debug.Log(songBracket.numberOfSongs);
            if (instance.currentlySelectedSong == songBracket.numberOfSongs) {
                Debug.Log("last song");
                return;
            }

            instance.currentlySelectedSong = instance.currentlySelectedSong + 1;
            songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
            // currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
            selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
            songList.localPosition = new Vector3(songList.localPosition.x - 330, songList.localPosition.y, songList.localPosition.z);
            currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
            DataPersistenceManager.instance.LoadGame();
            songPlaying.clip = selectedSong.clip;
            songPlaying.Play();
            image.OnSwitch();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (currentlySelectedSong == 8 && difficulty == "easy") {
                Debug.Log(currentlySelectedSong);
            } else {
                StartCoroutine(selectedSong.DelaySecondLoad(instance.currentlySelectedSong));
            }
        }
    }

    public void LoadData(GameData data){ 
        int tmpScore;
        int tmpSong;

        if (currentlySelectedSong == 8 && difficulty == "easy") {
            songScore.text = "This song is only available on Normal";
            return;
        }

        if (difficulty == "easy") {
            tmpSong = currentlySelectedSong + 1000;
        } else {
            tmpSong = currentlySelectedSong;
        }

        Debug.Log("Load Run = " + currentlySelectedSong);

        Debug.Log(tmpSong);

        data.songList.TryGetValue(tmpSong, out tmpScore);

        
        
        if (tmpScore >= 0) {
            songScore.text = tmpScore.ToString();
        }


    }

    public void SaveData(ref GameData data) {
        // string tmpName;
        // int nameLength;
        // int tmpSong; 
        // tmpName = PlayerPrefs.GetString("songName");
        // nameLength = tmpName.Length;

        // tmpSong = tmpName[nameLength-1] - '0';
        // if (data.songList.ContainsKey(tmpSong)) {
        //     data.songList.Remove(tmpSong);
        // }
        // data.songList.Add(tmpSong, PlayerPrefs.GetInt("notesHit"));

        // else {
        //     data.songList.Add(songNumber, PlayerPrefs.GetInt("notesHit"));
        // }
    }

}
