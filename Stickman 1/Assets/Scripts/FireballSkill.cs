﻿using UnityEngine;
using System.Collections;

public class FireballSkill : MonoBehaviour {

	private Skill mySkill;
	private Character myCharacter;
	private Vector3 targetPosition;
	public GameObject fireballPrefab;

	public void cancelSkill(){
		Destroy (this.gameObject);
	}

	public void startSkill(){
		mySkill.setCancel(false);
		myCharacter.setCasting(1);
		mySkill.SetCasting(true);
		mySkill.setCancel(true);
	}

	public void finishSkill(){
		GameObject fireb = Instantiate(fireballPrefab, myCharacter.transform.position, Quaternion.identity) as GameObject;
		Fireball fire = fireb.GetComponent<Fireball>();
		fire.setTarget(targetPosition, myCharacter);
		Destroy(this.gameObject);
	}

	public void setParameters (Character character, Vector3 v){
		myCharacter = character;
		targetPosition = v;
	}

	// Use this for initialization
	void Awake () {
		mySkill = this.GetComponent<Skill>();
		mySkill.setSkillNumber(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}