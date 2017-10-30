using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulObject : MonoBehaviour {
	public int damage; // How harmful this object is
	public string harmTarget; // What this object harm for

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (harmTarget == "all") {
			if (coll.gameObject.GetComponent<MortalObject> () != null) {
				coll.gameObject.SendMessage ("takeDamage", damage);
			}
		} else {
			if (coll.gameObject.tag == harmTarget) {
				coll.gameObject.SendMessage ("takeDamage", damage);
			}
		}
	}
}
