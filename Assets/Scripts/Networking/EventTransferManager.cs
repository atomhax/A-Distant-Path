﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTransferManager : Photon.MonoBehaviour {

	public int cur;


	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {

		if(photonView.isMine && Input.GetKeyDown("space"))
			GetComponent<PhotonView>().RPC("Event",PhotonTargets.Others, new object[]{50});
		
	}


	[PunRPC]
	public void Event(int TileID){
		Debug.Log(TileID);
	}	

}