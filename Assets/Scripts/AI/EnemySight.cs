﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

	private LevelManager lm = LevelManager.instance;

	private Player player;

	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Tile personalLastSighting;

	private NavMeshAgent nav;
	private SphereCollider col;

	
	void Awake() {
		nav = GetComponent<NavMeshAgent>();
		col = GetComponent<SphereCollider>();

		player = lm.getPlayer();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay() {

		Vector3 direction = player.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);

		// check if player is within fied of view
		if (angle < fieldOfViewAngle * 0.5f) {

			RaycastHit hit;



		}

	}
}

	

	