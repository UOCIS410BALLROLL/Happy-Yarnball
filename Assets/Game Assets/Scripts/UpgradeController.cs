using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UpgradeController : MonoBehaviour {

	public float upgradeDuration; //Amount of time the upgrade lasts
	public float upgradePower; //"Power" of the upgrade
	public float respawnTime; //Length of time in seconds before upgrade respawns
	public Text alertText;
	public string powerUp;

	private float inactiveTime;
	private GameController gc;
	private int powerCons;

	// Use this for initialization
	void Start () {
		inactiveTime = 0;
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		powerCons = gc.GetPowerConstant (powerUp);
	}
	
	// Update is called once per frame
	void Update () {
		if (inactiveTime != 0 && Time.time > inactiveTime) {
			inactiveTime = 0;
			this.gameObject.GetComponent<Collider> ().enabled = true;
			this.gameObject.GetComponent<Renderer> ().enabled = true;
			gc.RemPower (powerCons);
		}
	}

	public float GetDuration(){
		return upgradeDuration;
	}

	public float GetPower(){
		return upgradePower;
	}

	public void SetInactive(){
		this.gameObject.GetComponent<Collider> ().enabled = false;
		this.gameObject.GetComponent<Renderer> ().enabled = false;
		this.gameObject.GetComponent<AudioSource> ().Play ();
		inactiveTime = Time.time + respawnTime;
		gc.AddPower (powerCons);
	}

}
