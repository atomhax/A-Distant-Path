﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public bool firstTime = true;

	public Vector3 SpawnPoint;

	public GameObject nextFlagGO;
	public Flag nextFlag;

	// Use this for initialization
	void Start () {
		this.SpawnPoint = new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z);
		nextFlag = nextFlagGO.GetComponent<Flag>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other){
		Debug.Log("TRIGGER ON CHECKPOINT");

		Player player = other.gameObject.GetComponent<Player>();

		if(player != null){
			player.nextFlag = this.nextFlag;

            if (firstTime) { player.emptyInventory(); }
			player.curRespawnPoint = this.SpawnPoint;
                firstTime = false;
		}

	}
}
