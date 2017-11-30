using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFinder : MonoBehaviour {

	public GameObject finder;
	public GameObject target;
	public string layerName;
	NodeControl control;
	List<Vector2> path;
//	private RaycastHit2D[] hits;
//	private RaycastHit2D hit;
//	private LayerMask layerMask;
//	Vector2 finderBound; 
	// Use this for initialization

	void Awake(){
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		control = (NodeControl)cam.GetComponent(typeof(NodeControl));
	
	}

	void Start () {

//		finder = this.GetComponent<Rigidbody2D>();

//		target = GameObject.FindGameObjectWithTag ("Target").GetComponent<Rigidbody2D> ();

//		finderBound = new Vector2 (finder.GetComponent<BoxCollider2D> ().bounds.extents.x+0.1f, finder.GetComponent<BoxCollider2D> ().bounds.extents.y+0.1f);

//		layerMask = 1 << LayerMask.NameToLayer (layerName);
		//Debug.Log (layerMask.value);



	}



	
	// Update is called once per frame
//	void Update () {
//
//		//finder.velocity = Vector2.right;
//		hits = Physics2D.LinecastAll (finder.position, target.position,layerMask);
//
//		if(hits.Length>0){
//			Debug.Log (hits.Length);
//			for (int i = 0; i < hits.Length; i++) {
//				Debug.Log (hits [i].transform.name);
//				if (hits [i].transform.name != target.name) {
//					
//				}
//			}
//		}
//
//		
//	}



//	void Update () {
//
//		//finder.velocity = Vector2.right;
//		hit = Physics2D.Linecast (finder.position+finderBound, target.position,layerMask);
//
//		if(hit){
//			//Debug.Log (hit.transform.name);
//			if (hit.transform.name == target.name) {
//				Debug.DrawLine (transform.position, hit.transform.position,Color.blue);
//
//			} else {
//				Debug.DrawLine (transform.position, hit.transform.position,Color.red);
//				Transform[] childs = hit.transform.gameObject.GetComponentsInChildren<Transform>();
//				for (int i = 0; i < childs.Length; i++) {
//					Vector2 nodePos= new Vector2(childs[i].position.x,childs[i].position.y);
//					RaycastHit2D cantSee = (Physics2D.Linecast (finder.position+finderBound, childs[i].position));
//					Debug.Log(childs[i].tag);
//					if(childs[i].CompareTag("Node")&& !cantSee){
//						Debug.DrawLine (finder.position, childs[i].transform.position, Color.cyan);
//					}
//				}
//			
//			}
//
//			}
//		}



	void Update(){
		Debug.Log ("Calling Path");
		path = control.Path (finder, target, layerName);
		if (path == null) {
			Debug.Log ("No Path Found");
		} else {
			for (int i = 0; i < path.Count - 1; i++) {
				Debug.Log (path.Count);
				DrawPath (path [i], path [i + 1], Color.blue);
			}
		}
	}


	void DrawPath(Vector2 startPos, Vector2 endPos, Color colorName){
	
		Vector3 start = new Vector3 (startPos.x, startPos.y, 0.0f);
		Vector3 end = new Vector3 (endPos.x, endPos.y, 0.0f);

		Debug.DrawLine (start, end, colorName);


	
	}



}
