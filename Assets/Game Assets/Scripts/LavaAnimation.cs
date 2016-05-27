using UnityEngine;
using System.Collections;

public class LavaAnimation : MonoBehaviour {

	public float speed;
	public float x_factor;
	public float z_factor;
	public float period;

	private Renderer rend;
	private Vector2 direction;
	
	void Start () {
		rend = GetComponent<Renderer>();
		direction = new Vector2 (x_factor, z_factor) * speed;
		StartCoroutine (ManageMove ());
	}

	IEnumerator ManageMove() {
		while (true) {
			yield return new WaitForSeconds (period);
			direction = -direction;
		}
	}
	
	void FixedUpdate() {
		/*if(!reverse) {
			Vector3 endPosition = new Vector3(end_x, rb.transform.position.y, end_z);
			rb.transform.position = Vector3.MoveTowards(rb.transform.position, endPosition, Time.deltaTime * speed);
			
			reverse = rb.transform.position == endPosition;
		}
		else {
			Vector3 startPosition = new Vector3(start_x, rb.transform.position.y, start_z);
			rb.transform.position = Vector3.MoveTowards(rb.transform.position, startPosition, Time.deltaTime * speed);
			
			reverse = !(rb.transform.position == startPosition);
		}*/

		rend.material.SetTextureOffset ("_MainTex", rend.material.GetTextureOffset("_MainTex") + direction * Time.deltaTime);
	}
}
