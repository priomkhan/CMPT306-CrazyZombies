using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator3 : MonoBehaviour, MapGenerator {
	public Texture2D wallImg;
	public Texture2D entranceImg;
	public Texture2D floorImg;
	public Texture2D fenceImg;
	public GameObject target;
	public GameObject[,] map;
	public Texture2D brokenInnerWallImg;
	public int mapSize;
	public GameObject bossObject;

	// Use this for initialization
	void Start () {
		map = new GameObject[mapSize, mapSize];
		generateRoomWallAndFloor ();
		generateBossArea ();
		generateFence (4, Random.Range(0,3));
		generateFence (8, Random.Range(0,3));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 getPlayerRespawn() {
		return new Vector2 (mapSize / 2,1);
	}

	public bool isLengthSet() {
		return true;
	}

	public GameObject[,] detailedMap() {
		return map;
	}

	private void generateRoomWallAndFloor() {
		for (int i = 0; i < map.GetLength (0); i++) {
			int temp = 0;
			if (i == 0) {
				temp = 0;
				createGameObject (wallImg, -1, -1, true, false);
				createGameObject (wallImg, -1, map.GetLength(1), true, false);
			} else if (i == map.GetLength (0) - 1) {
				temp = 2;
				createGameObject (wallImg, map.GetLength (0), -1, true, false);
				createGameObject (wallImg, map.GetLength (0), map.GetLength (1), true, false);
			} else {
				temp = 1;
			}
			createGameObject (wallImg, i, -1, true, false);
			createGameObject (wallImg, i, map.GetLength (1), true, false);
			for (int j = 0; j < map.GetLength (1); j++) {
				if (temp == 0) {
					createGameObject (wallImg, -1, j, true, false);
				} else if (temp == 2) {
					createGameObject (wallImg, map.GetLength(0), j, true, false);
				}
				createGameObject (floorImg, i, j, false, true);
			}
		}
		createGameObject (entranceImg, mapSize / 2 - 1, -1, true, false);
		createGameObject (entranceImg, mapSize / 2, -1, true, false);
		createGameObject (entranceImg, mapSize / 2 + 1, -1, true, false);
	}

	/** 
	 * generate fence divided the room into in and out areas
	 * distance: how far the fence from the room wall
	 * open: the gate direction (0: up, 1: right, 2: bottom, 3: left)
	*/
	private void generateFence(int distance, int open) {
		for (int i = 0; i < mapSize - distance * 2; i++) {
			int skip = -1;
			if ((i >= (mapSize - distance * 2) / 2 - 1 && i <= (mapSize - distance * 2) / 2 + 1)) {
				skip = open;
			}
			if (skip != 2) {
				createFence (distance + i, distance);
			}
			if (skip != 1) {
				createFence (mapSize - distance, distance + i);
			}
			if (skip != 0) {
				createFence (mapSize - distance - i, mapSize - distance);
			}
			if (skip != 3) {
				createFence (distance, mapSize - distance - i);
			}
		}
	}

	private void generateBossArea() {
		target.GetComponent<Rigidbody2D> ().transform.position = new Vector2 (mapSize / 2, mapSize / 2);
		target.SetActive (true);
		bossObject.GetComponent<Rigidbody2D> ().transform.position = new Vector2 (mapSize / 2, mapSize / 2 - 5);
		for (int i = 0; i < 6; i++) {
			if (i < 2 || i > 4) {
				createFence (mapSize / 2 - 3 + i, mapSize / 2 - 3);
			}
			createFence (mapSize / 2 + 3, mapSize / 2 - 3 + i);
			createFence (mapSize / 2 + 3 - i, mapSize / 2 + 3);
			createFence (mapSize / 2 - 3, mapSize / 2 + 3 - i);
		}
	}

	/**
	 * Generate game object at position (x,y) with texture img 
	 */
	private GameObject createGameObject(Texture2D img, float x, float y, bool withCollider,bool isBackground) {
		GameObject go = new GameObject ();
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = sp;
		go.transform.position = new Vector3 (x, y, isBackground ? 10f : 0f);
		go.transform.localScale = new Vector3 (1.6f, 1.6f);
		if (withCollider) {
			go.AddComponent<BoxCollider2D> ();
			addNodes (go);
		}
		go.tag = "object";
		go.layer = 12;
		return go;
	}

	private void addNodes(GameObject gameObject){
		float xPos = gameObject.GetComponent<BoxCollider2D> ().size.x+0.1f;
		float yPos = gameObject.GetComponent<BoxCollider2D> ().size.y+0.1f;
		//Vector3 center = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y,0);
		Vector3 center = gameObject.transform.position;
		Vector3 bottomLeft = center + new Vector3 (-xPos, -yPos,0);
		Vector3 bottomRight = center + new Vector3 (xPos, -yPos,0);
		Vector3 topLeft = center + new Vector3 (-xPos, yPos,0);
		Vector3 topRight = center+ new Vector3 (xPos, yPos,0);

		GameObject nodeBottomLeft = new GameObject ();
		nodeBottomLeft.name = "NodeBottomLeft";
		nodeBottomLeft.transform.position = bottomLeft;
		nodeBottomLeft.transform.parent = gameObject.transform;
		nodeBottomLeft.AddComponent<SpriteRenderer>();
		//enemySpawnPos.GetComponent<SpriteRenderer> ().sprite = (Sprite) Resources.Load ("TestArt/Objects/tile_368/tile_368");
		nodeBottomLeft.GetComponent<SpriteRenderer> ().sprite = GameObject.FindGameObjectWithTag("Node").GetComponent<SpriteRenderer>().sprite;
		nodeBottomLeft.layer = 12;
		nodeBottomLeft.tag = "Node";
		nodeBottomLeft.GetComponent<SpriteRenderer> ().sortingOrder = 0;


		GameObject nodeBottomRight = new GameObject ();
		nodeBottomRight.name = "NodeBottomRight";
		nodeBottomRight.transform.position = bottomRight;
		nodeBottomRight.transform.parent = gameObject.transform;
		nodeBottomRight.AddComponent<SpriteRenderer>();
		nodeBottomRight.GetComponent<SpriteRenderer> ().sprite = GameObject.FindGameObjectWithTag("Node").GetComponent<SpriteRenderer>().sprite;
		nodeBottomRight.layer = 12;
		nodeBottomRight.tag = "Node";
		nodeBottomRight.GetComponent<SpriteRenderer> ().sortingOrder = 0;

		GameObject nodeTopLeft = new GameObject ();
		nodeTopLeft.name = "NodeTopLeft";
		nodeTopLeft.transform.position = topLeft;
		nodeTopLeft.transform.parent = gameObject.transform;
		nodeTopLeft.AddComponent<SpriteRenderer>();
		nodeTopLeft.GetComponent<SpriteRenderer> ().sprite = GameObject.FindGameObjectWithTag("Node").GetComponent<SpriteRenderer>().sprite;
		nodeTopLeft.layer = 12;
		nodeTopLeft.tag = "Node";
		nodeTopLeft.GetComponent<SpriteRenderer> ().sortingOrder = 0;

		GameObject nodeTopRightt = new GameObject ();
		nodeTopRightt.name = "NodeTopRight";
		nodeTopRightt.transform.position = topRight;
		nodeTopRightt.transform.parent = gameObject.transform;
		nodeTopRightt.AddComponent<SpriteRenderer>();
		nodeTopRightt.GetComponent<SpriteRenderer> ().sprite = GameObject.FindGameObjectWithTag("Node").GetComponent<SpriteRenderer>().sprite;
		nodeTopRightt.layer = 12;
		nodeTopRightt.tag = "Node";
		nodeTopRightt.GetComponent<SpriteRenderer> ().sortingOrder = 0;
	}

	private GameObject createFence(int x, int y) {
		GameObject go = createGameObject (fenceImg, x, y, true, false);
		MortalObject hp = go.AddComponent<MortalObject> ();
		WallDie wd = go.AddComponent<WallDie> ();
		wd.brokenWallImage = brokenInnerWallImg;
		hp.hp = 10000;
		go.tag = "fence";
		go.layer = 12;
		return go;
	}
}
