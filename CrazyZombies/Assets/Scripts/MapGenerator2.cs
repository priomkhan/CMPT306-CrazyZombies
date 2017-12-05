using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator2 : MonoBehaviour, MapGenerator {
	public GameObject target;
	public Texture2D wallImg;
	public Texture2D floorImg;
	public Texture2D doorImg;
	public GameObject doorObj;
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


	/**
	 * Generate door object at position (x,y) with texture img 
	 */
	private GameObject createDoorGameObject(Texture2D img, float x, float y, bool withCollider,bool isBackground) {
		GameObject go = new GameObject ();
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0f, 0f));
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = sp;
		go.transform.position = new Vector3 (x, y, isBackground ? 10f : 0f);
		go.transform.localScale = new Vector3 (1.6f, 1.6f);
		if (withCollider) {
			BoxCollider2D newCol= go.AddComponent<BoxCollider2D> ();
			newCol.size = new Vector2 (0.65f, 1.5f);
			//newCol.offset = new Vector2 (0.32f, 0.4f);
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
		createGameObject (entranceDoorImg, 12, 0, true, false);
		createGameObject (entranceDoorImg, 13, 0, true, false);
		createGameObject (wallImg, 10, rooms.GetLength (1) * 10 - 1, true, false);
		createGameObject (wallImg, 14, rooms.GetLength (1) * 10 - 1, true, false);
		for (int i = 0; i < 3; i++) {
			GameObject go = createGameObject (exitDoorImg, 11 + i, rooms.GetLength (1) * 10 - 1, true, false);
			DoorScript door = go.AddComponent<DoorScript> ();
			door.color = Color.black;
		}
	}

	private void generateRoomLayout() {
		List<Vector2> roomInfo = new List<Vector2> ();;
		int keys = 5;
		for (int index = 0; index < 5; index++) {
			Vector2 roomPos = new Vector2();
			do {
				int targetRoom = Random.Range (0, rooms.GetLength (0) * rooms.GetLength (1));
				roomPos = new Vector2(targetRoom / rooms.GetLength (1), targetRoom % rooms.GetLength (1));
			} while (rooms[Mathf.FloorToInt(roomPos.x), Mathf.FloorToInt(roomPos.y)] > 0);
			// Lock door
			foreach (Vector2 v in roomInfo) {
				Debug.Log("room :" +  rooms [Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y)]);
				rooms [Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y)] *= 10;
			}

			Debug.Log ("-----");
			// Put key
			rooms [Mathf.FloorToInt (roomPos.x), Mathf.FloorToInt (roomPos.y)] = keys;
			roomInfo.Add(roomPos);

			keys--;
		}
		for (int i = 0; i < roomInfo.Count; i++) {
			Debug.Log("Test:"+rooms [Mathf.FloorToInt (roomInfo [i].x), Mathf.FloorToInt (roomInfo [i].y)].ToString());
		}
	}

	private GameObject generateDoor(int x, int y, Color color) {
		GameObject go = null;
		if (x == 0) {
			go = createDoorGameObject (doorImg, 9, y * 10 + 4.2f, true, false);
			//go = Instantiate(doorObj,new Vector3(x,y,0),doorObj.transform.rotation);
		} else {
			go = createDoorGameObject (doorImg, 15, y * 10 + 4.3f, true, false);
			//go = Instantiate(doorObj,new Vector3(x,y,0),doorObj.transform.rotation);
		}
		go.GetComponent<SpriteRenderer> ().color = color;
		Animator newAnim = go.AddComponent<Animator> ();
		newAnim.runtimeAnimatorController = doorObj.GetComponent<Animator> ().runtimeAnimatorController;
		DoorScript door = go.AddComponent<DoorScript> ();
		door.color = color;
		go.transform.rotation = doorObj.transform.rotation;
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
		go.gameObject.SetActive (true);
		return go;
	}
}
