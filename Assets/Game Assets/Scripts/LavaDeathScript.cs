using UnityEngine;
using System.Collections;

public class LavaDeathScript : MonoBehaviour {
	
	private bool killingPlayer;
	
	void Start() {
		killingPlayer = false;
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player") && !killingPlayer) {
			killingPlayer = true;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().SetPlayer(null);
			StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().LavaDeath());
		}
	}
}
