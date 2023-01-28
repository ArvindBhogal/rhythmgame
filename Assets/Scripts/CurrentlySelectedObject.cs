using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlySelectedObject : MonoBehaviour, IDataPersistence 
{
    public int currentlySelectedSong;
    public Text songScore;
    private SpriteRenderer songDiamond;
    public static CurrentlySelectedObject instance;
    private SpriteRenderer currentlySelected;
    public SelectSong selectedSong;
    public SwitchSongBracket songBracket;
    public ImageController image;
    private int[] songsScoresList;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        currentlySelectedSong = 1;
        selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();
        songBracket = this.GetComponent<SwitchSongBracket>();
        image.OnSwitch();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (instance.currentlySelectedSong % 5 == 1) {
                if (instance.currentlySelectedSong == 1) {
                    Debug.Log("first song");
                    return;
                }
                Debug.Log("triggered left");
                songBracket.SwitchBracketLeft();
                instance.currentlySelectedSong = instance.currentlySelectedSong - 1;
                currentlySelected.transform.localPosition = new Vector3(648, -324, -1);
                selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
                DataPersistenceManager.instance.LoadGame();
                image.OnSwitch();
                return;
            }
            else {
                instance.currentlySelectedSong = instance.currentlySelectedSong - 1;
                songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
                currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
                selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
                DataPersistenceManager.instance.LoadGame();
                image.OnSwitch();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (instance.currentlySelectedSong % 5 == 0) {
                if (instance.currentlySelectedSong == songBracket.numberOfSongs) {
                    Debug.Log("last song");
                    return;
                }
                Debug.Log("triggered right");
                songBracket.SwitchBracketRight();
                instance.currentlySelectedSong = instance.currentlySelectedSong + 1;
                currentlySelected.transform.localPosition = new Vector3(-648, -324, -1);
                selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
                DataPersistenceManager.instance.LoadGame();
                image.OnSwitch();
                return;
            }
            else {
                instance.currentlySelectedSong = instance.currentlySelectedSong + 1;
                songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
                currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
                selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
                DataPersistenceManager.instance.LoadGame();
                image.OnSwitch();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(selectedSong.DelaySecondLoad(instance.currentlySelectedSong));
        }
    }

    public void LoadData(GameData data){ 
        int tmpScore;
        data.songList.TryGetValue(currentlySelectedSong, out tmpScore);
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
