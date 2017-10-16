using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerWeaponType{Hand,PISTOL, NULL}
public class PlayerController : MonoBehaviour {

	private Animator animator;
	public float playerMovingSpeed;
	PlayerWeaponType currentWeapon=PlayerWeaponType.NULL;
	float attackTime=0.4f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 v = new Vector2 ();
		if (Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {
			v.x = -100 * playerMovingSpeed * Time.deltaTime;
		} else if (Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {
			v.x = 100 * playerMovingSpeed * Time.deltaTime;
		} else {
			v.x = 0;
		}

		if (Input.GetButton ("Vertical") && Input.GetAxisRaw ("Vertical") < 0) {
			v.y = -100 * playerMovingSpeed * Time.deltaTime;
		} else if (Input.GetButton ("Vertical") && Input.GetAxisRaw ("Vertical") > 0) {
			v.y = 100 * playerMovingSpeed * Time.deltaTime;
		} else {
			v.y = 0;
		}
		GetComponent<Rigidbody2D> ().velocity = v;

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.rotation = Quaternion.LookRotation (Vector3.forward, mousePos - transform.position);
	}


	void SetWeapon(PlayerWeaponType weaponType){
		if (weaponType != currentWeapon) {
			currentWeapon = weaponType;
			animator.SetTrigger ("WeaponChange");
			switch (weaponType) {
			case PlayerWeaponType.Hand:
				attackTime=0.4f;
				animator.SetInteger ("WeaponType", 0);
				break;
			case PlayerWeaponType.PISTOL:
				attackTime=0.1f;
				animator.SetInteger ("WeaponType", 1);
				break;
			}
		}
		GameManager.SelectWeapon (weaponType);
	}

}
