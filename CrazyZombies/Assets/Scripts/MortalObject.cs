using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalObject : MonoBehaviour {
	public int hp;
	private int currentHP;
	private LineRenderer healthBar;

	// Use this for initialization
	void Start () {
		currentHP = hp;
		GameObject go = new GameObject ();
		healthBar = go.AddComponent<LineRenderer> ();
		Vector3 startPos = new Vector3 (gameObject.transform.position.x - 0.5f, gameObject.transform.position.y + 1, -1f);
		Vector3 endPos = new Vector3 (gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 1, -1f);
		healthBar.SetPosition (0, startPos);
		healthBar.SetPosition (1, endPos);
		healthBar.startWidth = 0.1f;
		healthBar.endWidth = 0.1f;
		healthBar.material = new Material (Shader.Find("Particles/Alpha Blended Premultiply"));
		healthBar.startColor = Color.green;
		healthBar.endColor = Color.green;
		healthBar.transform.parent = gameObject.transform;
		go.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 startPos = new Vector3 (gameObject.transform.position.x - 0.5f, gameObject.transform.position.y + 1, -1f);
		Vector3 endPos = new Vector3(gameObject.transform.position.x - 0.5f + currentHP * 1.0f / hp, gameObject.transform.position.y + 1, -1f);
		healthBar.SetPosition (0, startPos);
		healthBar.SetPosition (1, endPos);
	}

	public bool isLowHp() {
		return currentHP <= hp / 3;
	}
	public IEnumerator takeDamage(int dmg) {
		currentHP = currentHP - dmg;
		if (currentHP < 0) {
			currentHP = 0;
		}
		healthBar.SetPosition (1, new Vector3(gameObject.transform.position.x - 0.5f + currentHP * 1.0f / hp, gameObject.transform.position.y + 1, -1f));
		healthBar.gameObject.SetActive (true);
		if (isLowHp ()) {
			healthBar.startColor = Color.red;
			healthBar.endColor = Color.red;
		}
		if (currentHP <= 0 && GetComponent<Die>() != null) {
			SendMessage ("die");
		}
		yield return new WaitForSeconds (3f);
		healthBar.gameObject.SetActive (false);
	}
}
