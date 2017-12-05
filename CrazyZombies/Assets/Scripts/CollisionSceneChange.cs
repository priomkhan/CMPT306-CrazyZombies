using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionSceneChange : MonoBehaviour {

	public string levelName = "level2";

	// Use this for initialization
	void  OnCollisionEnter2D (Collision2D Colider)
	{
		if (Colider.gameObject.tag == "player") {

			SceneManager.LoadScene (levelName);
			Debug.Log("Target found!");
		}
	}
}
	