﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float lavaSinkTime; //How long it takes for the player to sink into lava
	public float lavaSinkMaxAngularVelocity;

	private Rigidbody rb;
    private MeshRenderer rend;
    private Collider coll;
    private GameController game;
	private AudioSource audioSource;
    private AudioSource powerupSound;
	private bool isJumping, isPowered, canDoubleJump, hasDoubleJumped;
	private bool touchedLava;
	private float jumpHeight;
	private float powerEnd;
	private float upgrade;
	private float scaleVal;
	

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();       //I can't figure out how to instatiate an instance of GameController
		isJumping=false;
		isPowered = false;
		canDoubleJump = false;
		hasDoubleJumped = false;
		touchedLava = false;
		audioSource = GetComponent<AudioSource> ();
		upgrade = 1;
		scaleVal = 1.0f;
    }
	
	bool isGrounded() {
		RaycastHit hit;
		
		//Casts a sphere of radius -0.1f less than the length of the radius of the player's sphere collider. This sphere is sent downward a maximum distance of 0.2f (0.1f below the
		//collider boundary) and attempts to detect a collision with the ground.
		return Physics.SphereCast(gameObject.transform.position, (GetComponent<SphereCollider>().radius - 0.1f) * scaleVal, Vector3.down, out hit, 0.25f);
	}
	
	void OnCollisionEnter(Collision collisionInfo) {
	    //This function is used to reduce occurrences of jumping before hitting the ground due to the isGrounded() implementation.
		//May still occur if player hits a wall before falling to the floor.
		isJumping = false;
	}

	void FixedUpdate ()
	{
		if(touchedLava) {
			return;
		}
		
		float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

		if (!isJumping && isGrounded() && Input.GetKeyDown ("space"))
		{
			isJumping = true;
			hasDoubleJumped = false;
			rb.velocity = rb.velocity.y > 0
				? new Vector3(rb.velocity.x, rb.velocity.y + jumpHeight, rb.velocity.z)
				: new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
		}
		else if(!hasDoubleJumped && canDoubleJump && Input.GetKeyDown ("space")) {
			hasDoubleJumped = true;
			rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
		}
		
		if (isPowered) {
			if (Time.time > powerEnd) {
				upgrade = 1;
				isPowered = false;
				canDoubleJump = false;
				scaleVal = 1.0f;
				rb.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
			}
		}

        //get trig values of the current camera angle
        float angle = Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad;
        float trig1 = Mathf.Cos(angle);
        float trig2 = Mathf.Sin(angle);

        //defines the partial vertical and horizontal sub-components of movement
        //by adding trig-defined magnitudes
        float adjustedHorizontal = moveHorizontal * trig1 + moveVertical * trig2;
        float adjustedVertical = moveVertical * trig1 - moveHorizontal * trig2;
        
        ///Vector3 movement = Vector3.ClampMagnitude(new Vector3(adjustedHorizontal, moveHeight, adjustedVertical), 1.0f);
		Vector3 movement = new Vector3(adjustedHorizontal,0.0f,adjustedVertical);
		rb.AddForce(movement * speed * upgrade);
		Vector2 clampedVelocity = Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), speed * upgrade);
		rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.y);
	}

	public void MoonJump()
	{
		//cheat function, activates through GameController
		Vector3 movement = new Vector3 (rb.velocity.x, 40.0f, rb.velocity.y);
		rb.AddForce (movement * speed * upgrade);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ( "Cat"))
		{
			other.gameObject.SetActive (false);
			game.AddMorsel();
            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.Play ();
			StartCoroutine(GameObject.FindGameObjectWithTag("UIPickup").GetComponent<PickupDisplayer> ().ShowPickup ());
		}
		else if (other.gameObject.CompareTag("SpeedUp1"))
		{
            
            UpgradeController powerup = (UpgradeController) other.gameObject.GetComponent("UpgradeController");
            rend = other.gameObject.GetComponent<MeshRenderer>();
            coll = other.gameObject.GetComponent<Collider>();
            rend.enabled = false;
            coll.enabled = false;
            powerupSound = other.gameObject.GetComponent<AudioSource>();
            powerupSound.Play();
            //powerup.SetInactive ();
            isPowered = true;
			upgrade = powerup.GetPower();
			powerEnd = Time.time + powerup.GetDuration();
			StartCoroutine(GameObject.FindGameObjectWithTag("UISpeed").GetComponent<PickupDisplayer> ().ShowPickup ());
		}
		else if (other.gameObject.CompareTag("Goal"))
		{
			game.ExitReached();
		}
		else if(other.gameObject.CompareTag("DoubleJump")) {
            UpgradeController powerup = (UpgradeController)other.gameObject.GetComponent("UpgradeController");
            rend = other.gameObject.GetComponent<MeshRenderer>();
            coll = other.gameObject.GetComponent<Collider>();
            rend.enabled = false;
            coll.enabled = false;
            powerupSound = other.gameObject.GetComponent<AudioSource>();
            powerupSound.Play();
            //powerup.SetInactive ();
            isPowered = true;
			canDoubleJump = true;
			powerEnd = Time.time + powerup.GetDuration();
			StartCoroutine(GameObject.FindGameObjectWithTag("UIDouble").GetComponent<PickupDisplayer> ().ShowPickup ());
		}
        else if (other.gameObject.CompareTag("Shrink"))
        {
            UpgradeController powerup = (UpgradeController) other.gameObject.GetComponent("UpgradeController");
            rend = other.gameObject.GetComponent<MeshRenderer>();
            coll = other.gameObject.GetComponent<Collider>();
            rend.enabled = false;
            coll.enabled = false;
            powerupSound = other.gameObject.GetComponent<AudioSource>();
            powerupSound.Play();
            //powerup.SetInactive ();
            isPowered = true;
			scaleVal = powerup.GetPower();
            rb.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
			StartCoroutine(GameObject.FindGameObjectWithTag("UIShrink").GetComponent<PickupDisplayer> ().ShowPickup ());
			powerEnd = Time.time + powerup.GetDuration();
        }
        
    }
	
	[SerializeField]
	public IEnumerator LavaDeath() {
		touchedLava = true;
		rb.velocity = new Vector3(0, 0, 0);
		rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, lavaSinkMaxAngularVelocity);
		
		float playerHeight = 2 * GetComponent<SphereCollider>().radius;
		float playerY = rb.transform.position.y;
		
		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime/lavaSinkTime) {
			rb.transform.position = new Vector3(rb.transform.position.x, playerY - (playerHeight * t), rb.transform.position.z);
			yield return null;
		}
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerDestroy();
	}

	[SerializeField]
	public void SetJumpHeight(float height) {
		jumpHeight = height;
	}
	
    /*void shrinkPlayer()
    {
        float scaleVal = 0.7f;
        if (countdownTimer())
        {
            rb.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        }
        else {
            scaleVal = 1.0f;
            rb.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        }
    }*/

}