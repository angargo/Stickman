﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int MaxHP = 50;
	private int HP = 50;
	private Enemy enemy;
	private Character myCharacter;

	public void decreaseHealth (int a, Character character){
		if (a > 0){
			if (myCharacter.getStatus(0)) return;
			HP -= a;
			myCharacter.isHit ();
			if (enemy != null && HP <= 0){ //now player is immortal!
				myCharacter.die();
				//Destroy(this.gameObject);
			}
			if (enemy != null && character != null && character.GetComponent<Enemy>() == null){
				enemy.beingAttacked(character);
			}
		}
		else HP -= a;
	}

	public int getCurrentHP(){
		return HP;
	}

	public int getMaxHP(){
		return MaxHP;
	}

	// Use this for initialization
	void Start () {
		enemy = this.GetComponent<Enemy>();
		myCharacter = this.GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
