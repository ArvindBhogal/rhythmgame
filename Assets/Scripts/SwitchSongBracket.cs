using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwitchSongBracket : MonoBehaviour
{

    private GameObject[] songList;
    public GameObject[] collectionList;
    public GameObject currentlySelected;
    public CurrentlySelectedObject currentlySelectedObj;
    public List<GameObject> songBracket;
    public List<GameObject> tmpBracket;
    public int numberOfSongs;

    // Start is called before the first frame update
    void Start() {

        int tmpNumber = PlayerPrefs.GetInt("collectionNumber");

        currentlySelected = GameObject.FindWithTag("Activator");

        if (tmpNumber == 1) {
            // currentlySelected.currentlySelectedSong = 1;
            for (int i = 0; i < collectionList.Length; i++) {
                if (i != tmpNumber - 1) {
                    collectionList[i].SetActive(false);
                }
            }
            collectionList[0].SetActive(true);
            currentlySelected.transform.SetParent(collectionList[0].transform);

        } else if (tmpNumber == 2) {
            // currentlySelected.currentlySelectedSong = 11;
            for (int i = 0; i < collectionList.Length; i++) {
                if (i != tmpNumber - 1) {
                    collectionList[i].SetActive(false);
                }
            }
            collectionList[1].SetActive(true);
            currentlySelected.transform.SetParent(collectionList[1].transform);
        }

        

        


        currentlySelectedObj = currentlySelected.GetComponent<CurrentlySelectedObject>();

        if (tmpNumber == 1) {
            // currentlySelectedObj.currentlySelectedSong = 1;
        } else if (tmpNumber == 2) {
            // currentlySelectedObj.currentlySelectedSong = 11;
        }

        songList = GameObject.FindGameObjectsWithTag("Song Crystal");
        numberOfSongs = songList.Length;
        songBracket = new List<GameObject>();
        Debug.Log(numberOfSongs);
        for (int i = 0; i < numberOfSongs; i++) {
            songBracket.Add(songList[i]);
            Debug.Log(songBracket[i].name);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SwitchBracketLeft() {
        tmpBracket = new List<GameObject>(songBracket);
        GameObject tmp = songBracket[0];
        SpriteRenderer ogSongDiamond = tmp.GetComponent<SpriteRenderer>(); 
        float tmpPosition = ogSongDiamond.transform.localPosition.y;

        songBracket.Clear();
        for (int k = currentlySelectedObj.currentlySelectedSong - 2; k > currentlySelectedObj.currentlySelectedSong - 7; k--) {
            songBracket.Add(songList[k]);
        }

        songBracket = songBracket.OrderBy(p => p.name).ToList();

        GameObject tmp2 = tmpBracket[0];
        

        foreach (GameObject go in tmpBracket) {
            SpriteRenderer tmpSongDiamond = go.GetComponent<SpriteRenderer>();
            tmpSongDiamond.transform.localPosition = new Vector3(tmpSongDiamond.transform.localPosition.x, -800, -2f);
        }

        foreach (GameObject go2 in songBracket) {
            SpriteRenderer songDiamond = go2.GetComponent<SpriteRenderer>();
            songDiamond.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, tmpPosition, -2f);
        }
    }

    public void SwitchBracketRight() {
        // tmpBracket stores the old songs previously in view
        // songBracket will store the new songs to come to view
        tmpBracket = new List<GameObject>(songBracket);
        GameObject tmp = songBracket[0];
        SpriteRenderer ogSongDiamond = tmp.GetComponent<SpriteRenderer>(); 
        float tmpPosition = ogSongDiamond.transform.localPosition.y;

        songBracket.Clear();
        for (int j = currentlySelectedObj.currentlySelectedSong; j < currentlySelectedObj.currentlySelectedSong + 5; j++) {
            songBracket.Add(songList[j]);
        }

        GameObject tmp2 = tmpBracket[0];

        foreach (GameObject go in tmpBracket) {
            SpriteRenderer tmpSongDiamond = go.GetComponent<SpriteRenderer>();
            tmpSongDiamond.transform.localPosition = new Vector3(tmpSongDiamond.transform.localPosition.x, -800, -2f);
        }

        foreach (GameObject go2 in songBracket) {
            SpriteRenderer songDiamond = go2.GetComponent<SpriteRenderer>();
            songDiamond.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, tmpPosition, -2f);
        }

    }


}
