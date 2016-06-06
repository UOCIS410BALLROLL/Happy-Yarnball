using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraVerticleInverter : MonoBehaviour {

	public Text butText;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("VerticalDirection") == 0) {
			PlayerPrefs.SetInt ("VerticalDirection", 1);
		}
		butText.text = "Vertical: " + (PlayerPrefs.GetInt ("VerticalDirection") == 1 ? "Inverted" : "Normal");
	}
	
	// Update is called once per frame
	void Update () {
		butText.text = "Vertical: " + (PlayerPrefs.GetInt ("VerticalDirection") == 1 ? "Inverted" : "Normal");
	}

	public void OnClick() {
		PlayerPrefs.SetInt ("VerticalDirection", (PlayerPrefs.GetInt ("VerticalDirection") == 1 ? -1 : 1));
	}
}
