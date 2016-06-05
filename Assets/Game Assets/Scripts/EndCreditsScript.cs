using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class EndCreditsScript : MonoBehaviour {

	public GameObject cam;
	public int speed = 1;
	public string level;

	void Start() {
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Update() {
		cam.GetComponent<Camera>().transform.Translate (Vector3.down * Time.deltaTime * speed);
	}

	IEnumerator waitFor() {
		yield return new WaitForSeconds (20);
		SceneManager.LoadScene (level);
	}
}


