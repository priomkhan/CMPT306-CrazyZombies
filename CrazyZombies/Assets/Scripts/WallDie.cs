using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDie : MonoBehaviour, Die {
	public Texture2D brokenWallImage;
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
		Destroy(gameObject.GetComponent<BoxCollider2D>());
		gameObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create(brokenWallImage, new Rect(0, 0, brokenWallImage.width, brokenWallImage.height), new Vector2(0.5f, 0.5f));
		dead = true;
	}
}
