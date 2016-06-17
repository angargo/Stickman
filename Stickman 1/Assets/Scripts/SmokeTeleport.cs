using UnityEngine;
using System.Collections;

public class SmokeTeleport : MonoBehaviour {

	private float speed = 40;
	private Character myCharacter;
	private Vector3 targetPosition;
	private float waitingTime = 3;
	public GameObject smokePrefab;
	private bool moving;
	Skill mySkill;
	Vector3 direction;


	public void cancelSkill(){
		SpriteRenderer sr = myCharacter.GetComponentInChildren<SpriteRenderer>();
		//myCharacter.setStatus(0,0);
		//myCharacter.setStatus(1,0);
		sr.enabled = true;
		Instantiate(smokePrefab, myCharacter.transform.position, Quaternion.identity);
		myCharacter.setMove(true);
		Destroy(this.gameObject);
	}

	public void startSkill(){
		moving = false;
		SpriteRenderer sr = myCharacter.GetComponentInChildren<SpriteRenderer>();
		//myCharacter.setStatus(0,3);
		//myCharacter.setStatus(1,3);
		sr.enabled = false;
		Instantiate(smokePrefab, myCharacter.transform.position, Quaternion.identity);
		mySkill.setCancel(true);
	}

	public void secondCast(){
		moving = true;
		mySkill.setCancel(false);
		myCharacter.setMove(false);
		myCharacter.SetTargetPosition(targetPosition);
	}

	private void moveCharacter(){
		if (Vector3.Dot(targetPosition - myCharacter.transform.position, direction) < Mathf.Epsilon){
			cancelSkill();
		}
		Vector3 newPosition;
	    //if we are close enough to our target or not
		if ((myCharacter.transform.position - targetPosition).magnitude <= speed * Time.deltaTime) newPosition = targetPosition;
		else  newPosition = myCharacter.transform.position + direction * speed * Time.deltaTime;

		myCharacter.moveTo(newPosition); //Everything goes smoothly.
	}

	public void setParameters (Character character, Vector3 v){
		if (moving) return;
		myCharacter = character;
		targetPosition = v;
		direction = targetPosition - myCharacter.transform.position;
		if (direction.magnitude > Mathf.Epsilon) direction.Normalize();
	} 

	// Use this for initialization
	void Awake () {
		mySkill = this.GetComponent<Skill>();
		mySkill.setSkillNumber(1);
	}
	
	// Update is called once per frame
	void Update () {
		waitingTime -= Time.deltaTime;
		if (!moving && waitingTime <= 0) cancelSkill();
		if (moving){
			moveCharacter();
		}
	}
}
