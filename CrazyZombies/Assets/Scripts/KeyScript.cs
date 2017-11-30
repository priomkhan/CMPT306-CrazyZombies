using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {
	public Color color = Color.black;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "player") {
			col.gameObject.GetComponent<PlayerController>().getItem(color.ToString() + " key");
			Destroy (gameObject);
		}
	}
}
