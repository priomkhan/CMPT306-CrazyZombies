using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeControl : MonoBehaviour {
	public float maxDistance;
	public string layer;
	private LayerMask layerMask;
	private List<Node> path;

	class Node {
		public Vector2 pos;
		public float score;
		public Node prevNode;


		public Node(Vector2 p) {
			pos = p;
			score = float.PositiveInfinity;
			prevNode = this;
		}

		public override bool Equals (object obj)
		{
			return this.pos.Equals (((Node)obj).pos);;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}

	void Start() {
		path = new List<Node> ();
		InvokeRepeating ("findPath", 0f, 0.5f);
	}

	void Update() {
	}


	public Vector2 generatePath(GameObject finder) {
		Vector2 startPos = finder.transform.position;
		Vector2 targetPos = gameObject.transform.position;
		bool hit = Physics2D.Linecast (startPos, targetPos,layerMask);

		if (!hit) {
			//Debug.Log ("Adding Path..........Collide with: ");
			return targetPos;
		}

		Node curNode = new Node (Vector2.positiveInfinity);
		float minDistance = float.MaxValue;

		foreach (Node n in this.path) {
			if (!Physics2D.Linecast (startPos, n.pos,layerMask) && 
				n.score + Vector2.Distance (startPos, n.pos) < minDistance && 
				n.score + Vector2.Distance (startPos, n.pos) < maxDistance) {
				curNode = n;
				minDistance = n.score + Vector2.Distance (startPos, n.pos);
			}
		}
		return curNode.pos;
	}

	private void findPath() {
		//Debug.Log ("Path Finding Started.......");

		// when nothing blocking
		layerMask = 1 << LayerMask.NameToLayer (layer);

		Vector2 targetPos = gameObject.transform.position;

		GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag ("Node");
		List<Node> nodes = new List<Node> ();

		foreach (GameObject node in nodeObjects) {
			Node currNode = new Node (node.transform.position);
			if (Vector2.Distance (currNode.pos, targetPos) < maxDistance * 2) {
				nodes.Add (currNode);
			}
		}
		Node targetNode = new Node (targetPos);
		nodes.Add (targetNode);

		List<Node> openList = new List<Node> ();
		List<Node> closeList = new List<Node> ();
		openList.Add (targetNode);
		targetNode.score = 0;

		while (openList.Count > 0) {
			Node curNode = findClosestNode (openList);


			openList.Remove (curNode);
			closeList.Add (curNode);

			List<Node> connectedNodes = findConnectedNodes (curNode, nodes);
			foreach (Node n in connectedNodes) {
				float distance = curNode.score + Vector2.Distance (curNode.pos, n.pos);
				if (distance > maxDistance || closeList.Contains(n)) {
					continue;
				}
				if (openList.Contains (n)) {
					if (distance < n.score) {
						n.prevNode = curNode;
						n.score = distance;
					}
				} else {
					n.prevNode = curNode;
					n.score = distance;
					openList.Add (n);
				}
				Debug.DrawLine (n.pos, curNode.pos, Color.white, 1f); 
			}
		}
		path = closeList;
	}

	// find the node have min score in the list
	private Node findClosestNode(List<Node> nodeList) {
		float minDistance = float.MaxValue;
		Node closetNode = null;
		foreach (Node n in nodeList) {
			if (n.score < minDistance) {
				minDistance = n.score;
				closetNode = n;
			}
		}
		return closetNode;
	}

	// find nodes connected to current node
	private List<Node> findConnectedNodes(Node curNode, List<Node> nodeList) {
		List<Node> connectedNodes = new List<Node> ();
		foreach (Node n in nodeList) {
			if (n.Equals (curNode)) {
				continue;
			}
			bool hitNode = Physics2D.Linecast (n.pos, curNode.pos, layerMask);
			if (!hitNode) {
				connectedNodes.Add (n);
			}
		}
		return connectedNodes;
	}

}
