using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulObject : MonoBehaviour {
	public int damage; // How harmful this object is
	public GameObject target;// What this object harm for

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//GameObject player = GameObject.FindGameObjectWithTag (harmTarget);
		//if (GetComponent<Rigidbody2D> ().IsTouching (player.GetComponent<Collider2D>()) && player.GetComponent<MortalObject>() != null) {
		//	player.SendMessage ("takeDamage", damage);

		if (GetComponent<Rigidbody2D> ().IsTouching (target.GetComponent<Collider2D>()) && target.GetComponent<MortalObject>() != null) {
			target.SendMessage ("takeDamage", damage);
		}
	}
}
