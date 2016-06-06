using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraHorizontalInverter : MonoBehaviour {

	public Text butText;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("HorizontalDirection") == 0) {
			PlayerPrefs.SetInt ("HorizontalDirection", 1);
		}
		butText.text = "Horizontal: " + (PlayerPrefs.GetInt ("HorizontalDirection") == 1 ? "Inverted" : "Normal");
	}

	// Update is called once per frame
	void Update () {
		butText.text = "Horizontal: " + (PlayerPrefs.GetInt ("HorizontalDirection") == 1 ? "Inverted" : "Normal");
	}

	public void OnClick() {
		PlayerPrefs.SetInt ("HorizontalDirection", (PlayerPrefs.GetInt ("HorizontalDirection") == 1 ? -1 : 1));
	}
}
