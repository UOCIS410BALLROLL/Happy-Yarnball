using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	public Text cheatText;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("Cheatyface") == 1 && SceneManager.GetActiveScene().name.CompareTo("Help") != 0 && cheatText != null) {
			cheatText.text = "Cheatyface";
		}
		if (PlayerPrefs.GetInt ("hasrun") == 0) {
			PlayerPrefs.SetInt ("hasrun", 1);
			PlayerPrefs.Save ();
			LoadHelp ();
		}
		if (PlayerPrefs.GetInt ("HorizontalDirection") == 0) {
			PlayerPrefs.SetInt ("HorizontalDirection", 1);
			PlayerPrefs.SetInt ("VerticalDirection", 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetData(){
		int vert = PlayerPrefs.GetInt ("VerticalDirection");
		int hor = PlayerPrefs.GetInt ("HorizontalDirection");
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetInt ("VerticalDirection", vert);
		PlayerPrefs.SetInt ("HorizontalDirection", hor);
	}

	public void LoadHelp(){

		SceneManager.LoadScene (7);
	}

	public void LoadMenu(){
		PlayerPrefs.SetInt ("hasrun", 1);
		PlayerPrefs.Save ();
		SceneManager.LoadScene (0);
	}

	public void CheatyFace(){
		if (PlayerPrefs.GetInt ("Cheatyface") == 1) {
			PlayerPrefs.SetInt ("Cheatyface", 0);
			cheatText.text = "";
			PlayerPrefs.Save ();
		} else {
			PlayerPrefs.SetInt ("Cheatyface", 1);
			cheatText.text = "Cheatyface";
			PlayerPrefs.Save ();
		}
	}
}
