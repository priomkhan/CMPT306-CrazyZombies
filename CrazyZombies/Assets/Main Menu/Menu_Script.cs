using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Script : MonoBehaviour {

	public AudioSource a;
	void Start () {
		//backgroundmusic 
		a = GetComponent<AudioSource>();
		a.Play ();
	}
	// Start game from level 1
	public void PlayGame () {
		SceneManager.LoadScene(1);

	}
	//exit the game
	public void Quit () {
		Application.Quit();	
	}
}
