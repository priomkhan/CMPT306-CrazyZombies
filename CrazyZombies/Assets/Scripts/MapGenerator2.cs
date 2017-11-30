﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator2 : MonoBehaviour, MapGenerator {
	public GameObject target;
	public Texture2D wallImg;
	public Texture2D floorImg;
	public Texture2D doorImg;
	public Texture2D keyImg;
	public Texture2D entranceDoorImg;
	public Texture2D exitDoorImg;
	private int[,] rooms;
	private bool ready = false;
	private GameObject[,] detailMap;

	// Use this for initialization
	void Start () {
		int height = Random.Range (3, 5);
		rooms = new int[2, height];
		detailMap = new GameObject[25, height * 10];

		generateRoomLayout ();
		generateFloor();
		for (int i = 0; i < height; i++) {
			generateRoom (0, i);
			generateRoom (1, i);
		}
		generateHallway ();

		ready = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 getPlayerRespawn() {
		return new Vector2 (13, 1);
	}

	public bool isLengthSet() {
		return ready;
	}

	public GameObject[,] detailedMap() {
		return detailMap;
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

	private void generateRoom(int x, int y) {
		Vector2 roomPos = new Vector2 ();
		roomPos.x = x == 0 ? 0 : 15;
		roomPos.y = y * 10;

		Color[] colors = new Color[4];
		colors [0] = Color.green;
		colors [1] = Color.blue;
		colors [2] = Color.yellow;
		colors [3] = Color.red;
		int doorId = 0;
		int keyId = rooms [x, y];
		while (keyId > 9) {
			keyId /= 10;
			doorId++;
		}

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
				if (doorId > 0) {
					generateDoor (x, y, colors[doorId - 1]);
				}
			} else {
				createGameObject (wallImg, roomPos.x, i, true, false);
				createGameObject (wallImg, roomPos.x + 9, i, true, false);
			}
		}
		if (keyId == 5 || keyId < 0) {
			target.GetComponent<Rigidbody2D> ().position = new Vector2 (roomPos.x + Random.Range (1, 9), roomPos.y + Random.Range (1, 9));
		} else if (keyId > 0) {
			generateKey (x, y, colors[keyId - 1]);
		}
		Debug.Log ("RoomId:" + x + "," + y + " key: " + keyId + " door: " + doorId); 

	}

	private void generateHallway() {
		createGameObject (wallImg, 10, 0, true, false);
		createGameObject (wallImg, 14, 0, true, false);
		createGameObject (entranceDoorImg, 11, 0, true, false);
		createGameObject (wallImg, 10, rooms.GetLength (1) * 10 - 1, true, false);
		createGameObject (wallImg, 14, rooms.GetLength (1) * 10 - 1, true, false);
		GameObject go = createGameObject (exitDoorImg, 11, rooms.GetLength (1) * 10 - 1, true, false);
		DoorScript door = go.AddComponent<DoorScript> ();
		door.color = Color.black;
	}

	private void generateRoomLayout() {
		Vector2[] roomInfo = new Vector2[4];
		int keys = 5;
		for (int index = 0; index <= roomInfo.GetLength (0); index++) {
			Vector2 roomPos = new Vector2();
			do {
				int targetRoom = Random.Range (0, rooms.GetLength (0) * rooms.GetLength (1));
				roomPos = new Vector2(targetRoom / rooms.GetLength (1), targetRoom % rooms.GetLength (1));
			} while (rooms[Mathf.FloorToInt(roomPos.x), Mathf.FloorToInt(roomPos.y)] > 0);
			// Lock door
			for (int i = 0; i < roomInfo.GetLength (0); i++) {
				if (roomInfo [i] != null) {
					rooms [Mathf.FloorToInt (roomInfo [i].x), Mathf.FloorToInt (roomInfo [i].y)] *= 10;
				}
			}
			// Put key
			rooms [Mathf.FloorToInt (roomPos.x), Mathf.FloorToInt (roomPos.y)] = keys;
			if (index != roomInfo.GetLength(0)) {
				roomInfo [index] = roomPos;
			}

			keys--;
		}
//		for (int i = 0; i < roomInfo.GetLength (0); i++) {
//			Debug.Log("Test:"+rooms [Mathf.FloorToInt (roomInfo [i].x), Mathf.FloorToInt (roomInfo [i].y)].ToString());
//		}
	}

	private GameObject generateDoor(int x, int y, Color color) {
		GameObject go = null;
		if (x == 0) {
			go = createGameObject (doorImg, 9, y * 10 + 4, true, false);
		} else {
			go = createGameObject (doorImg, 15, y * 10 + 4, true, false);
		}
		go.GetComponent<SpriteRenderer> ().color = color;
		DoorScript door = go.AddComponent<DoorScript> ();
		door.color = color;
		return go;
	}

	private GameObject generateKey(int x, int y, Color color) {
		GameObject go = null;
		if (x == 0) {
			go = createGameObject (keyImg, Random.Range (1, 9), y * 10 + Random.Range (1, 9), true, false);
		} else {
			go = createGameObject (keyImg, 15 + Random.Range (1, 9), y * 10 + Random.Range (1, 9), true, false);
		}
		go.GetComponent<SpriteRenderer> ().color = color;
		KeyScript key = go.AddComponent<KeyScript> ();
		key.color = color;
		return go;
	}
}