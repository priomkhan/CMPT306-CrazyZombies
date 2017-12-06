﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class Tutorial : MonoBehaviour {

	public MovieTexture movie;
    private AudioSource audio;
	
	// Use this for initialization 
	void Start () {
		GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
		audio.clip = movie.audioClip;

		movie.Play();
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {

		//if spacebar hit then destory movie and go to menu
		if(Input.GetButtonDown("Jump") && movie.isPlaying){
			Destroy(this.gameObject);
        }
		//if video over destory image and go to menu
		if (!movie.isPlaying) {
			Destroy (this.gameObject);
		}


	}
}