using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float playerMovingSpeed;
	public float mouseRotationSpeed;
	public GameObject mousePointer;
	Rigidbody myRigidBody;
	public Animator anim;
	public Vector2 speed = new Vector2(5, 5);
	private Vector3 movement;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");
		anim.SetFloat ("SpeedX", inputX);
		anim.SetFloat ("SpeedY", inputY);

		Vector3 movement = new Vector3 (
			                   speed.x * inputX,
			                   speed.y * inputY,
			                   0
		                   );

		movement *= Time.deltaTime;
		transform.Translate (movement);


		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < 0.5f) {
		
			transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * playerMovingSpeed * Time.deltaTime, 0f, 0f));
		
		}

		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < 0.5f) {

			transform.Translate (new Vector3 (0f, Input.GetAxisRaw ("Vertical") * playerMovingSpeed * Time.deltaTime, 0f));

		}
			

		if (inputX != 0 || inputY != 0) {
			
			anim.SetBool ("walk", true);

			if (inputX > 0) {
				anim.SetFloat ("MoveX", 1f);
			} else if (inputX < 0) {
				anim.SetFloat ("MoveX", -1f);
			} else {
				anim.SetFloat ("MoveX", 0f);
			}
			if (inputY > 0) {
				anim.SetFloat ("MoveY", 1f);
			} else if (inputY < 0) {
				anim.SetFloat ("MoveY", -1f);
			} else {
				anim.SetFloat ("MoveY", 0f);
			}
				
		} else {
			anim.SetBool ("walk", false);
			
		}
	}	
}
