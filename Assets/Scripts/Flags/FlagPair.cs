﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPair : MonoBehaviour {
    public FlagPair next;
    public bool flagsActive;
    public Player playerScript;
    public bool rightFlag = false;
    public bool leftFlag = false;
    public Gate gate;
	// Use this for initialization
	void Start () {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        playerScript = obj.GetComponent<Player>();

    }
	
	// Update is called once per frame
	void Update () {
        if (flagsActive)
        {
            if (playerScript.otherPlayerFinishedLevel || gate.gateOpen)
            {
                rightFlag = true;
            }
            if (playerScript.recentFinishTile != null)
            {
                leftFlag = true;
            }
        }
	}
}
