using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject fireAnimation;
	public AudioClip shotFired;
	public AudioClip deadZomieSound;
	private GameController gameController;
	public int scoreValue = 1;
	// Use this for initialization
	void Start () {

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Update is called once per frame

	void Update () {

	}


	void OnCollisionEnter2D(Collision2D col)
	{
		AudioSource audioPlay = GetComponent<AudioSource>();
		if ( col.gameObject.tag == "wall" || col.gameObject.tag == "enemy" ||  col.gameObject.tag == "car" || col.gameObject.tag == "object")
		{
//			if (col.gameObject.tag == "enemy") {
//				EnemyAI zombie = col.gameObject.GetComponent<EnemyAI> ();
//				zombie.zombieHit ();
//				zombie.attackPlayer ();
//				if (zombie.getHP () == 0) {
//					audioPlay.PlayOneShot (deadZomieSound);
//					gameController.AddScore (scoreValue);
//					Destroy (col.gameObject);
//				}
//
//				if (col.gameObject.tag == "car") {
//				
//					
//				}

			if (col.gameObject.tag != "object") {
				col.gameObject.SendMessage ("takeDamage", 1);
			}






			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			Destroy(GetComponent<BoxCollider2D>());

			int childs = transform.childCount;
			for (var i = childs - 1; i >= 0; i--)
			{
				Destroy(transform.GetChild(i).gameObject, 0.1f);
			}
			audioPlay.PlayOneShot(shotFired);
			Destroy(gameObject, 0f); // destroys bullet
			GameObject bulletExplosion = (GameObject)Instantiate(fireAnimation, transform.position, transform.rotation * Quaternion.Euler(0, 0, 90));
			Destroy(bulletExplosion, 0.9f); // distroy expoition animation




		}

	}




}
