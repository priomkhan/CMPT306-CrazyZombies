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

	public GameObject spawnDoorObj;

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


				enemySpawnPos = Instantiate(spawnDoorObj,spwanPosition,spawnDoorObj.transform.rotation);
				enemySpawnPos.SetActive (true);
				enemySpawnPos.name = "spawnDoor"+i;
//				enemySpawnPos.transform.position = spwanPosition;
				enemySpawnPos.transform.parent = GameController.transform;
//				enemySpawnPos.AddComponent<SpriteRenderer>();
//				//enemySpawnPos.GetComponent<SpriteRenderer> ().sprite = (Sprite) Resources.Load ("TestArt/Objects/tile_368/tile_368");
//				enemySpawnPos.GetComponent<SpriteRenderer> ().sprite = spawnDoorObj;
//				enemySpawnPos.AddComponent<BoxCollider2D> ();
//				enemySpawnPos.layer = 12;
//				enemySpawnPos.tag = "object";
//				enemySpawnPos.GetComponent<SpriteRenderer> ().sortingOrder = 0;

			}



		}


	
	}
		
}
