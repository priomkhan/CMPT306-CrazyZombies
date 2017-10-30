using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDie : MonoBehaviour, Die {
	public Texture2D brokenCarImage;
	public GameObject explosion;
	private bool dead = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void die() {
		if (dead) {
			return;
		}
		if (Random.Range (0, 3) == 0) {
			GameObject exp = GameObject.Instantiate (explosion);
			exp.transform.position = gameObject.transform.position;
			exp.SetActive (true);
			Destroy (exp, 2.4f);
		}
		gameObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create(brokenCarImage, new Rect(0, 0, brokenCarImage.width, brokenCarImage.height), new Vector2(0.5f, 0.5f));
		dead = true;
	}
}
