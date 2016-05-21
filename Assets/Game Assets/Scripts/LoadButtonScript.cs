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
			PlayerPrefs.SetString ("PlayType", "Once");
			PlayerPrefs.Save ();
			SceneManager.LoadScene (levelNum);
		}
	}

	public void NewGame(){
		PlayerPrefs.SetString ("PlayType", "All");
		SceneManager.LoadScene (1);
	}
}
