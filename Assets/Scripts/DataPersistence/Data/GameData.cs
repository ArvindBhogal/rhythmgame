using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float storyProgression;
    public SerializableDictionary<int, int> songList;


    // Values Defined in this constructor will be the default values
    // the game starts with when there's no data to load (New Game)
    public GameData() {
        storyProgression = 0.0f;
        songList = new SerializableDictionary<int, int>();
    }
}


