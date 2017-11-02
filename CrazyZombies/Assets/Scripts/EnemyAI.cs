using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	public float speed = 4f; // the original speed of the zombie
	public int hitpoints = 3; // how many hits the zombie can take
	public float lowHpThreshHold = 1; // when it is considered having low hp to retreat

	public float enemyLineOfSight = 6f; 	// the range the zombie are away of the zombie. Chosen because it's ~ half the camera distance
	public float dangerZone = 4f;    // The Danger zone of enemy, enemy must attack with out wait for friend.
	public float inRange = 6f; 	// the range the player is to attack it, should be less than the threat zone but not miniscule

	public float friendRange = 7f; 	// the range when friendlies are close enough to help attack. 
									//Chosen based on zombie size, only other zombie in range will help chase
									// down the player
									// Use this for initialization
	//public float monsterTooFarFromSpawn = 5f; 	// about a quarter of the map, allows zombie to "control a corner" of the map if the player
												// is out of range.

	public int enemyGathering = 2;



	delegate void MyDelegate();
	MyDelegate enemyAction;

	float distanceOfPlayer;
	bool targetPlayer = false;
	bool runFromPlayer = false;
	GameObject player;
	UnityEngine.Color originalColor;

	float velocity;



	void Start () {
		player = GameObject.FindGameObjectWithTag ("player");
		originalColor = GetComponent<SpriteRenderer>().color;

		if (player != null) {
			//Debug.Log ("Player Found");
			enemyAction = TargetInZone;
		} else {
			Debug.Log ("Player Referance Missing");
		}

		InvokeRepeating ("Wait_decision", 1, 1);

	}
	
	// Update is called once per frame
	void Update () {

		distanceOfPlayer = Vector3.Distance (this.gameObject.transform.position, player.transform.position);


		if (targetPlayer) {

			Vector2 v = new Vector2 ();
			v.x = player.transform.position.x - transform.position.x;
			v.y = player.transform.position.y - transform.position.y;
			if (v.magnitude != 0) {
				if (runFromPlayer) {
					Vector2 v2 = new Vector2 ();
					v2.x = -v.x * speed / v.magnitude * Time.deltaTime;
					v2.y = -v.y * speed / v.magnitude * Time.deltaTime;

					GetComponent<Rigidbody2D> ().velocity = v2;
				} else {
					Vector2 v2 = new Vector2 ();
					v2.x = v.x * speed / v.magnitude * Time.deltaTime;
					v2.y = v.y * speed / v.magnitude * Time.deltaTime;

					GetComponent<Rigidbody2D> ().velocity = v2;
				}

			}

			Vector2 dir = player.transform.position - transform.position;
			if (dir != Vector2.zero) {
				float angle;
				if (runFromPlayer) {
					angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 180;
				} else {
					angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				}

				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);	

			}




		}
	}


	void FixedUpdate () {
	

			

	}


	public void zombieHit(){
		hitpoints--;
	}

	public int getHP(){
		return hitpoints;
	}

	public void attackPlayer(){
		if (distanceOfPlayer < enemyLineOfSight && !runFromPlayer) {
			enemyAction = attack;
		}
	}


	/* ............ Decisions............. */

	// is a the target within the range of line of sight ?
	private void TargetInZone()
	{
		if (distanceOfPlayer <= dangerZone) {
			//Debug.Log ("Target: Player in Danger Zone : True");
			enemyAction = attack;

		}

		if (distanceOfPlayer > dangerZone && distanceOfPlayer < enemyLineOfSight ) {
			//Debug.Log ("Target: Player in Range: True");
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
			enemyAction = InRange;
		} 
		else
		{
			//Debug.Log("Target: Player in Range: False");
			this.gameObject.GetComponent<SpriteRenderer> ().color = originalColor;
			enemyAction = tooFarFromSpawn;
		}

	}


	private void tooFarFromSpawn(){
		
		enemyAction = TargetInZone;
	}


	private void InRange(){
		if (distanceOfPlayer < inRange) {
			//Debug.Log("inRange True");
			enemyAction = friendsNear;
		} 
		else {
			//Debug.Log("inRange False");
			enemyAction = hpLow;
		}
	}


	private void friendsNear(){
		if (isFriendClose()) {
			//Debug.Log("Friends Near True");
			Debug.Log("Lets Attack the Player............");
			enemyAction = attack;
		} else {
			//Debug.Log("Friends Near False");
			Debug.Log("Can not attack now");
			enemyAction = hpLow;
		}
	}

	private bool isFriendClose(){
		GameObject[] enemy = GameObject.FindGameObjectsWithTag ("enemy");
		int noEnemyFriendNearby = 0;
		foreach (GameObject e in enemy){
			if (e != this.gameObject){
				if (Vector3.Distance (e.gameObject.transform.position, this.transform.position) < friendRange) {
					//Debug.Log("Found Enemy Friend");
					noEnemyFriendNearby = noEnemyFriendNearby+1;
					Debug.Log("No of Enemy Friends found: "+ noEnemyFriendNearby);
				}
			}
		}
		if (noEnemyFriendNearby > enemyGathering) {
			
			return true;
		}

		return false;
	}


	private void hpLow(){
		if (hitpoints > lowHpThreshHold) {
			Debug.Log("HP Low False");
			enemyAction = attack; // again because attack/advance are samesies.
		} 
		else 
		{
			Debug.Log("HP Low True");
			enemyAction = retreat;
		}
	}



	private void retreat(){
		this.gameObject.GetComponent<SpriteRenderer> ().color = Color.gray;
		Debug.Log("Retreating");
		targetPlayer = true;
		runFromPlayer = true;
		velocity = speed;	
		enemyAction = TargetInZone;
	}


	private void attack(){
		this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		// code to target player
		Debug.Log("Attacking");
		runFromPlayer = false;
		targetPlayer = true;
		enemyAction = TargetInZone;
	}

	// takes longer to make decisions for demonstration purposes
	private void Wait_decision()
	{
		enemyAction();
	}
}
