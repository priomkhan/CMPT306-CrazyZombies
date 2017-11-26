using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, Die {

	//	public AudioClip deadZombieSound;
	//	public float speed = 40f; // the original speed of the zombie
	////	public int hitpoints = 3; // how many hits the zombie can take
	////	public float lowHpThreshHold = 1; // when it is considered having low hp to retreat
	//	private bool dead;
	//
	//	public float enemyLineOfSight = 6f; 	// the range the zombie are away of the zombie. Chosen because it's ~ half the camera distance
	//	public float dangerZone = 4f;    // The Danger zone of enemy, enemy must attack with out wait for friend.
	//	public float inRange = 6f; 	// the range the player is to attack it, should be less than the threat zone but not miniscule
	//
	//	public float friendRange = 6f; 	// the range when friendlies are close enough to help attack. 
	//									//Chosen based on zombie size, only other zombie in range will help chase
	//									// down the player
	//									// Use this for initialization
	//	public float monsterTooFarFromSpawn = 7f; 	// about a quarter of the map, allows zombie to "control a corner" of the map if the player
	//												// is out of range.
	//
	//	public int enemyGathering = 2;
	//
	//
	//
	//	delegate void MyDelegate();
	//	MyDelegate enemyAction;
	//
	//	float distanceOfPlayer;
	//	float distanceOfSpawn;
	//
	//	bool targetPlayer = false;
	//	bool runFromPlayer = false;
	//	GameObject player;
	//	UnityEngine.Color originalColor;
	//
	//	float velocity;
	//	Vector3 spawn;
	//	private GameController gameController;
	//
	//	private int scoreValue = 1;

	//	void Start () {
	//
	//		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
	//		if (gameControllerObject != null)
	//		{
	//			gameController = gameControllerObject.GetComponent <GameController>();
	//		}
	//		if (gameController == null)
	//		{
	//			Debug.Log ("Cannot find 'GameController' script");
	//		}
	//
	//		player = GameObject.FindGameObjectWithTag ("player");
	//		originalColor = GetComponent<SpriteRenderer>().color;
	//
	//		if (player != null) {
	//			//Debug.Log ("Player Found");
	//			enemyAction = TargetInZone;
	//		} else {
	//			Debug.Log ("Player Referance Missing");
	//		}
	//
	//		spawn = this.transform.position;
	//
	//		//InvokeRepeating ("Wait_decision", 1, 1);
	//
	//	}

	// Update is called once per frame
	//	void Update () {
	//
	//		enemyAction();
	//
	//		distanceOfPlayer = Vector3.Distance (this.gameObject.transform.position, player.transform.position);
	//
	//		distanceOfSpawn = Vector3.Distance (spawn, this.gameObject.transform.position);
	//
	//
	//		if (targetPlayer) {
	//
	//			Vector2 v = new Vector2 ();
	//			v.x = player.transform.position.x - transform.position.x;
	//			v.y = player.transform.position.y - transform.position.y;
	//			if (v.magnitude != 0) {
	//				if (runFromPlayer) {
	//					Vector2 v2 = new Vector2 ();
	//					v2.x = -v.x * speed*2 / v.magnitude * Time.deltaTime;
	//					v2.y = -v.y * speed*2 / v.magnitude * Time.deltaTime;
	//
	//					GetComponent<Rigidbody2D> ().velocity = v2;
	//				} else {
	//					Vector2 v2 = new Vector2 ();
	//					v2.x = v.x * speed / v.magnitude * Time.deltaTime;
	//					v2.y = v.y * speed / v.magnitude * Time.deltaTime;
	//
	//					GetComponent<Rigidbody2D> ().velocity = v2;
	//				}
	//
	//			}
	//
	//			Vector2 dir = player.transform.position - transform.position;
	//			if (dir != Vector2.zero) {
	//				float angle;
	//				if (runFromPlayer) {
	//					angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 180;
	//				} else {
	//					angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
	//				}
	//
	//				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);	
	//
	//			}
	//				
	//		}
	//	}




	//public AudioClip deadZombieSound;
	public float speed = 5f; // the original speed of the zombie
	public float enemyLineOfSight = 16f; 	// the range the zombie are away of the zombie. Chosen because it's ~ half the camera distance
	public float dangerZone = 10f;    // The Danger zone of enemy, enemy must attack with out wait for friend.
	public float inRange = 14f; 	// the range the player is to attack it, should be less than the threat zone but not miniscule

	public float friendRange = 8f; 	// the range when friendlies are close enough to help attack. 
	//Chosen based on zombie size, only other zombie in range will help chase
	// down the player
	// Use this for initialization
	public float monsterTooFarFromSpawn = 15f; 	// about a quarter of the map, allows zombie to "control a corner" of the map if the player
	// is out of range.
	public int enemyGathering = 2;				// Number of enemy friend need before go for attack the player 

	private bool dead;
	delegate void MyDelegate();
	MyDelegate enemyAction;

	float distanceOfPlayer;
	float distanceOfSpawn;
	bool targetPlayer = false;
	bool runFromPlayer = false;
	GameObject player;
	UnityEngine.Color originalColor;

	float velocity;
	Vector3 spawn;
	private GameController gameController;

	private int scoreValue = 1;

	Animator anim;
	AudioSource audioSource;


	void Awake(){
		audioSource = gameObject.GetComponent<AudioSource>();
		anim = GetComponent<Animator> ();

	}

	void Start () {

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

		player = GameObject.FindGameObjectWithTag ("player");
		originalColor = GetComponent<SpriteRenderer>().color;

		if (player != null) {
			//Debug.Log ("Player Found");
			enemyAction = TargetInZone;
		} else {
			Debug.Log ("Player Referance Missing");
		}

		velocity = speed;

		spawn = this.transform.position;

		//enemyAction();
		InvokeRepeating ("Wait_decision", 1, 1);

	}




	void Update () {

		if (velocity == 0) {
			anim.Play ("zombie_Idle");
		}

		anim.SetFloat("Speed",velocity); // change idle/movement animation


		distanceOfPlayer = Vector3.Distance (this.gameObject.transform.position, player.transform.position);
		//Debug.Log ("distance Of Player: " + distanceOfPlayer);
		distanceOfSpawn = Vector3.Distance (spawn, this.gameObject.transform.position);
		//Debug.Log ("distance Of Spawn: " + distanceOfSpawn);

		if (distanceOfPlayer < 1.5f && targetPlayer) {
			anim.SetBool ("attack",true);
		} else {
			anim.SetBool ("attack",false);
		}

		GetComponent<Rigidbody2D>().AddForce(gameObject.transform.right * velocity);


		if (targetPlayer) {
			float z = Mathf.Atan2((player.transform.position.y - transform.position.y), 
				(player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg;

			if (runFromPlayer) {
				z = -z;
			}
			transform.eulerAngles = new Vector3(0, 0, z);



		}
	}


	void FixedUpdate () {

	}


	//	public void zombieHit(){
	//		hitpoints--;
	//	}
	//
	//	public int getHP(){
	//		return hitpoints;
	//	}

	public void lowHp() {
		// game object behavior when low hp, will called when enter lowHp state
	}

	public void die() {
		if (dead) {
			return;
		}
		dead = true;
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "deadZombie";
		gameController.AddScore (scoreValue);
		//audioSource.PlayOneShot(audioSource.clip);
		anim.SetTrigger("dead");
		Destroy(gameObject,1.2f);
	}

	public void attackPlayer(){
		if (distanceOfPlayer < enemyLineOfSight && !runFromPlayer) {
			enemyAction = attack;
		} else {
			enemyAction = TargetInZone;
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

		else if (distanceOfPlayer > dangerZone && distanceOfPlayer < enemyLineOfSight ) {
			//Debug.Log ("Target: Player in Range: True");
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
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

		if (distanceOfSpawn > monsterTooFarFromSpawn) {

			Debug.Log("Too Far From Spawn");
			targetPlayer = false;

			Vector2 dir = spawn - transform.position;
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			velocity = speed;
			enemyAction = TargetInZone;

		} else {
			Debug.Log("Not Too Far From Spawn");
			//enemyAction = TargetInZone;
			enemyAction = random;

		}

	}


	private void random(){

		enemyAction = randomWalk;
	}





	private void randomWalk(){
		//Debug.Log("Random Walk");
		this.gameObject.GetComponent<SpriteRenderer>().color = originalColor;
		targetPlayer = false;
		velocity = speed;
		transform.eulerAngles = new Vector3 (0, 0, Random.Range (0, 360));
		enemyAction = TargetInZone;
	}


	private void InRange(){
		if (distanceOfPlayer < inRange) {
			//Debug.Log("inRange True");
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
			enemyAction = friendsNear;
		} else{
			if (isHpLow ()) {
				enemyAction = hpLow;
			}
		}
	}


	private void friendsNear(){
		if (isFriendClose()) {
			//Debug.Log("Friends Near True");
			Debug.Log("Lets Attack the Player............");
			enemyAction = attack;
		} else {
			//Debug.Log("Friends Near False");

			if (!isHpLow ()) {
				enemyAction = TargetInZone;

			} else {

				Debug.Log("Can not attack now");
				enemyAction = hpLow;

			}
		}
	}

	private bool isFriendClose(){
		GameObject[] enemy = GameObject.FindGameObjectsWithTag ("enemy");
		int numEnemyFriendNearby = 0;
		foreach (GameObject e in enemy){
			if (e != this.gameObject){
				if (Vector3.Distance (e.gameObject.transform.position, this.transform.position) < friendRange) {
					//Debug.Log("Found Enemy Friend");
					numEnemyFriendNearby = numEnemyFriendNearby+1;
					Debug.Log("No of Enemy Friends found: "+ numEnemyFriendNearby);
					//return true;
				}
			}
		}
		if (numEnemyFriendNearby > enemyGathering) {

			return true;
		}

		return false;
	}


	private void hpLow(){
		if (!isHpLow()) {
			//Debug.Log("HP Low False");
			enemyAction = attack; // again because attack/advance are samesies.
		} 
		else 
		{
			Debug.Log("HP Low True");
			enemyAction = retreat;
		}
	}

	private bool isHpLow(){
		return GetComponent<MortalObject> ().isLowHp ();	
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
