using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRun : MonoBehaviour {

	public GameObject player;
	public Texture2D groundImg;
	public Texture2D bottomLeftBorderImg;
	public Texture2D bottomTopBorderImg;
	public Texture2D bottomRightBorderImg;
	public Texture2D leftRightBorderImg;
	public Texture2D topRightBorderImg;
	public Texture2D topLeftBorderImg;
	public int width;
	public int height;

	// Use this for initialization
	void Start () {
		generateMap ();

		player.GetComponent<Rigidbody2D>().position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
/*		int x = 0;
		int y = 0;
		while (x < 80 && y < 10) {
			GameObject obj = Instantiate (object1);	
			Vector2 v;
			do {
				v = new Vector2 (100, 100);
				Debug.Log(v);
			} while (Physics2D.OverlapArea (v, v) == null);

			//obj.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			obj.GetComponent<Rigidbody2D> ().position = v;
			//obj.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
			x = Random.Range (0, 100);
			y++;
		}*/

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void generateMap() {
		createGameObject (bottomLeftBorderImg, -1, -1, true);
		createGameObject (bottomRightBorderImg, width, -1, true);
		createGameObject (topLeftBorderImg, -1, height, true);
		createGameObject (topRightBorderImg, width, height, true);
		for (int i = 0; i < width; i++) {
			if (i == 0) {
				for (int j = 0; j < height; j++) {
					createGameObject (leftRightBorderImg, -1, j, true);
				}
			} else if (i == width - 1) {
				for (int j = 0; j < height; j++) {
					createGameObject (leftRightBorderImg, width, j, true);
				}
			}
			createGameObject (bottomTopBorderImg, i, -1, true);
			createGameObject (bottomTopBorderImg, i, height, true);
			for (int j = 0; j < height; j++) {
				createGameObject(groundImg, i ,j, false);
			}
		}
	}

	private GameObject createGameObject(Texture2D img, float x, float y, bool withCollider) {
		GameObject go = new GameObject ();
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0f, 0f));
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = sp;
		go.transform.position = new Vector3 (x, y, 10f);
		go.transform.localScale = new Vector3 (1.6f, 1.6f);
		if (withCollider) {
			go.AddComponent<BoxCollider2D> ();
		}
		return go;
	}
}
