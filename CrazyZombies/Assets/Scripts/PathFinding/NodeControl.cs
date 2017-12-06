using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeControl : MonoBehaviour {
	public float maxDistance;

	public LayerMask layerMask;

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
	}

	public List<Vector2> Path(GameObject finder, string layer) {
		Debug.Log ("Path Finding Started.......");

		List<Vector2> path = new List<Vector2>();

		Vector2 startPos = finder.transform.position;
		Vector2 targetPos = gameObject.transform.position;

		// when nothing blocking
		layerMask = 1 << LayerMask.NameToLayer (layer);
		bool hit = Physics2D.Linecast (startPos, targetPos,layerMask);
		if (!hit) {
			Debug.Log ("Adding Path..........Collide with: ");
			path.Add (startPos);
			path.Add (targetPos);
			return path;
		}

		GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag ("Node");
		List<Node> nodes = new List<Node> ();

		foreach (GameObject node in nodeObjects) {
			Node currNode = new Node (node.transform.position);
			if (Vector2.Distance (currNode.pos, startPos) < maxDistance * 2 && Vector2.Distance (currNode.pos, targetPos) < maxDistance * 2) {
				nodes.Add (currNode);
			}
		}
		Node startNode = new Node (startPos);
		nodes.Add (startNode);
		Node targetNode = new Node (targetPos);
		nodes.Add (targetNode);

		List<Node> openList = new List<Node> ();
		List<Node> closeList = new List<Node> ();
		openList.Add (startNode);
		startNode.score = 0;

		while (openList.Count > 0) {
			Node curNode = findClosestNode (openList);
			if (curNode.Equals(targetNode)) {
				while (curNode != startNode) {
					path.Add(curNode.pos);
					curNode = curNode.prevNode;
				}
				path.Add (startPos);
				return path;
			}

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
				Debug.DrawLine (n.pos, curNode.pos, Color.white); 
			}
		}

		return null;
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
