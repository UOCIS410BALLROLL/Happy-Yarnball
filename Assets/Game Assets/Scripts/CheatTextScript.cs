using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheatTextScript : MonoBehaviour {
	public Text cheatText;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("Cheatyface") == 0) {
			cheatText.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
