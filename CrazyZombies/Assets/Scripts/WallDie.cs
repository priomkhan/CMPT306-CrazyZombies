using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDie : MonoBehaviour, Die {
	public Texture2D brokenWallImage;
	public Texture2D weekWallImage;
	private bool dead = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void lowHp() {
		gameObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create(weekWallImage, new Rect(0, 0, weekWallImage.width, weekWallImage.height), new Vector2(0.5f, 0.5f));
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
