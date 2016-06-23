﻿using UnityEngine;
using System.Collections;

public class BodyRenderer : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	public void setInvisible(bool b){
		if (b) spriteRenderer.enabled = false;
		else spriteRenderer.enabled = true;
	}

	public void setSprite(Sprite s){
		spriteRenderer.sprite = s;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}