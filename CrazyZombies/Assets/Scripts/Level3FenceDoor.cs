using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3FenceDoor : MonoBehaviour {
	public GameObject boss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (boss == null) {
			Destroy (gameObject);
		}
	}
}
