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

	public GameObject effect;


	public void cancelSkill(){
		Instantiate(smokePrefab, myCharacter.transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}

	public void startSkill(){
		moving = false;

		//Cancel all other skills
		SkillManager skillManager = GameObject.FindObjectOfType<SkillManager>();
		skillManager.cancelAllOtherSkills(myCharacter, mySkill);

		//Invisible status associated to this skill
		GameObject statusObject = Instantiate(effect, myCharacter.transform.position, Quaternion.identity) as GameObject;
		statusObject.transform.parent = myCharacter.transform;
		Status status = statusObject.GetComponent<Status>();
		status.setParameters(mySkill, new float[1], false, Constants.invisible, true);

		//Invulnerable status associated to this skill
		GameObject statusObject2 = Instantiate(effect, myCharacter.transform.position, Quaternion.identity) as GameObject;
		statusObject2.transform.parent = myCharacter.transform;
		Status status2 = statusObject2.GetComponent<Status>();
		status2.setParameters(mySkill, new float[1], false, Constants.invulnerable, true);

		//Smoke prefab
		Instantiate(smokePrefab, myCharacter.transform.position, Quaternion.identity);

		//Can cancel this skill
		mySkill.setCancel(true);
	}

	public void secondCast(){
		moving = true;
		mySkill.setCancel(false);
		//myCharacter.setMove(false);
		GameObject statusObject = Instantiate(effect, myCharacter.transform.position, Quaternion.identity) as GameObject;
		statusObject.transform.parent = myCharacter.transform;
		Status status = statusObject.GetComponent<Status>();
		status.setParameters(mySkill, new float[1], false, Constants.controlled, true);
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
		mySkill.setSkillNumber(Constants.smokeTeleport);
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
