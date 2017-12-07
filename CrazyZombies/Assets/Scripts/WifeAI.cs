using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WifeAI : MonoBehaviour, Die {

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

	int targetIndex;
	NodeControl control;
	Vector2 path;
	public string layerName;
	public string levelName;

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

		control = (NodeControl)player.GetComponent(typeof(NodeControl));

		velocity = speed;

		spawn = this.transform.position;

		//enemyAction();
		InvokeRepeating ("Wait_decision", 1, 1);
		InvokeRepeating("findPath", 1.0f, 1.15f); //Need to comment out for without PathFinding

	}




	void Update () {

		if (velocity == 0) {
			anim.Play ("zombie_Idle");
		}




		distanceOfPlayer = Vector3.Distance (this.gameObject.transform.position, player.transform.position);
		//Debug.Log ("distance Of Player: " + distanceOfPlayer);
		distanceOfSpawn = Vector3.Distance (spawn, this.gameObject.transform.position);
		//Debug.Log ("distance Of Spawn: " + distanceOfSpawn);

		if (distanceOfPlayer < 1.5f && targetPlayer) {
			anim.SetBool ("attack",true);
		} else {
			anim.SetBool ("attack",false);
		}

		GetComponent<Rigidbody2D>().AddForce(gameObject.transform.right *40* velocity*Time.deltaTime);

		//Without Path finding
		//		if (targetPlayer) {
		//			float z = Mathf.Atan2 ((player.transform.position.y - transform.position.y), 
		//				          (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg;
		//
		//			if (runFromPlayer) {
		//				z = -z;
		//			}
		//			transform.eulerAngles = new Vector3 (0, 0, z);
		//
		//		}

	}


	public void findPath() {

		if (targetPlayer) {
			path = control.generatePath (gameObject);
			//transform.eulerAngles = new Vector3(0, 0, z);
			float z = 0;
			if (path.Equals (Vector2.positiveInfinity)) {
				Debug.Log ("No Path Found");
			} else {

				anim.SetFloat ("Speed", velocity); // change idle/movement animation
				z = Mathf.Atan2 ((path.y - transform.position.y), 
					(path.x - transform.position.x)) * Mathf.Rad2Deg;

				if (runFromPlayer) {
					z = -z;
				}

				//Debug.DrawLine (path, GetComponent<Rigidbody2D>().position, Color.blue, 1f);

			}
			transform.eulerAngles = new Vector3 (0, 0, z);

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









	void DrawPath(Vector2 startPos, Vector2 endPos, Color colorName){

		Vector3 start = new Vector3 (startPos.x, startPos.y, 0.0f);
		Vector3 end = new Vector3 (endPos.x, endPos.y, 0.0f);

		Debug.DrawLine (start, end, colorName);



	}

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
		anim.SetFloat ("Speed", 0f);
		anim.SetTrigger("dead");
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "deadZombie";
		gameController.AddScore (scoreValue);
		audioSource.PlayOneShot(audioSource.clip);

		SceneManager.LoadScene (levelName);

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
			enemyAction = TargetInZone;

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
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
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
		//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.gray;
		Debug.Log("Retreating");
		targetPlayer = true;
		runFromPlayer = true;
		velocity = speed;	
		enemyAction = TargetInZone;
	}


	private void attack(){
		//this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		// code to target player
		Debug.Log("Attacking");
		runFromPlayer = false;
		targetPlayer = true;
		velocity = 3 * speed;
		enemyAction = TargetInZone;
	}

	// takes longer to make decisions for demonstration purposes
	private void Wait_decision()
	{
		enemyAction();
	}
}
