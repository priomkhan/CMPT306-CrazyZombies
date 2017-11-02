using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalObject : MonoBehaviour {
	public int hp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void takeDamage(int dmg) {
		hp = hp - dmg;

		if (hp <= 0 && GetComponent<Die>() != null) {
			SendMessage ("die");
		}
	}
}
