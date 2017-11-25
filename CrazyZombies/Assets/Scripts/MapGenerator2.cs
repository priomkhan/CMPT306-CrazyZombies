using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator2 : MonoBehaviour {

	public Texture2D wallImg;
	public Texture2D floorImg;
	public Texture2D doorImg;
	public Texture2D entranceDoorImg;
	public Texture2D exitDoorImg;
	private int[,] rooms;

	// Use this for initialization
	void Start () {
		int height = Random.Range (3, 5);
		rooms = new int[2, height];

		generateFloor();
		for (int i = 0; i < height; i++) {
			generateRoomWall (0, i);
			generateRoomWall (1, i);
		}
		generateHallway ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Generate game object at position (x,y) with texture img 
	 */
	private GameObject createGameObject(Texture2D img, float x, float y, bool withCollider,bool isBackground) {
		GameObject go = new GameObject ();
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0f, 0f));
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = sp;
		go.transform.position = new Vector3 (x, y, isBackground ? 10f : 0f);
		go.transform.localScale = new Vector3 (1.6f, 1.6f);
		if (withCollider) {
			go.AddComponent<BoxCollider2D> ();
		}
		go.tag = "object";
		go.layer = 12;
		return go;
	}

	private void generateFloor() {
		int width = 25;
		int height = rooms.GetLength (1) * 10;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				createGameObject (floorImg, i, j, false, true);
			}
		}
	}

	private void generateRoomWall(int x, int y) {
		Vector2 roomPos = new Vector2 ();
		roomPos.x = x == 0 ? 0 : 15;
		roomPos.y = y * 10;
		for (int i = Mathf.FloorToInt(roomPos.y); i < roomPos.y + 10; i++) {
			if (i == Mathf.FloorToInt(roomPos.y) || i == Mathf.FloorToInt(roomPos.y) + 9) {
				for (int j = Mathf.FloorToInt(roomPos.x); j < roomPos.x + 10; j++) {
					createGameObject (wallImg, j, i, true, false);
				}
			} else if (i == Mathf.FloorToInt(roomPos.y) + 4 || i == Mathf.FloorToInt(roomPos.y) + 5) {
				if (x == 0) {
					createGameObject (wallImg, roomPos.x, i, true, false);
				} else {
					createGameObject (wallImg, roomPos.x + 9, i, true, false);
				}
			} else {
				createGameObject (wallImg, roomPos.x, i, true, false);
				createGameObject (wallImg, roomPos.x + 9, i, true, false);
			}

		}
	}

	private void generateHallway() {
		createGameObject (wallImg, 10, 0, true, false);
		createGameObject (wallImg, 14, 0, true, false);
		createGameObject (entranceDoorImg, 12, 0, true, false);
		createGameObject (wallImg, 10, rooms.GetLength (1) * 10 - 1, true, false);
		createGameObject (wallImg, 14, rooms.GetLength (1) * 10 - 1, true, false);
		createGameObject (exitDoorImg, 12, rooms.GetLength (1) * 10 - 1, true, false);
	}

	private void generateRoomConent(int x, int y) {
		Vector2 roomPos = new Vector2 ();
		roomPos.x = x == 0 ? 0 : 15;
		roomPos.y = y * 10;
	}
}
