﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFlagControlller : MonoBehaviour {
    public FlagPair fp;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (fp.leftFlag && this.transform.localPosition.z < 4.19)
        {
            transform.position += Vector3.up * 0.01f;
        }
    }
}
