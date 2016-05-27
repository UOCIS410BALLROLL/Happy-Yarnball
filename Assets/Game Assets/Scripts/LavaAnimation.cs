using UnityEngine;
using System.Collections;

public class LavaAnimation : MonoBehaviour {

	public float speed;
	public float end_x;
	public float end_z;
	
	private Rigidbody rb;
	private float start_x, start_z;
	private bool reverse;
	private Vector2 direction;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		start_x = rb.transform.position.x;
		start_z = rb.transform.position.z;
		reverse = false;
		StartCoroutine (ManageMove ());
	}

	IEnumerator ManageMove() {
		Vector2 newDirection;
		float angle;
		while (true) {
			while ((angle = (Vector2.Angle ((newDirection = Random.insideUnitCircle * speed), direction) + 360) % 360) < 90 || angle > 270) {}
			direction = newDirection;
			yield return new WaitForSeconds (2.5f);
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

		gameObject.GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex",
			gameObject.GetComponent<Renderer>().material.GetTextureOffset("_MainTex") + direction * Time.deltaTime);
	}
}
