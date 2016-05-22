using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class EndCreditsScript : MonoBehaviour {

	public GameObject camera;
	public int speed = 1;
	public string level;

	void Start() {
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Update() {
		camera.GetComponent<Camera>().transform.Translate (Vector3.down * Time.deltaTime * speed);
	}

	IEnumerator waitFor() {
		yield return new WaitForSeconds (20);
		SceneManager.LoadScene (level);
	}
}


