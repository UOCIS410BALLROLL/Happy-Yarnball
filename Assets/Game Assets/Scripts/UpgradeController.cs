using UnityEngine;
using System.Collections;

public class UpgradeController : MonoBehaviour {

	public float upgradeDuration; //Amount of time the upgrade lasts
	public float upgradePower; //"Power" of the upgrade
	public float respawnTime; //Length of time in seconds before upgrade respawns

	private float inactiveTime;
	// Use this for initialization
	void Start () {
		inactiveTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public float GetDuration(){
		return upgradeDuration;
	}

	public float GetPower(){
		return upgradePower;
	}

	public void SetInactive(){
		StartCoroutine (WaitTilActive ());
		this.gameObject.SetActive (false);
	}

	IEnumerator WaitTilActive(){
		yield return new WaitForSeconds (3);
		this.gameObject.SetActive (true);
	}
}
