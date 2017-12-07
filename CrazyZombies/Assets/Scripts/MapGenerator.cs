using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MapGenerator {

	Vector2 getPlayerRespawn ();
	bool isLengthSet ();
	GameObject[,] detailedMap ();
}
