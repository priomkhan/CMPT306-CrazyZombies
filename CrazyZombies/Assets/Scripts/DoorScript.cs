using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
	public Color color;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "player" && col.gameObject.GetComponent<PlayerController> ().haveItem (color.ToString () + " key")) {
			Destroy (gameObject);
		}
	}
}
