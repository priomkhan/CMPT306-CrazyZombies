using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {


	//public Transform[] spawnPoints;


	public Transform[] spawnPoints;
	public GameObject[] childs;
	int childCount=0;

	public float spawnTime = 5f;
	public float waitTime = 5f;
	public float spawnRateIncrease = 0.1f;
	public GameObject[] enemyTypes;


	public Text scoreText;
	private int score;

	void Awake(){
	

	}


	// Use this for initialization
	void Start () {

//		Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
//		int i = 0;
//		int length = ts.Length;
//		Debug.Log ("Spwan array size: "+length);
//		if (ts == null) {
//			Debug.Log ("Spwan Position not found");
//		}
//		spawnPoints = new Transform[length];
//		foreach (Transform t in ts) {
//			
//			if (t != null && t.transform != null) {
//				Debug.Log ("Spwan" + i + "Position adding");
//				spawnPoints [i] = t.transform;
//				i++;
//			}
//		}

	

//
//		childs = gameObject.GetComponentsInChildren<>();
//		Debug.Log ("Spwan" + childs.Length + "Position adding");
//		spawnPoints = new Transform[childs.Length];
//		foreach(GameObject child in childs) {
//				value++;
//			spawnPoints.SetValue (child.transform, value - 1);
//			}






//
//		childCount = transform.childCount;
//
//		if (childCount > 0) {
//			spawnPoints = new Transform[childCount];
//
//			for (var i = childCount - 1; i >= 0; i--) {
//
//				spawnPoints.SetValue (transform.GetChild (i), i);
//			}
//		} else {
//
//			Debug.Log ("Spwan List Not initiated");
//
//			spawnPoints = gameObject.GetComponentsInChildren<Transform>();
//		}













		InvokeRepeating("Spawn", spawnTime, spawnTime);
		StartCoroutine(increaseSpawnRate());
	}

	IEnumerator increaseSpawnRate() // increases the rate of spawn every 5 seconds
	{
		yield return new WaitForSeconds(waitTime);
		if (spawnTime > 1.5f)
		{
			spawnTime -= spawnRateIncrease;
			StartCoroutine(increaseSpawnRate());
		}

	}

	GameObject randomEnemyCharecter(){
		int number = Random.Range(0, enemyTypes.Length);

		Debug.Log ("Random Number: " + number);

		return enemyTypes[number];
	
	}

	void Spawn()
	{
		int spawnPointIndex = Random.Range(0, spawnPoints.Length);
		Animator anim = spawnPoints [spawnPointIndex].transform.GetComponent<Animator> ();
		anim.SetBool ("spawnDoorOpen", true);
		GameObject enemyClone = Instantiate(randomEnemyCharecter(), spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		enemyClone.gameObject.SetActive(true);
		anim.SetBool ("spawnDoorOpen", false);
	}


	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score.ToString();
	}

	// Update is called once per frame
	void FixedUpdate () {

		childCount = transform.childCount;

		if (childCount > 0) {
			spawnPoints = new Transform[childCount];

			for (var i = childCount - 1; i >= 0; i--) {

				spawnPoints.SetValue (transform.GetChild (i), i);
			}
		} else {

			Debug.Log ("Spwan List Not initiated");

			spawnPoints = gameObject.GetComponentsInChildren<Transform>();
		}

	}

}
