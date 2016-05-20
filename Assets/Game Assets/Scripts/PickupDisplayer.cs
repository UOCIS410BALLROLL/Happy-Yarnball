using UnityEngine;
using System.Collections;

public class PickupDisplayer : MonoBehaviour {

	private bool isPowerup;

	void Start () {
		if ((isPowerup = gameObject.GetComponents<MeshRenderer>().Length > 0)) {
			GetComponent<MeshRenderer> ().enabled = false;
		} else {
			SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
			for (int i = 0; i < meshes.Length; i++) {
				meshes [i].enabled = false;
			}
		}
	}

	[SerializeField]
	public IEnumerator ShowPickup() {
		if (isPowerup) {
			GetComponent<MeshRenderer> ().enabled = true;
			yield return new WaitForSeconds (3);
			GetComponent<MeshRenderer> ().enabled = false;
		} else {
			SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
			for (int i = 0; i < meshes.Length; i++) {
				meshes [i].enabled = true;
			}
			yield return new WaitForSeconds (3);
			for (int i = 0; i < meshes.Length; i++) {
				meshes [i].enabled = false;
			}
		}
	}
}
