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
			buttonText.text = level;
			timeText.text = string.Format("{0:F2}", PlayerPrefs.GetFloat (level + "-Time"));
			img.texture = starTextures [stars];
		} else {
			unlocked = false;
			buttonText.text = "Locked";
			timeText.text = "";
			img.texture = starTextures [stars];
		}
	}

	public void LoadLevel(int levelNum){
		if (unlocked) {
			SceneManager.LoadScene (levelNum);
		}
	}

	public void NewGame(){
		if (PlayerPrefs.GetInt ("Mountain-Stars") > 0) {
			SceneManager.LoadScene (5);
		} else if (PlayerPrefs.GetInt ("Forest-Stars") > 0) {
			SceneManager.LoadScene (4);
		} else if (PlayerPrefs.GetInt ("Ocean-Stars") > 0) {
			SceneManager.LoadScene (3);
		} else if (PlayerPrefs.GetInt ("Desert-Stars") > 0) {
			SceneManager.LoadScene (2);
		} else {
			SceneManager.LoadScene (1);
		}
	}

	public void LoadHelp(){
		SceneManager.LoadScene (-1);
	}


}
