using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Script : MonoBehaviour {

	public AudioSource audio;
	void Start () {
		audio = GetComponent<AudioSource>();
		audio.Play ();

	}
	// Use this for initialization
	public void PlayGame () {
		SceneManager.LoadScene(1);

	}

	// Update is called once per frame
	public void Quit () {
		Application.Quit();
	}
}
