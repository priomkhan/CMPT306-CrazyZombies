using UnityEngine;
using System.Collections;

public class BarbarianAI : MonoBehaviour {

	public Transform player;

	public float attackThreshold = 2.5f;

	public float attackSpeed;
	public float attackSpeedVariance;

	private bool can_attack = true;

	public float rotateSpeed = 8.0f;

	private Animator anim;

	bool isActive = true;

	DecisionTree root;
    
	void Awake()
	{
		anim = GetComponent<Animator> ();
	}

	void Start()
	{
		BuildDecisionTree();
		player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
	}

	void Update()
	{
		if(isActive)
			root.Search();
	}

	public void SetAnimator(Animator an)
	{
		anim = an;
		anim.applyRootMotion = false;
	}

	void EnableAttack()
	{
		can_attack = true;
	}

	void SetActive()
	{
		isActive = true;
	}

	void SetInactive()
	{
		isActive = false;
	}

	/*****  Decisions  ******/

	bool CheckPlayerDistance()
	{
		float playerDist = Vector3.Distance(gameObject.transform.position, player.position);

		if(playerDist < attackThreshold)
			return true;
		else
			return false;
	}

//	bool CheckHostile()
//	{
//		return GetComponent<Player_weapon_select>().IsHostile();
//	}
//
	/******  Actions  ******/
	void Idle()
	{
		gameObject.SendMessage("SetTarget", gameObject.transform, SendMessageOptions.DontRequireReceiver);
	}

	void ApproachPlayer()
	{
		FacePlayer ();
		gameObject.SendMessage("SetTarget", player.transform, SendMessageOptions.DontRequireReceiver);
	}

	void Attack()
	{
		FacePlayer ();

		gameObject.SendMessage("SetTarget", gameObject.transform, SendMessageOptions.DontRequireReceiver);

		if(can_attack)
		{
			can_attack = false;
			anim.SetTrigger ("Attack");

			float attackInterval = Random.Range (attackSpeed - attackSpeedVariance, attackSpeed + attackSpeedVariance);
			Invoke ("EnableAttack", attackInterval);
		}
	}

	void FacePlayer()
	{
		Vector3 relativePos = player.transform.position - transform.position;
		Quaternion lookrotation = Quaternion.LookRotation(relativePos, Vector3.up);
		lookrotation.x = 0;
		lookrotation.z = 0;
		transform.rotation = Quaternion.Lerp(transform.rotation, lookrotation, Time.deltaTime * rotateSpeed);
	}

	/******  Build Decision Tree  ******/

	void BuildDecisionTree()
	{
		/******  Decision Nodes  ******/
		//DecisionTree isHostileNode = new DecisionTree();
		//isHostileNode.SetDecision(CheckHostile);

		DecisionTree isInRangeNode = new DecisionTree();
		isInRangeNode.SetDecision(CheckPlayerDistance);

		DecisionTree actIdleNode = new DecisionTree();
		actIdleNode.SetAction(Idle);

		DecisionTree actApproachNode = new DecisionTree();
		actApproachNode.SetAction(ApproachPlayer);

		DecisionTree actAttackNode = new DecisionTree();
		actAttackNode.SetAction(Attack);

		/******  Assemble Tree  ******/
		//isHostileNode.SetLeft(actIdleNode);
		//isHostileNode.SetRight(isInRangeNode);

		isInRangeNode.SetLeft(actApproachNode);
		isInRangeNode.SetRight(actAttackNode);

		//root = isHostileNode;
	}
}
