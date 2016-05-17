﻿using UnityEngine;
using System.Collections;

public class LavaStopper : MonoBehaviour {
	
	private bool hasTriggered;

	void Start () {
		hasTriggered = false;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player") && !hasTriggered) {
			hasTriggered = true;
			GameObject.FindGameObjectWithTag("Lava").GetComponent<LavaRiseController>().StopLava();
		}
	}
}