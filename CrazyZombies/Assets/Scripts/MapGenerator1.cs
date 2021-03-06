﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator1 : MonoBehaviour, MapGenerator {

	public GameObject target;
	public GameObject explosion;
	public Texture2D outGroundImg;
	public Texture2D inGroundImg;
	public Texture2D[] objectImgs;
	private float[] objectPossibility;	public Texture2D outWallImg;
	public Texture2D weakOutWallImg;
	public Texture2D brokenOutWallImg;
	public Texture2D innerWallImg;
	public Texture2D weakInnerWallImg;
	public Texture2D brokenInnerWallImg;
	public Texture2D[] carImgs;
	public Texture2D[] brokenCarImgs;
	private bool ready = false;
	private Vector2 playerRespawn;
	private int[,] map;
	private GameObject[,] detailMap;

	// Use this for initialization
	void Start () {
		int width = Random.Range (8, 11);
		int height = Random.Range (8, 11);
		objectPossibility = new float[objectImgs.GetLength(0)];
		for (int i = 0; i < objectImgs.GetLength(0); i++) {
			objectPossibility [i] = 640 * 640 / (objectImgs [0].height * objectImgs [0].width);
		}


		map = new int[width,height];
		detailMap = new GameObject[width * 10, height * 10];
		generateCriticalArea (map);
		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				if (map [i, j] != 0) {
					continue;
				}
				generateBlockMap (i, j, map, false);
			}
		}
		generateBorder (map);

		ready = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public Vector2 getPlayerRespawn() {
		return playerRespawn;
	}

	public GameObject[,] detailedMap() {
		return detailMap;
	}

	public bool isLengthSet(){
		return ready;
	}

	/**
	 * Generate game object at position (x,y) with texture img 
	 */
	private GameObject createGameObject(Texture2D img, float x, float y, bool withCollider,bool isBackground, bool withNode) {
		GameObject go = new GameObject ();
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = sp;
		go.transform.position = new Vector3 (x, y, isBackground ? 10f : 0f);
		go.transform.localScale = new Vector3 (1.6f, 1.6f);
		if (withCollider) {
			go.AddComponent<BoxCollider2D> ();
			if (withNode) {
			addNodes (go);//Adding Nodes for path finding
			}
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
	 * generate 10*10 block of map
	 */
	private void generateBlockMap(int x, int y, int[,] map, bool inner) {
		map [x, y] = 1;
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				detailMap[x * 10 + i, y * 10 + j] = createGameObject (inner ? inGroundImg : outGroundImg, x * 10 + i, y * 10 + j, false, true, false);
			}
		}
		if (!inner) {
			int numOfObject = Random.Range (2, 4);
			for (int i = 0; i < numOfObject; i++) {
				if (Random.Range (0, 5) == 0) {
					int a = Random.Range (0, 10);
					int b = Random.Range (0, 10);
					detailMap [x * 10 + a, y * 10 + b] = generateCar (x * 10 + a, y * 10 + b, Random.Range (0, 180));
				} else {
					int a = Random.Range (0, 10);
					int b = Random.Range (0, 10);
					GameObject go = generateObject (x, y, a, b);
					go.transform.Rotate(0,0,Random.Range(0,180));
					detailMap [x * 10 + a, y * 10 + b] = go;
				}
			}
		}

	}






	/**
	 * generate hospital area, inner wall and out wall
	 */
	private void generateCriticalArea(int[,] map) {
		int hospital = Random.Range (0, 4);
		int outWall = 0;
		// Setting hospital area
		switch (hospital) {
		case(0):
			target.transform.position = new Vector3 (5, 5, 0);
			generateBlockMap (0, 0, map, true);
			generateBlockMap (1, 0, map, true);
			generateBlockMap (1, 1, map, true);
			generateBlockMap (0, 1, map, true);
			generateWall (2, 0, 6, map, true);
			generateWall (2, 1, 6, map, true);
			generateWall (2, 2, 7, map, true);
			generateWall (1, 2, 0, map, true);
			generateWall (0, 2, 0, map, true);
			outWall = Random.Range (0, 2) == 0 ? 2 : 1;
			break;
		case(1):
			target.transform.position = new Vector3 ((map.GetLength (0) - 1) * 10 + 5, 5, 0);
			generateBlockMap (map.GetLength (0) - 1, 0, map, true);
			generateBlockMap (map.GetLength(0) - 2, 0, map, true);
			generateBlockMap (map.GetLength(0) - 2, 1, map, true);
			generateBlockMap (map.GetLength(0) - 1, 1, map, true);
			generateWall (map.GetLength(0) - 3, 0, 2, map, true);
			generateWall (map.GetLength(0) - 3, 1, 2, map, true);
			generateWall (map.GetLength(0) - 3, 2, 1, map, true);
			generateWall (map.GetLength(0) - 2, 2, 0, map, true);
			generateWall (map.GetLength(0) - 1, 2, 0, map, true);
			outWall = Random.Range (0, 2) == 0 ? 3 : 2;
			break;
		case(2):
			target.transform.position = new Vector3 ((map.GetLength (0) - 1) * 10 + 5, (map.GetLength (1) - 1) * 10 + 5, 0);
			generateBlockMap (map.GetLength (0) - 1, map.GetLength (1) - 1, map, true);
			generateBlockMap (map.GetLength(0) - 2, map.GetLength(1) - 1, map, true);
			generateBlockMap (map.GetLength(0) - 2, map.GetLength(1) - 2, map, true);
			generateBlockMap (map.GetLength(0) - 1, map.GetLength(1) - 2, map, true);
			generateWall (map.GetLength(0) - 3, map.GetLength(1) - 1, 2, map, true);
			generateWall (map.GetLength(0) - 3, map.GetLength(1) - 2, 2, map, true);
			generateWall (map.GetLength(0) - 3, map.GetLength(1) - 3, 3, map, true);
			generateWall (map.GetLength(0) - 2, map.GetLength(1) - 3, 4, map, true);
			generateWall (map.GetLength(0) - 1, map.GetLength(1) - 3, 4, map, true);
			outWall = Random.Range (0, 2) == 0 ? 0 : 3;
			break;
		case(3):
			target.transform.position = new Vector3 (5, (map.GetLength (1) - 1) * 10 + 5, 0);
			generateBlockMap (0, map.GetLength (1) - 1, map, true);
			generateBlockMap (1, map.GetLength(1) - 1, map, true);
			generateBlockMap (1, map.GetLength(1) - 2, map, true);
			generateBlockMap (0, map.GetLength(1) - 2, map, true);
			generateWall (2, map.GetLength(1) - 1, 6, map, true);
			generateWall (2, map.GetLength(1) - 2, 6, map, true);
			generateWall (2, map.GetLength(1) - 3, 5, map, true);
			generateWall (1, map.GetLength(1) - 3, 4, map, true);
			generateWall (0, map.GetLength(1) - 3, 4, map, true);
			outWall = Random.Range (0, 2) == 0 ? 0 : 1;
			break;
		}
		// Setting out wall
		switch (outWall) {
		case(0):
			for (int i = 0; i < map.GetLength (0); i++) {
				generateWall (i, 1, 4, map, false);
			}
			playerRespawn = new Vector2 (Random.Range (0, map.GetLength(0)) * 10 + Random.Range (0, 10), Random.Range (0, 10));
			break;
		case(1):
			for (int i = 0; i < map.GetLength (1); i++) {
				generateWall (map.GetLength(0) - 2, i, 6, map, false);
			}
			playerRespawn = new Vector2 ((map.GetLength(0) - 1) * 10 + Random.Range (0, 10), Random.Range (0, map.GetLength(1)) * 10 + Random.Range (0, 10));
			break;
		case(2):
			for (int i = 0; i < map.GetLength (0); i++) {
				generateWall (i, map.GetLength(1) - 2, 0, map, false);
			}
			playerRespawn = new Vector2 (Random.Range (0, map.GetLength(0)) * 10 + Random.Range (0, 10), (map.GetLength(1) - 1) * 10 + Random.Range (0, 10));
			break;
		case(3):
			for (int i = 0; i < map.GetLength (1); i++) {
				generateWall (1, i, 2, map, false);
			}
			playerRespawn = new Vector2 (Random.Range (0, 10), Random.Range (0, map.GetLength(1)) * 10 + Random.Range (0, 10));
			break;
		}
	}

	/**
	 * generate inner wall at (x,y) block
	 * in int x: position x
	 * in int y: position y
	 * in in dir: where wall is, 0 up 1 up-right 2 right 3 bottom-right 4 bottom 5 bottom-left 6 left 7 up-left
	 */
	private void generateWall(int x, int y, int dir, int[,] map, bool inner) {
		map [x, y] = 1;
		Texture2D wallImg = inner ? innerWallImg : outWallImg;
		switch(dir) {
		case(0):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (j == 0) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}

				}
			}
			for (int i = 0; i < 10; i++) {
				if (i % 2 == 0) {
					if (Random.Range (0, 5) == 0) {
						detailMap [x * 10 + i, y * 10 + 2] = generateCar (x * 10 + i, y * 10 + 2, 0);
					}
				} else {
					generateLine (x * 10 + i, y * 10, x * 10 + i, y * 10 + 4);
				}
			}
			break;
		case(1):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (i == 9 && j == 0) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}

				}
			}
			break;
		case(2):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false,true, false);
					if (i == 9) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}
					
				}
			}
			for (int i = 0; i < 10; i++) {
				if (i % 2 == 0) {
					if (Random.Range (0, 5) == 0) {
						detailMap [x * 10 + 7, y * 10 + i] = generateCar (x * 10 + 7, y * 10 + i, 90);
					}
				} else {
					generateLine (x * 10 + 9, y * 10 + i, x * 10 + 5, y * 10 + i);
				}
			}
			break;
		case (3):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (i == 9 && j == 9) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}
				}
			}
			break;
		case (4):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (j == 9) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}
				}
			}
			for (int i = 0; i < 10; i++) {
				if (i % 2 == 0) {
					if (Random.Range (0, 5) == 0) {
						detailMap [x * 10 + i, y * 10 + 7] = generateCar (x * 10 + i, y * 10 + 7, 180);
					}
				} else {
					generateLine (x * 10 + i, y * 10 + 9, x * 10 + i, y * 10 + 5);
				}
			}
			break;
		case(5):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (i == 0 && j == 9) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}
				}
			}
			break;
		case(6):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (i == 0) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}
				}
			}
			for (int i = 0; i < 10; i++) {
				if (i % 2 == 0) {
					if (Random.Range (0, 5) == 0) {
						detailMap [x * 10 + 2, y * 10 + i] = generateCar (x * 10 + 2, y * 10 + i, 270);
					}
				} else {
					generateLine (x * 10, y * 10 + i, x * 10 + 4, y * 10 + i);
				}
			}
			break;
		case(7):
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					detailMap [x * 10 + i, y * 10 + j] = createGameObject (inGroundImg, x * 10 + i, y * 10 + j, false, true, false);
					if (i == 0 && j == 0) {
						detailMap [x * 10 + i, y * 10 + j] = generateWall (x * 10 + i, y * 10 + j, inner);
					}
				}
			}
			break;
		}


	}

	private void generateBorder(int[,] map) {
		for (int i = 0; i < map.GetLength(1) * 10; i++) {
			createGameObject (outWallImg, -1, i, true, true, false);
			createGameObject (outWallImg, map.GetLength (0) * 10, i, true, true, false);
		}
		for (int i = 0; i < map.GetLength (0) * 10; i++) {
			createGameObject (outWallImg, i, -1, true, true, false);
			createGameObject (outWallImg, i, map.GetLength (1) * 10, true, true, false);
		}
	}

	private GameObject generateCar(float x, float y, float rotation) {
		int imgIndex = Random.Range (0, carImgs.GetLength (0));
		GameObject go = createGameObject (carImgs [imgIndex], x, y, true, false, true);
		Rigidbody2D rigid = go.AddComponent<Rigidbody2D> ();
		rigid.mass = 1000;
		rigid.gravityScale = 0;
		rigid.drag = 10;
		rigid.angularDrag = 10;
		MortalObject hp = go.AddComponent<MortalObject> ();
		hp.hp = 10;
		go.transform.Rotate (0, 0, rotation);

		CarDie cd = go.AddComponent<CarDie> ();
		cd.explosion = explosion;
		cd.brokenCarImage = brokenCarImgs[imgIndex];
		go.tag = "car";
		return go;
	}
		
	private GameObject generateWall(float x, float y, bool inner) {
		GameObject go = createGameObject (inner ? innerWallImg : outWallImg, x, y, true, false, true);
		MortalObject hp = go.AddComponent<MortalObject> ();
		hp.hp = Random.Range (0, 3) == 0 ? 10 : 10000;
		WallDie wd = go.AddComponent<WallDie> ();
		wd.brokenWallImage = inner ? brokenInnerWallImg : brokenOutWallImg;
		wd.weekWallImage = inner ? weakInnerWallImg : weakOutWallImg;
		go.tag = "wall";
		return go;
	}

	private GameObject generateLine(float x1, float y1, float x2, float y2) {
		GameObject go = new GameObject ();
		LineRenderer lr = go.AddComponent<LineRenderer> ();
		lr.SetPosition (0, new Vector3 (x1, y1));
		lr.SetPosition (1, new Vector3 (x2, y2));
		lr.startWidth = 0.3f;
		lr.endWidth = 0.3f;
		lr.material = new Material (Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.startColor = Color.white;
		lr.endColor = Color.white;
		return go;
	}

	private GameObject generateObject(int x, int y, int a, int b) {
		float sum = objectPossibility.Sum ();
		float rand = Random.Range (0, sum);
		Texture2D img = null;
		for (int i = 0; i < objectPossibility.GetLength(0); i++) {
			rand = rand - objectPossibility [i];
			if (rand <= 0) {
				img = objectImgs [i];
				break;
			}
		}

		GameObject go = createGameObject (img, x * 10 + a, y * 10 + b, true, false, true);
		return go;
	}
}
