using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//private Animator animator;

	public GameObject bullet_obj;
	public float bullet_speed = 5f;
	float bullet_cooldown = 1.0f;
	float cur_bullet_cooldown;
	public AudioClip bulletFired;
	public AudioClip playerDead;
	public Vector3 bulletOffset = new Vector3(2.45f, 3.5f, 0);


	// Use this for initialization
	void Start () {
		//animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Quaternion rotation =  Quaternion.LookRotation ( Vector3.forward, mousePos - transform.position);
		transform.rotation = rotation;
			
		Vector2 v = new Vector2 ();
		if (Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {
			v.x = -200 * Time.deltaTime;
		} else if (Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {
			v.x = 200 * Time.deltaTime;
		} else {
			v.x = 0;
		}

		if (Input.GetButton ("Vertical") && Input.GetAxisRaw ("Vertical") < 0) {
			v.y = -200 * Time.deltaTime;
		} else if (Input.GetButton ("Vertical") && Input.GetAxisRaw ("Vertical") > 0) {
			v.y = 200 * Time.deltaTime;
		} else {
			v.y = 0;
		}


		GetComponent<Rigidbody2D> ().velocity = v;
   

		//Shooting

		cur_bullet_cooldown -= Time.deltaTime;

		Vector3 offset = transform.rotation * bulletOffset;



		bool shooting = Input.GetButton("Fire1");
		AudioSource audioPlay = GetComponent<AudioSource>();

		if (shooting && cur_bullet_cooldown <= 0) { //cur_bullet_cooldown <= Time.time

			audioPlay.PlayOneShot(bulletFired);

			//Create a bullet object
			GameObject new_bullet = (GameObject) Instantiate(bullet_obj, this.transform.position + offset, this.transform.rotation * Quaternion.identity);
			Rigidbody2D new_bullet_physics = new_bullet.GetComponent<Rigidbody2D> ();
			new_bullet_physics.velocity = this.transform.up * bullet_speed;

			cur_bullet_cooldown = bullet_cooldown;

		}    
	}




	//Player Die on collision with enemy

	void OnCollisionEnter2D(Collision2D col)   {
		if (col.gameObject.tag == "enemy" )		{
			Renderer[] renderers = GetComponentsInChildren<Renderer>(); // remove player from view            
			foreach (Renderer r in renderers)
			{                
				r.enabled = false;
			}
			AudioSource audio = GetComponent<AudioSource>();
			audio.PlayOneShot(playerDead);
			StartCoroutine(pause());
			GetComponent<Collider2D>().enabled = false; // so it doesnt spam screams if hit multiple times

		}
	}

	//Reset and reload the game

	IEnumerator pause()	{
		yield return new WaitForSeconds(3);
		//Application.LoadLevel(Application.loadedLevel);
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	
}
