using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditButtonController : MonoBehaviour {
	public Button credits;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt("Space-Stars") == 0)
			credits.gameObject.SetActive (false);
	}

	public void LoadCredits(){
		SceneManager.LoadScene (6);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
