using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRun : MonoBehaviour {

	public GameObject player;
	public GameObject target;

//	public GameObject spawn1;
//	public GameObject spawn2;
//	public GameObject spawn3;
//	public GameObject spawn4;

	public GameObject enemySpawnPos;

	public int levelDeficulty = 1;

	bool RunOnce = false;

	private MapGenerator mapGenerator;

	public Sprite enemySpawnPosition;

	int width=0;
	int height=0;

	// Use this for initialization
	void Start () {
		mapGenerator = this.gameObject.GetComponent<MapGenerator>();




	}
	
	// Update is called once per frame
	void Update () {


		if (!RunOnce && mapGenerator.isLengthSet()) {
			createEnemySpawnPosition ();
			player.GetComponent<Rigidbody2D>().position = mapGenerator.getPlayerRespawn();
			RunOnce = true;
		}
	}


	void createEnemySpawnPosition(){

		if (mapGenerator.isLengthSet ()) {
			width = mapGenerator.detailedMap ().GetLength (0);
			height = mapGenerator.detailedMap ().GetLength (1);
			Debug.Log ("Width:" + width);
			Debug.Log ("Height:" + height);


			GameObject GameController = GameObject.Find ("GameController");

			int numberOfSpawnPos = ((levelDeficulty + 1) * 5);


			for (int i = 0; i <= numberOfSpawnPos; i++) {
				Vector2 spwanPosition = new Vector2 (Random.Range (0, width), Random.Range (0, height));	
				enemySpawnPos = new GameObject ();

				enemySpawnPos.name = "spawn"+i;
				enemySpawnPos.transform.position = spwanPosition;
				enemySpawnPos.transform.parent = GameController.transform;
				enemySpawnPos.AddComponent<SpriteRenderer>();
				//enemySpawnPos.GetComponent<SpriteRenderer> ().sprite = (Sprite) Resources.Load ("TestArt/Objects/tile_368/tile_368");
				enemySpawnPos.GetComponent<SpriteRenderer> ().sprite = enemySpawnPosition;
				enemySpawnPos.layer = 13;
				enemySpawnPos.tag = "ground";
				enemySpawnPos.GetComponent<SpriteRenderer> ().sortingOrder = 0;

			}






		
//			Vector2 spwan1Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
//			Vector2 spwan2Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
//			Vector2 spwan3Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
//			Vector2 spwan4Position = new Vector2 (Random.Range (0, width), Random.Range (0, height));
//
//			spawn1 = new GameObject ();
//			spawn2 = new GameObject ();
//			spawn3 = new GameObject ();
//			spawn4 = new GameObject ();
//
//			GameObject GameController = GameObject.Find ("GameController");
//
//			spawn1.name = "spawn1";
//			spawn1.transform.position = spwan1Position;
//			spawn1.transform.parent = GameController.transform;
//
//
//			spawn2.name = "spawn2";
//			spawn2.transform.position = spwan2Position;
//			spawn2.transform.parent = GameController.transform;
//
//			spawn3.name = "spawn3";
//			spawn3.transform.position = spwan3Position;
//			spawn3.transform.parent = GameController.transform;
//
//			spawn4.name = "spawn4";
//			spawn4.transform.position = spwan4Position;
//			spawn4.transform.parent = GameController.transform;


		}


	
	}
		
}
