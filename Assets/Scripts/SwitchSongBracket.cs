using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSongBracket : MonoBehaviour
{

    private GameObject[] songList;
    private CurrentlySelectedObject currentlySelected;
    public List<GameObject> songBracket;
    public List<GameObject> tmpBracket;
    public int numberOfSongs;

    // Start is called before the first frame update
    void Start() {

        currentlySelected = gameObject.GetComponent<CurrentlySelectedObject>();

        songList = GameObject.FindGameObjectsWithTag("Song Crystal");
        numberOfSongs = songList.Length;
        songBracket = new List<GameObject>();
        for (int i = 0; i < 5; i++) {
            songBracket.Add(songList[i]);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SwitchBracketLeft() {
        tmpBracket = new List<GameObject>(songBracket);
        songBracket.Clear();

        for (int k = currentlySelected.currentlySelectedSong - 2; k > currentlySelected.currentlySelectedSong - 7; k--) {
            songBracket.Add(songList[k]);
        }

        GameObject tmp = songBracket[0];
        GameObject tmp2 = tmpBracket[0];
        SpriteRenderer ogSongDiamond = tmp.GetComponent<SpriteRenderer>(); 

        foreach (GameObject go in tmpBracket) {
            SpriteRenderer tmpSongDiamond = go.GetComponent<SpriteRenderer>();
            tmpSongDiamond.transform.localPosition = new Vector3(tmpSongDiamond.transform.localPosition.x, ogSongDiamond.transform.localPosition.y, -2);
        }

        foreach (GameObject go2 in songBracket) {
            SpriteRenderer songDiamond = go2.GetComponent<SpriteRenderer>();
            songDiamond.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, -324, -2);
        }
    }

    public void SwitchBracketRight() {
        tmpBracket = new List<GameObject>(songBracket);
        songBracket.Clear();
        for (int j = currentlySelected.currentlySelectedSong; j < currentlySelected.currentlySelectedSong + 5; j++) {
            songBracket.Add(songList[j]);
        }

        GameObject tmp = songBracket[0];
        GameObject tmp2 = tmpBracket[0];
        SpriteRenderer ogSongDiamond = tmp.GetComponent<SpriteRenderer>(); 

        foreach (GameObject go in tmpBracket) {
            SpriteRenderer tmpSongDiamond = go.GetComponent<SpriteRenderer>();
            tmpSongDiamond.transform.localPosition = new Vector3(tmpSongDiamond.transform.localPosition.x, ogSongDiamond.transform.localPosition.y, -2);
        }

        foreach (GameObject go2 in songBracket) {
            SpriteRenderer songDiamond = go2.GetComponent<SpriteRenderer>();
            songDiamond.transform.localPosition = new Vector3(songDiamond.transform.localPosition.x, -324, -2);
        }

    }


}
