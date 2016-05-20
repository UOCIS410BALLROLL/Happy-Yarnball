using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadButtonScript : MonoBehaviour {
	public Text buttonText;
	public Text timeText;
	public RawImage img;
	public Texture[] starTextures;
	public string level;

	private bool unlocked;

	void Start(){
		unlocked = false;
	}

	void Update () {
		int stars = PlayerPrefs.GetInt (level + "-Stars");
		if (stars > 0) {
			unlocked = true;
			timeText.text = PlayerPrefs.GetFloat (level + "-Time").ToString();
			img.texture = starTextures [stars];
		} else {
			unlocked = false;
			buttonText.text = "Locked";
			timeText.text = "";
		}
	}

	public void LoadLevel(int levelNum){
		if (unlocked) {
			SceneManager.LoadScene (levelNum);
		}
	}

	public void NewGame(){
		SceneManager.LoadScene (1);
	}
}
