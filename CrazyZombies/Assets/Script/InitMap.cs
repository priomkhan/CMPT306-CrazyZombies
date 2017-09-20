using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMap : MonoBehaviour {

	public GameObject object1;

	// Use this for initialization
	void Start () {
		int x = 0;
		int y = 0;
		while (x < 80 && y < 10) {
			GameObject obj = Instantiate (object1);	
			Vector2 v;
			do {
				v = new Vector2 (100, 100);
				Debug.Log(v);
			} while (Physics2D.OverlapArea (v, v) == null);

			//obj.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			obj.GetComponent<Rigidbody2D> ().position = v;
			//obj.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
			x = Random.Range (0, 100);
			y++;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
