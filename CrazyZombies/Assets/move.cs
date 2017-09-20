using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 v = new Vector2 ();
		if (Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {
			v.x = -200 * Time.deltaTime;
		} else if (Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {
			v.x = 200 * Time.deltaTime;
		} else {
			v.x = 0;
		}

		if (Input.GetButton ("Vertical") && Input.GetAxisRaw ("Vertical") < 0) {
			v.y = -200 * Time.deltaTime;
		} else if (Input.GetButton ("Vertical") && Input.GetAxisRaw ("Vertical") > 0) {
			v.y = 200 * Time.deltaTime;
		} else {
			v.y = 0;
		}
		GetComponent<Rigidbody2D> ().velocity = v;
	}
}
