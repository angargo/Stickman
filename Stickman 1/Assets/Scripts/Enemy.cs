﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

  public float moveRadius = 5;
  public float expectedMoveTime = 4;

  public float maxPursueRadius = 10;

  private Character myCharacter;
  private Character myTarget;
  private bool isChasing;

	// Use this for initialization
	void Start () {
		myCharacter = this.GetComponent<Character>();
		isChasing = false;
	}

	void setNewTarget(){ //Choose uniformely a poing inside a circle of radius r.
		float r = moveRadius*moveRadius;
		r *= Random.value;
		float moveRad = Mathf.Sqrt(r);
		float angle = Mathf.Sqrt(Random.value)*360.0f;
		Vector3 direction = new Vector3(moveRad*Mathf.Cos(angle), moveRad*Mathf.Sin(angle),0);
		Vector3 targetPosition = this.transform.position + direction;
		myCharacter.SetTargetPosition(targetPosition);
	}

	// Unimportant
	void decideIfMoving(){ //If I'm not doing shit, we may move
		if (myCharacter.IsIdle()){
			if (Random.value < Time.deltaTime/expectedMoveTime){
				setNewTarget();
			}
		}
	}

	void startChasing(){
		if (myTarget == null) return;
		myCharacter.chaseEnemy(myTarget);
		isChasing = true;
	}

	public void beingAttacked(Character character){  //If I'm hit let's teach this guy a lesson! (It takes 0.25s to react)
		if (character.Equals(myTarget)) return;
		myTarget = character;
		Invoke ("startChasing", 0.25f); //Reaction time = 0.25s, not xploitable.
	}

	bool shouldStopPursuing(){
		if (!isChasing) return false;
		if (myTarget == null) return true;
		if (myTarget.getStatus(1)) return true;
		Vector3 diff = myTarget.transform.position - this.transform.position;
		if (diff.sqrMagnitude > maxPursueRadius*maxPursueRadius) return true;
		return false;
	}

	void stopPursuing(){
		myCharacter.stopChasing();
		myTarget = null;
		isChasing = false;
	}

	// Update is called once per frame
	void Update () {
		decideIfMoving();
		if (shouldStopPursuing()) stopPursuing();
	}
}
