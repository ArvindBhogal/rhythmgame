using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance { get; private set; }
    private FileDataHandler dataHandler;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        else {
            instance = this;
        }
    }

    private void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame() {
        this.gameData = new GameData();
    }

    public void LoadGame() {
        // TODO - Load any saved data from a file using the data handler. 
        // In my case, I'd only be using one save data so only one file needs to be specified
        // If no data can be loaded, initialize to new game

        this.gameData = dataHandler.Load();

        if (this.gameData == null) {
            Debug.Log("No data found. Making new game");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.LoadData(gameData);
        }

        // Debug.Log("Loaded Score = " + gameData.scoreSong1);
        // TODO - Push the loaded data to all other scripts that need it
        // In my case, this would be the songSelect screen to load previous values and scores.
    }

    public void SaveGame() {
        // TODO - Pass data to other scripts so they can update it
        // TODO - Save data to a file using the data handler
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Debug.Log("Saved Score = " + gameData.scoreSong1);

        dataHandler.Save(gameData);
        Debug.Log("Saved");

    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}

