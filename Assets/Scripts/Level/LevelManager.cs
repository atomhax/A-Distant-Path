﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {


	public const int PAST_PLAYER_SCENE = 6;
	public const int PRESENT_PLAYER_SCENE = 5; 
	public const int OFFLINE_PLAYER_SCENE = (int) Scenes.Offline;

	// GameManager GetInstanceGameManager
	private GameManager gm = GameManager.instance;
	private AIManager am = AIManager.instance;
	private EventTransferManager ETManager = null;

	// static instance of LevelManager
	public static LevelManager instance = null;
	public GameObject pastPlayerPrefab;
	public GameObject futurePlayerPrefab;
	public GameObject elementManagerPrefab;
	public GameObject uiManagerPrefab;

	public Material skyBox;

	// Instance of player
	private Player player;
	
	public TimeStates TimeState;

	// TileList for given level
	private List<Tile> TileList;

	// Associated level attached to this manager
	private Level attachedLevel = null;

	TimeStates getTimeState(){
		return this.TimeState;
	}

	public void setTimeState(TimeStates state){this.TimeState = state;}


	void Awake() {
		// if the static class instance is null (singleton pattern)
		if (instance == null)
			instance = this;

		// if instance already exists and it's not this:
		else if (instance != this)

			// then destroy this. Enforces singletonPattern
			Destroy(gameObject);

		// Sets this to not be destroyed on scene reload
		DontDestroyOnLoad(gameObject);

		Debug.Log("LevelManager.cs: Manager initialized");

		// Initialize the AI manager
		gm.InitAIManager();

	}	

	public void Update(){
		//RenderSettings.skybox = skyBox;
	}

	// LoadLevelScene() is called before LoadTileList() !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	public void LoadLevelScene(){

		Debug.Log("LevelManager.cs: Loading scene...");
    
    	if(TimeState == TimeStates.Past){
			//SceneManager.LoadScene((int)Scenes.Past);
			PhotonNetwork.LoadLevel(PAST_PLAYER_SCENE);
		}
		else if(TimeState == TimeStates.Present){
			//SceneManager.LoadScene((int)Scenes.Present);
			PhotonNetwork.LoadLevel(PRESENT_PLAYER_SCENE);
		}
		else if(TimeState == TimeStates.Offline){
 			SceneManager.LoadScene(OFFLINE_PLAYER_SCENE);	
			//SceneManager.LoadScene((int)Scenes.AITest);	

		}
		else{
			Debug.Log("INVALID TIMESTATE!!");
		}
		


		GameObject elementManagerGO = Instantiate (elementManagerPrefab) as GameObject;
		elementManagerGO.transform.parent = this.gameObject.transform;
		DontDestroyOnLoad(elementManagerGO);

		GameObject player;
		if(TimeState == TimeStates.Present || TimeState == TimeStates.Offline){
			player = Instantiate(futurePlayerPrefab, new Vector3(5f, 2.5f, 6f), Quaternion.identity) as GameObject;
		}
		else{
			player = Instantiate(pastPlayerPrefab, new Vector3(5f, 2.5f, 6f), Quaternion.identity) as GameObject;			
		}

		this.player = player.GetComponent<Player>();
		DontDestroyOnLoad(player);

        GameObject cam = Instantiate(Resources.Load("Camera")) as GameObject;
        cam.GetComponent<CameraControls>().setCharacter(player);
        DontDestroyOnLoad(cam);

        Camera.main.enabled = false;
        if (TimeState != TimeStates.Offline){
			GameObject ETManagerGO = (GameObject)PhotonNetwork.Instantiate("EventTransferManager", Vector3.zero, Quaternion.identity, 0);
			this.ETManager = ETManagerGO.GetComponent<EventTransferManager>();
			ETManager.player = player.GetComponent<Player>();
			player.GetComponent<Player>().ETmanager = ETManager;
			DontDestroyOnLoad(ETManagerGO);
        }
   
        DontDestroyOnLoad(player);

        

	}

	// Attach a new Level object (called from Level.cs)
	public void AttachLevel(Level level) {

		// Assign level to this manager
		this.attachedLevel = level;
	}

	// Get the Tile with the passed in ID, returns null if not found
	public Tile getTileAtID(int id){
		foreach(Tile tile in TileList){
			if(tile.getTileID() == id)
				return tile;
		}

		return null;
	}

	public Tile getTileAt(Vector3 pos){
		foreach(Tile tile in TileList){
			if(tile.transform.position == pos){
				return tile;
			}
		}

		Debug.Log("COULDN'T FIND TILE IN GETTILEAT");
		return null;
	}

	public Tile getTileAt(int tileID){
		return null;
	}

	// Load the tile list from level into TileList and initialize tiles
	public void LoadTileList() {
			
		// Instantiate new TileList
		TileList = new List<Tile>();

		// Loop and add all tiles
		foreach(Transform child in attachedLevel.transform){			
			TileList.Add(child.GetComponent<Tile>());
		}

	
		// Calculate the ID's of all the tiles (this must be done first in order for the neighbor method to work)
		foreach(Tile tile in TileList){
			tile.calcID();
		}
		foreach(Tile tile in TileList){
			tile.initTile();
        }

        foreach (Tile tile in TileList)
        {
        	Debug.Log(tile);
        	Debug.Log("ET MANAGER: " + this.ETManager);
            tile.EventManager = this.ETManager;
        }

		foreach(Tile tile in TileList) {
			// Attaches random elements to tiles other than the player's current position. Was used for testing 
			// out the level.
			/*if(tile.id != pastPlayerPrefab.GetComponent<Player>().getCurTileID()) {

				ElementType elementType = (ElementType)(Random.Range (0, Enum.GetNames (typeof(ElementType)).Length + 1) - 1);

				if (elementType >= 0) {
					CreateElementAtTile (tile, elementType);
				}
			}*/

			Collider[] neighbors = Physics.OverlapSphere(tile.transform.position, 1.0f);
			for (int i = 0; i < neighbors.Length; i++) {
				Tile other = neighbors[i].gameObject.GetComponent<Tile> ();

				if (other != null && !tile.neighbors.Contains(other) && other != tile) {
					tile.neighbors.Add (other);
				}
			}
		}
	}

	// Get TileList
	public List<Tile> getTileList(){return TileList;}

	// Get the attached level
	public Level getAttachedLevel() {
		return attachedLevel;
	}


	public void AddTileToList(Tile tile) {
		TileList.Add (tile);
	}

	public static void CreateElementAtTile(Tile tile, ElementType elementType) {
		GameObject elementCreated = Instantiate (ElementManager.elementSpawnDictionary[elementType], tile.transform);
		elementCreated.transform.localPosition = ElementManager.elementSpawnDictionary [elementType].transform.localPosition;
		//elementCreated.transform.position = new Vector3(tile.transform.position.x, elementCreated.transform.position.y, tile.transform.position.z);
		tile.element = elementCreated.GetComponent<Element> ();

		Debug.Log("ELEMENT NAV IN CREATELEM: " + tile.element.navigatable);
		tile.SetNavigatable (tile.element.navigatable);
	}

	public Tile GetClosestTileOfType(ElementType elemType, Vector3 position) {
		bool first = true;
		Tile closestTile = null;

		for (int i = 0; i < TileList.Count - 1; i++) {
			if(TileList[i].element != null && TileList[i].element.elementType == elemType) {
				if(first) {
					closestTile = TileList[i];
					first = false;
				}
				else{
					if(Mathf.Abs(Vector3.Distance(position, TileList[i].transform.position)) < 
						Mathf.Abs(Vector3.Distance(position, closestTile.transform.position))) {
						closestTile = TileList[i];
					}
				}
			}
		}
		return closestTile;
	}

	public Player getPlayer() {
		return this.player;
	}

}


public enum TimeStates {
	Past,
	Present,
	Offline
};