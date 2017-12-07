using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//private Animator animator;

	public GameObject bullet_obj;
	public float bullet_speed = 25f;
	float pistolCoolDown = 0.9f;
	float rifleCoolDown = 0.6f;
	float cur_bullet_cooldown;
	AudioSource audioPlay;
	public AudioClip pistolSound;
	public AudioClip rifleSound;
	public AudioClip playerDead;
	public Vector3 bulletOffset = new Vector3(.35f, 0.5f, 0);
	private List<string> inventory;
    Animator anim;
	public int ammo=10;
	int currentWeapon=0;
	bool weapon;



	public GameObject new_bullet;

	// Use this for initialization
	void Start () {
		//animator = GetComponent<Animator>();
		inventory = new List<string>();
		anim = GetComponent<Animator>();
		selectWeapon ();
	}

	void Update(){

	
		//Shooting part

		cur_bullet_cooldown -= Time.deltaTime;
		int prevWeapon = currentWeapon;
		Vector3 offset = transform.rotation * bulletOffset;
		bool shoot = Input.GetButton("Fire1");
		bool shoot2 = Input.GetButton ("Fire2");
		AudioSource audioPlay = GetComponent<AudioSource>();

		//pistol gun shooting function

		if (shoot && cur_bullet_cooldown <= 0) {
			audioPlay.PlayOneShot (pistolSound);

			anim.SetTrigger ("pistolShoot");
			//Create a bullet object
			new_bullet = (GameObject)Instantiate (bullet_obj, this.transform.position + offset, this.transform.rotation * Quaternion.identity);
			Rigidbody2D new_bullet_physics = new_bullet.GetComponent<Rigidbody2D> ();
			new_bullet_physics.velocity = this.transform.up * bullet_speed;

			cur_bullet_cooldown = pistolCoolDown;

		}

		//rifle gun shooting function
		if (shoot2 && cur_bullet_cooldown <= 0) { //cur_bullet_cooldown <= Time.time

			audioPlay.PlayOneShot (rifleSound);

			anim.SetTrigger ("Shoot");
			//Create a bullet object
			new_bullet = (GameObject)Instantiate (bullet_obj, this.transform.position + offset, this.transform.rotation * Quaternion.identity);
			Rigidbody2D new_bullet_physics = new_bullet.GetComponent<Rigidbody2D> ();
			new_bullet_physics.velocity = this.transform.up * bullet_speed;

			cur_bullet_cooldown = rifleCoolDown;
		}



		//switch weapon to the hand gun
		if (Input.GetMouseButtonDown(0)) {
			
			currentWeapon = 0;
			anim.SetBool ("handGun", true);
			anim.SetBool ("rifleGun", false);


		}
			
		//swith weapon to the rifle gun
		if (Input.GetMouseButtonDown(1)&&transform.childCount >= 2) {

			currentWeapon = 1;
			anim.SetBool ("rifleGun", true);
			anim.SetBool ("handGun", false);

		}

		if (prevWeapon != currentWeapon) {

			selectWeapon ();

		}
			
	}



	// Update is called once per frame
	void FixedUpdate () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Quaternion rotation =  Quaternion.LookRotation ( Vector3.forward, mousePos - transform.position);
		transform.rotation = rotation;
			


		//walk function
		Vector2 v = new Vector2 ();
		GetComponent<Rigidbody2D> ().velocity = v;

			if (Input.GetButton ("Horizontal") && Input.GetAxisRaw ("Horizontal") < 0) {
				v.x = -200 * Time.deltaTime;
		
			} else if (Input.GetButton ("Horizontal") && Input.GetAxisRaw ("Horizontal") > 0) {
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

		//set player walk animations

		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");

		if (inputX != 0 || inputY != 0) {
			anim.SetBool ("Player_Walk", true);

			if (inputX > 0) {
				anim.SetFloat ("MoveX", 1f);
			} else if (inputX < 0) {
				anim.SetFloat ("MoveX", -1f);
			} else {
				anim.SetFloat ("MoveY", 0f);
			}
			if (inputY > 0) {
				anim.SetFloat ("MoveY", 1f);
			} else if (inputY < 0) {
				anim.SetFloat ("MoveY", -1f);
			} else {
				anim.SetFloat ("MoveY", 0f);
			}
		} else {
			anim.SetBool ("Player_Walk", false);
		}

	}

	//switch guns
	void selectWeapon(){

		int i = 0;
		foreach (Transform weapon in transform){

			if (i == currentWeapon) {

				weapon.gameObject.SetActive (true);
			} else {

				weapon.gameObject.SetActive (false);
			}
			i++;
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

	public void getItem(string item) {
		inventory.Add (item);
	}

	public bool haveItem(string item) {
		return inventory.Contains (item);
	}
}
