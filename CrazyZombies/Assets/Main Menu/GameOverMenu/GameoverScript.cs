using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameoverScript : MonoBehaviour {

	// go back to menu script 
	public void menu(){
		SceneManager.LoadScene(0);
	}
	//exit the game
	public void Quit () {
		Application.Quit();	
	}

}
