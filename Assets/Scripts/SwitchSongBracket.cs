using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        tmpBracket = new List<GameObject>();
        for (int j = 5; j < 10; j++) {
            tmpBracket.Add(songList[j]);
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
        for (int k = currentlySelected.currentlySelectedSong - 2; k > currentlySelected.currentlySelectedSong - 7; k--) {
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
        for (int j = currentlySelected.currentlySelectedSong; j < currentlySelected.currentlySelectedSong + 5; j++) {
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
