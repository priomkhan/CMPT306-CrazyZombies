using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float playerMovingSpeed;
	public float mouseRotationSpeed;
	public GameObject mousePointer;
	Rigidbody myRigidBody;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal")< 0.5f) {
		
			transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * playerMovingSpeed * Time.deltaTime, 0f, 0f));
		
		}

		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw("Vertical")< 0.5f) {

			transform.Translate ( new Vector3 (0f, Input.GetAxisRaw ("Vertical") * playerMovingSpeed * Time.deltaTime, 0f));

		}

		//float inputHorizontal = Input.GetAxis ("Horizontal");
		//float inputVertical = Input.GetAxis ("Vertical");
		//Vector3 newVelocity=new Vector3(inputVertical*playerMovingSpeed, 0.0f, inputHorizontal*-playerMovingSpeed);
		//myRigidBody.velocity = newVelocity;

		//Vector2 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		//float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		//Quaternion rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		//transform.rotation = Quaternion.Slerp (transform.rotation, rotation, mouseRotationSpeed * Time.deltaTime);
	}




}
