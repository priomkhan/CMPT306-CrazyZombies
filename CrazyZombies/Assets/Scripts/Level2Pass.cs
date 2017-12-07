using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level2Pass : MonoBehaviour {
	public string levelName = "level3";

	void  OnCollisionEnter2D (Collision2D coll)
	{
		Debug.Log ("Need black key");
		if (coll.gameObject.tag == "player") {
			if (coll.gameObject.GetComponent<PlayerController> ().haveItem (Color.black.ToString() + " key")) {
				SceneManager.LoadScene (levelName);
				Debug.Log ("Target found!");
			}
		}
	}
}
