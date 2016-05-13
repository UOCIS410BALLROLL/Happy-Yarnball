using UnityEngine;
using System.Collections;

public class LavaRiseController : MonoBehaviour {
	
	public float riseSpeed;
	
	private Rigidbody rb;
	private bool raise;
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		raise = false;
	}
	
	void FixedUpdate() {
		if(raise) {
			rb.transform.position += Vector3.up * riseSpeed * Time.deltaTime;
		}
	}

	[SerializeField]
	public void RaiseLava() {
		raise = true;
	}
	
	[SerializeField]
	public void StopLava() {
		raise = false;
	}
}
