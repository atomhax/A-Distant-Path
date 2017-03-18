﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {


	CharacterController controller;
	AudioSource audio;


	// Use this for initialization
	void Start () {
		
		this.controller = GetComponent<CharacterController>();
		this.audio = GetComponent<AudioSource>();

		// footsteps may only be used on the player..
		if (controller == null)
			Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log("Velocity: " + controller.velocity.magnitude);

		if (controller.isGrounded == true && controller.velocity.magnitude > 0.0f && audio.isPlaying == false) {
			
			audio.volume = Random.Range(0.8f, 1);
			audio.pitch = Random.Range(0.95f, 1.05f);

			audio.Play();
		}

	}
}
