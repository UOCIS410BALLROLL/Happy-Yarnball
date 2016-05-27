using UnityEngine;
using System.Collections;

public class PickupDisplayer : MonoBehaviour {

	private bool isPowerup;
	private int activeCount;
	private bool isEnabled;

	void Start () {
		activeCount = 0;
		isEnabled = false;

		if ((isPowerup = gameObject.GetComponents<MeshRenderer>().Length > 0)) {
			GetComponent<MeshRenderer> ().enabled = false;
		} else {
			SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
			for (int i = 0; i < meshes.Length; i++) {
				meshes [i].enabled = false;
			}
		}
	}

	void Update() {
		if (activeCount > 0 && !isEnabled) {
			isEnabled = true;
			if (isPowerup) {
				GetComponent<MeshRenderer> ().enabled = true;
			} else {
				SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
				for (int i = 0; i < meshes.Length; i++) {
					meshes [i].enabled = true;
				}
			}
		} else if (activeCount == 0 && isEnabled) {
			isEnabled = false;
			if (isPowerup) {
				GetComponent<MeshRenderer> ().enabled = false;
			} else {
				SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
				for (int i = 0; i < meshes.Length; i++) {
					meshes [i].enabled = false;
				}
			}
		}
	}

	[SerializeField]
	public IEnumerator ShowPickup(float length) {
		activeCount++;
		yield return new WaitForSeconds (length);
		activeCount--;
	}

	[SerializeField]
	public void HidePickup() {
		if (isPowerup) {
			GetComponent<MeshRenderer> ().enabled = false;
		} else {
			SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
			for (int i = 0; i < meshes.Length; i++) {
				meshes [i].enabled = false;
			}
		}
	}
}
