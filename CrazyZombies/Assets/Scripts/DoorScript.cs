using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
	public Color color;
	Animator anim;


	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "player" && col.gameObject.GetComponent<PlayerController> ().haveItem (color.ToString () + " key")) {
			anim.SetBool ("doorOpen", true);

			Destroy (gameObject,1.0f);
		}
	}
}
