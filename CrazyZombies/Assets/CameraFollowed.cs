using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowed : MonoBehaviour {

	public GameObject PlayerCharacter;
	private Transform PlayerTransform;

	// Use this for initialization
	void Start () {
		PlayerTransform = PlayerCharacter.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (PlayerTransform.position.x, PlayerTransform.position.y, transform.position.z);
	}
}
