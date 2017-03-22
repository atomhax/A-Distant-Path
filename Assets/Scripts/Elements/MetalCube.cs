﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalCube : Element {

	public int tileID;

	private AIManager am = AIManager.instance;

	void Start () {
		elementType = ElementType.MetalCube;
		this.tileID = transform.parent.GetComponent<Tile>().getTileID();
		this.climable = true;
		navigatable = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		Debug.Log("HERHERHERHEHREHRHERHER");
		am.AIStateEvent(AIEvents.OnMetalCubeRust);
	}

	public override bool WaterInteract(EventTransferManager ETManager) {



		if(ETManager != null && LevelManager.instance.TimeState == TimeStates.Past) {
			ETManager.OnMetalRust(this.GetComponentInParent<Tile>().gameObject.transform.position);
		}
		this.GetComponentInParent<Tile> ().GainElement (ElementType.Water);
		return true;
	}

	public override bool FireInteract(EventTransferManager ETManager) {
		return false;
	}
}
