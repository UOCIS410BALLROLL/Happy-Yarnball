using UnityEngine;
using System.Collections;

public class LavaAnimation : MonoBehaviour {

	public float speed;
	public float end_x;
	public float end_z;
	
	private Rigidbody rb;
	private float start_x, start_z;
	private bool reverse;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		start_x = rb.transform.position.x;
		start_z = rb.transform.position.z;
		reverse = false;
	}
	
	void FixedUpdate() {
		if(!reverse) {
			Vector3 endPosition = new Vector3(end_x, rb.transform.position.y, end_z);
			rb.transform.position = Vector3.MoveTowards(rb.transform.position, endPosition, Time.deltaTime * speed);
			
			reverse = rb.transform.position == endPosition;
		}
		else {
			Vector3 startPosition = new Vector3(start_x, rb.transform.position.y, start_z);
			rb.transform.position = Vector3.MoveTowards(rb.transform.position, startPosition, Time.deltaTime * speed);
			
			reverse = !(rb.transform.position == startPosition);
		}
	}
}
