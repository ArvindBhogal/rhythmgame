using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlySelectedObject : MonoBehaviour, IDataPersistence 
{
    public int currentlySelectedSong;

    public int firstSong;
    public int lastSong; 
    public Text songScore;
    public string difficulty;
    private SpriteRenderer songDiamond;
    public static CurrentlySelectedObject instance;
    public SpriteRenderer currentlySelected;
    public AudioSource songPlaying;
    public SelectSong selectedSong;
    public SwitchSongBracket songBracket;
    public ImageController image;
    private int[] songsScoresList;
    public ScrollRect scrollRect;

    public RectTransform songList;   
    private bool isScrolling = false;

    private float elapsedTime;
    
    private float percentageComplete;
    private Vector3 endPosition;
    // Start is called before the first frame update
    void Start() {
        int tmpNumber = PlayerPrefs.GetInt("collectionNumber");

        instance = this;
        currentlySelectedSong = 1;
        if (tmpNumber == 1) {
            currentlySelectedSong = 1;
            firstSong = 1;
        } else if (tmpNumber == 2) {
            currentlySelectedSong = 11;
            firstSong = 11;
        }
        selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
        currentlySelected = GameObject.Find("Currently Selected").GetComponent<SpriteRenderer>();

        songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
        currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);


        songPlaying.clip = selectedSong.clip;
        songPlaying.Play();
        image.OnSwitch();
        difficulty = "normal";
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (instance.currentlySelectedSong == firstSong) {
                Debug.Log("first song");
                return;
            }
            // if (instance.currentlySelectedSong == 1) {
            //     Debug.Log("first song");
            //     return;
            // }
            
            instance.currentlySelectedSong = instance.currentlySelectedSong - 1;
            songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
            // currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
            selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
            currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
            ScrollToSong(songDiamond);
            DataPersistenceManager.instance.LoadGame();
            songPlaying.clip = selectedSong.clip;
            songPlaying.Play();
            image.OnSwitch();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Debug.Log(instance.currentlySelectedSong);
            Debug.Log(songBracket.numberOfSongs);

            if (instance.currentlySelectedSong >= songBracket.numberOfSongs) {
                Debug.Log("last song");
                return;
            }

            instance.currentlySelectedSong = instance.currentlySelectedSong + 1;
            songDiamond = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SpriteRenderer>();
            // currentlySelected.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, songDiamond.transform.localPosition.y, -1);
            selectedSong = GameObject.Find("Song " + instance.currentlySelectedSong).GetComponent<SelectSong>();
            // songList.localPosition = new Vector3(songList.localPosition.x - 330, songList.localPosition.y, songList.localPosition.z);
            currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
            ScrollToSong(songDiamond);
            DataPersistenceManager.instance.LoadGame();
            songPlaying.clip = selectedSong.clip;
            songPlaying.Play();
            image.OnSwitch();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (currentlySelectedSong == 8 && difficulty == "easy" || selectedSong.isLocked) {
                Debug.Log(currentlySelectedSong);
            } else {
                StartCoroutine(selectedSong.DelaySecondLoad(instance.currentlySelectedSong));
            }
        }

        if (isScrolling) {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / 0.8f;
            songList.position = Vector3.Lerp(songList.position, endPosition, percentageComplete);

            if (percentageComplete >= 1.00000000000000001) {
                isScrolling = false;
                percentageComplete = 0f;
                endPosition = Vector3.zero;
                elapsedTime = 0;
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

        if (currentlySelectedSong == 8 && data.storyProgression < 500) {
            songScore.text = "Locked";
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

    public void ScrollToSong(SpriteRenderer songDiamond) {
        // StartCoroutine(ScrollAnimate(songX, songY, songZ));        
        currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
        // endPosition = new Vector3 (songList.anchoredPosition.x + distance, songList.anchoredPosition.y, songList.position.z);


        // I AM ATTEMPTING TO LERP THE SONGLIST RECTTRANSFORM TO THE POSITION OF THE CURRENTLY SELECTED SPRITE, WHICH SHOULD BE AT THE NEXT SONG BY THE TIME WE GET TO THIS PART OF THE CODE

        endPosition = new Vector3 (songList.position.x - currentlySelected.transform.position.x, songList.position.y, songList.position.z);
        Debug.Log(endPosition);




        if (!isScrolling) {
            isScrolling = true;
        } else {
            elapsedTime = 0;
        }
    }

    // public IEnumerator ScrollAnimate(float songX, float songY, float songZ) {
    //     // songList.localPosition = new Vector3(songList.localPosition.x - 330, songList.localPosition.y, songList.localPosition.z);
    //     currentlySelected.transform.position = new Vector3(songDiamond.transform.position.x, songDiamond.transform.position.y, currentlySelected.transform.position.z);
    //     // float targetX = songList.localPosition.x - 330;
    //     // while (songList.localPosition.x !=  targetX) {
    //     //     songList.localPosition = new Vector3(songList.localPosition.x - 0.001f, songList.localPosition.y, songList.localPosition.z);
    //     // }
    //     // yield return new WaitForSeconds(1f);

    // }

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
