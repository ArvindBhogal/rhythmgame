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
    public AudioSource songPlaying;
    public SelectSong selectedSong;
    public SwitchSongBracket songBracket;
    public ImageController image;
    private int[] songsScoresList;

    public ScrollRect scrollRect;

    public RectTransform songList;   

    // Start is called before the first frame update
    void Start() {
        instance = this;
        currentlySelectedSong = 1;
        selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();
        songPlaying.clip = selectedSong.clip;
        songPlaying.Play();
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
                // songBracket.SwitchBracketLeft();
                instance.currentlySelectedSong = instance.currentlySelectedSong - 1;
                songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
                // currentlySelected.transform.localPosition = new Vector3(648, -324, -1);
                selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
                songList.localPosition = new Vector3(songList.localPosition.x + 330, songList.localPosition.y, songList.localPosition.z);
                currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
                DataPersistenceManager.instance.LoadGame();
                songPlaying.clip = selectedSong.clip;
                songPlaying.Play();
                image.OnSwitch();
                return;
            }
            else {
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
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (instance.currentlySelectedSong % 5 == 0) {
                if (instance.currentlySelectedSong == songBracket.numberOfSongs) {
                    Debug.Log("last song");
                    return;
                }
                Debug.Log("triggered right");
                // songBracket.SwitchBracketRight();
                instance.currentlySelectedSong = instance.currentlySelectedSong + 1;
                songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
                // currentlySelected.transform.localPosition = new Vector3(-648, -324, -1);
                songList.localPosition = new Vector3(songList.localPosition.x - 330, songList.localPosition.y, songList.localPosition.z);
                selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
                DataPersistenceManager.instance.LoadGame();
                currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
                songPlaying.clip = selectedSong.clip;
                songPlaying.Play();
                image.OnSwitch();
                return;
            }
            else {
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
