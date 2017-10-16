using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {

	public GameObject target;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 v = new Vector2 ();
		v.x = target.transform.position.x - transform.position.x;
		v.y = target.transform.position.y - transform.position.y;
		if (v.magnitude != 0) {
			Vector2 v2 = new Vector2 ();
			v2.x = v.x * speed / v.magnitude * Time.deltaTime;
			v2.y = v.y * speed / v.magnitude * Time.deltaTime;
			GetComponent<Rigidbody2D> ().velocity = v2;
		}
	}
}
