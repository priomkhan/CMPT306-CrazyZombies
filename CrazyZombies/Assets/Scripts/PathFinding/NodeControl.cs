using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeControl : MonoBehaviour {
	private Vector2 startPos;
	private Vector2 targetPos;
	private Vector2 finderBound;
	public string layerName;
	public string nodeTag;
	private LayerMask layerMask;
	private RaycastHit2D hit;
	Camera mainCamera;

	class Point{

		Vector2 m_pos;
		char m_state = 'u';
		float m_score = 0;
		Point m_prevPoint;

		List<Point> m_connectedPoints = new List<Point> ();
		List<Point> m_potentialPrevPoints = new List<Point> ();

		public Point(Vector2 pos, char state = 'u'){

			m_pos = pos;
			m_state = state;
		}

		public char GetState()
		{
			return m_state;
		}

		public Vector2 GetPos()
		{
			return m_pos;
		}

		public List<Point> GetConnectedPoints()
		{
			return m_connectedPoints;
		}

		public Point GetPrevPoint()
		{
			return m_prevPoint;
		}

		public float GetScore()
		{
			return m_score;
		}

		public List<Point> GetPotentialPrevPoints()
		{
			return m_potentialPrevPoints;
		}

		public void AddConnectedPoint(Point point)
		{
			m_connectedPoints.Add(point);
		}

		public void AddPotentialPrevPoint(Point point)
		{
			m_potentialPrevPoints.Add(point);
		}

		public void SetPrevPoint(Point point)
		{
			m_prevPoint = point;
		}

		public void SetState(char newState)
		{
			m_state = newState;
		}

		public void SetScore(float newScore)
		{
			m_score = newScore;
		}


	}


	public List<Vector2> Path(GameObject finder, GameObject target, string _layerName){
		//Debug.Log ("Finding Path............");
		this.layerName = _layerName;
		finderBound = new Vector2 (finder.GetComponent<BoxCollider2D> ().bounds.extents.x+0.1f, finder.GetComponent<BoxCollider2D> ().bounds.extents.y+0.1f);
//		startPos = finder.GetComponent<Rigidbody2D>().position;
//		targetPos = target.GetComponent<Rigidbody2D> ().position;

		startPos = new Vector2(finder.transform.position.x, finder.transform.position.y);
		targetPos = new Vector2(target.transform.position.x, target.transform.position.y);



		hit = Physics2D.Linecast (startPos+finderBound, targetPos,layerMask);

		layerMask = 1 << LayerMask.NameToLayer (layerName);

//		if (hit)
//		{
//			if (hit.transform.name == target.name) {
//				Debug.Log ("Hit:" + hit.collider.name);
//				Debug.Log ("Adding Path............");
//				path.Add (startPos);
//				path.Add (targetPos);
//				//Debug.DrawLine (path[0], path[1],Color.blue);
//				return path;
//			} else {
//				Debug.DrawLine (startPos, hit.transform.position, Color.red);
//				Transform[] childs = hit.transform.gameObject.GetComponentsInChildren<Transform> ();
//				for (int i = 0; i < childs.Length; i++) {
//					Vector2 nodePos = new Vector2 (childs [i].position.x, childs [i].position.y);
//					RaycastHit2D cantSee = (Physics2D.Linecast (startPos + finderBound, childs [i].position));
//					Debug.Log (childs [i].tag);
//					if (childs [i].CompareTag ("Node") && !cantSee) {
//						Debug.DrawLine (startPos, childs [i].transform.position, Color.cyan);
//					}
//				}
//			}
//
//		}

		if (hit && hit.transform.name == target.name) {
			List<Vector2> path = new List<Vector2>();
			path.Add (startPos);
			path.Add (targetPos);
			return path;
		
		}

		GameObject[] nodes = GameObject.FindGameObjectsWithTag (nodeTag);
		List<Point> points = new List<Point> ();

		foreach (GameObject node in nodes) {
			if (isInCameraBound (node.transform.position)) {
				Point currNode = new Point (node.transform.position);
				points.Add (currNode);
			}
		
		}

		Point endPoint = new Point (targetPos, 'e');

		//Debug.Log ("Number Of Node:" + points.Capacity);

		/***Connect them together***/
		foreach(Point point in points) //Could be optimized to not go through each connection twice
		{
			foreach (Point point2 in points)
			{
				RaycastHit2D hitNode = Physics2D.Linecast (point.GetPos(), point2.GetPos(),layerMask);
				if (!hitNode)
				{
					
					//Debug.DrawLine(point.GetPos(), point2.GetPos(), Color.white);
					point.AddConnectedPoint(point2);
				}
			}
			RaycastHit2D hitEndNode = Physics2D.Linecast (point.GetPos(),targetPos, layerMask);

			if (hitEndNode.collider.CompareTag(target.gameObject.tag) && hitEndNode)
			{


				//Debug.Log("I Hit With:......."+hitEndNode.collider.name+" My Pos:"+ point.GetPos().x+":"+point.GetPos().y);


					//Debug.DrawLine(targetPos,point.GetPos(), Color.white);
					point.AddConnectedPoint(endPoint);

			}

			RaycastHit2D hitStartNode = Physics2D.Linecast (point.GetPos(),startPos, layerMask);

			if (hitStartNode != null) {

				if (hitStartNode.collider.CompareTag (finder.gameObject.tag) && hitStartNode) {

					float distance = Vector2.Distance (startPos, point.GetPos ());
					//Debug.Log ("I Hit With:......." + hitStartNode.collider.name + " My Pos:" + point.GetPos ().x + ":" + point.GetPos ().y);

					if (hitStartNode.collider.gameObject == finder.gameObject) {
						//Debug.DrawLine (startPos, point.GetPos (), Color.white);

						Point startPoint = new Point (startPos, 's');
						point.SetPrevPoint (startPoint);
						point.SetState ('o');
						point.SetScore (distance + Vector2.Distance (targetPos, point.GetPos ()));
					}
				}
			}
		}


		//Go through until we find the exit or run out of connections
		bool searchedAll = false;
		bool foundEnd = false;

		while(!searchedAll)
		{

			searchedAll = true;
			List<Point> foundConnections = new List<Point>();

			foreach (Point point in points) {
			
				if (point.GetState () == 'o') {
					//Debug.Log ("Found Open at: "+point.GetPos().x+":"+point.GetPos().y);

					searchedAll = false;
					List<Point> potentials = point.GetConnectedPoints();
					//Debug.Log ("Number of ConnectedPoints: " + potentials.Count);
					foreach (Point potentialPoint in potentials) {
						
					
						if (potentialPoint.GetState() == 'u')
						{
							//Debug.Log ("Found the U...........");
							potentialPoint.AddPotentialPrevPoint(point);
							foundConnections.Add(potentialPoint);
							potentialPoint.SetScore(Vector2.Distance(startPos, potentialPoint.GetPos()) + Vector2.Distance(targetPos, potentialPoint.GetPos()));
						} else if (potentialPoint.GetState() == 'e')
						{
							//Debug.Log ("Found the exit..........");
							//Found the exit
							foundEnd = true;
							endPoint.AddConnectedPoint(point);
						}
					
					}
					point.SetState('c');
				
				}
			
			}

			//searchedAll = true;

			foreach (Point connection in foundConnections)
			{
				connection.SetState('o');
				//Find lowest scoring prev point
				float lowestScore = 0;
				Point bestPrevPoint = null;
				bool first = true;
				foreach (Point prevPoints in connection.GetPotentialPrevPoints())
				{
					if (first)
					{
						lowestScore = prevPoints.GetScore();
						bestPrevPoint = prevPoints;
						first = false;
					} else
					{
						if (lowestScore > prevPoints.GetScore())
						{
							lowestScore = prevPoints.GetScore();
							bestPrevPoint = prevPoints;
						}
					}
				}
				connection.SetPrevPoint(bestPrevPoint);
			}



		}
		if (foundEnd)
		{
			//trace back finding shortest route (lowest score)
			List<Point> shortestRoute = null;
			float lowestScore = 0;
			bool firstRoute = true;

			foreach (Point point in endPoint.GetConnectedPoints())
			{
				float score = 0;
				bool tracing = true;
				Point currPoint = point;
				List<Point> route = new List<Point>();
				route.Add(endPoint);
				while(tracing)
				{
					route.Add(currPoint);
					if (currPoint.GetState() == 's')
					{
						if (firstRoute)
						{
							shortestRoute = route;
							lowestScore = score;
							firstRoute = false;
						} else
						{
							if (lowestScore > score)
							{
								shortestRoute = route;
								lowestScore = score;
							}
						}
						tracing = false;
						break;
					}
					score += currPoint.GetScore();
					currPoint = currPoint.GetPrevPoint();
				}
			}

			shortestRoute.Reverse();
			List<Vector2> path = new List<Vector2>();
			foreach (Point point in shortestRoute)
			{
				path.Add(point.GetPos());
			}
			return path;
		} else
		{
			return null;
		}
	}



	private bool isInCameraBound(Vector3 pos){
		mainCamera = Camera.main;
		float gridSizeX = Mathf.RoundToInt (mainCamera.aspect * 2f * mainCamera.orthographicSize);
		float gridSizeY = Mathf.RoundToInt (2f * mainCamera.orthographicSize);



		float topBound = mainCamera.transform.position.y + gridSizeY;
		float bottomBound = mainCamera.transform.position.y - gridSizeY;
		float leftBound = mainCamera.transform.position.x - gridSizeX;
		float rightBound = mainCamera.transform.position.x + gridSizeX;

		//Vector3 distance = mainCamera.transform.position + pos;

		//Debug.Log ("topBound: " + topBound + " bottomBound: " + bottomBound + " leftBound: " + leftBound + " rightBound: " + rightBound);
		//Debug.Log ("Cam:" + mainCamera.transform.position.x + ":" + mainCamera.transform.position.y);

		//Debug.Log ("Pos:" + pos.x + ":" + pos.y);

		if (pos.x > leftBound && pos.x < rightBound && pos.y < topBound && pos.y > bottomBound) {
			//Debug.Log ("InBound" + pos.x + ":" + pos.y);
			return true;
		} else {
			return false;
		}
	}
}
