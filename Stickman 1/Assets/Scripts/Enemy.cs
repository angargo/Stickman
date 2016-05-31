using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

  public float moveRadius = 5;
  public float expectedMoveTime = 4;

  public float maxPursueRadius = 10;

  private Character myCharacter;
  private Character myTarget;

	// Use this for initialization
	void Start () {
		myCharacter = this.GetComponent<Character>();
	}

	void setNewTarget(){
		float r = moveRadius*moveRadius;
		r *= Random.value;
		float moveRad = Mathf.Sqrt(r);
		float angle = Mathf.Sqrt(Random.value)*360.0f;
		Vector3 direction = new Vector3(moveRad*Mathf.Cos(angle), moveRad*Mathf.Sin(angle),0);
		Vector3 targetPosition = this.transform.position + direction;
		myCharacter.MoveToPosition(targetPosition, false);
	}

	void decideIfMoving(){
		if (myCharacter.isIdle()){
			if (Random.value < Time.deltaTime/expectedMoveTime){
				setNewTarget();
			}
		}
	}

	public void beingAttacked(Character character){
		myCharacter.attackEnemy(character);
		myTarget = character;
	}

	void checkIfStopPursuing(){
		if (myTarget != null){
			Vector3 diff = myTarget.transform.position - this.transform.position;
			if (diff.sqrMagnitude > maxPursueRadius*maxPursueRadius) myCharacter.setIdle();
		}
	}

	// Update is called once per frame
	void Update () {
		decideIfMoving();
		checkIfStopPursuing();
	}
}
