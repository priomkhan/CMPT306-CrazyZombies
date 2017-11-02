using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRun : MonoBehaviour {

	public GameObject player;
	public GameObject target;

	public GameObject spawn1;
	public GameObject spawn2;
	public GameObject spawn3;
	public GameObject spawn4;


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

		Vector2 playerPosition = new Vector2 (Random.Range (0, width), Random.Range (0, height));
		player.GetComponent<Rigidbody2D>().position = playerPosition;
		float minDistance = Mathf.Sqrt (width * width + height * height) / 2;
		Vector2 targetPosition;
		do {
			targetPosition = new Vector2(Random.Range(0, width), Random.Range(0, height));
		} while (Vector2.Distance (playerPosition, targetPosition) < minDistance);
		target.GetComponent<Rigidbody2D> ().position = targetPosition;


		Vector2 spwan1Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
		Vector2 spwan2Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
		Vector2 spwan3Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
		Vector2 spwan4Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));

		spawn1 = new GameObject ();
		spawn2 = new GameObject ();
		spawn3 = new GameObject ();
		spawn4 = new GameObject ();

		GameObject GameController = GameObject.Find("GameController");

		spawn1.name = "spawn1";
		spawn1.transform.position = spwan1Position;
		spawn1.transform.parent = GameController.transform;


		spawn2.name = "spawn2";
		spawn2.transform.position = spwan2Position;
		spawn2.transform.parent = GameController.transform;

		spawn3.name = "spawn3";
		spawn3.transform.position = spwan3Position;
		spawn3.transform.parent = GameController.transform;

		spawn4.name = "spawn4";
		spawn4.transform.position = spwan4Position;
		spawn4.transform.parent = GameController.transform;




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
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = sp;
		go.transform.position = new Vector3 (x, y, 10f);
		go.transform.localScale = new Vector3 (1.6f, 1.6f);
		if (withCollider) {
			go.AddComponent<BoxCollider2D> ();

			go.layer = 11;
			go.tag = "wall";
		} else {
			go.tag = "ground";
			go.layer = 12;
		
		}

		return go;
	}
}
