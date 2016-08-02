using UnityEngine;
using System.Collections;

public class HealSkill : MonoBehaviour {

	private Character targetCharacter;
	private Character myCharacter;
	public GameObject healPrefab;
	private Skill mySkill;

	public void setParameters (Character character, Character tcharacter){
		myCharacter = character;
		targetCharacter = tcharacter;
	}

	public void cancelSkill(){
		Destroy(this.gameObject);
	}

	void Awake () {
		mySkill = this.GetComponent<Skill>();
		mySkill.setSkillNumber(Constants.heal);
	}

	public void startSkill(){
		SkillManager skillManager = GameObject.FindObjectOfType<SkillManager>();
		skillManager.cancelAllOtherSkills(myCharacter, mySkill);
		mySkill.setCancel(false);
		if (targetCharacter == null){
			cancelSkill();
			return;
		}
		Health health = targetCharacter.gameObject.GetComponent<Health>();
		health.decreaseHealth(-10, myCharacter, Constants.magical, Constants.neutral);
		GameObject heal = Instantiate(healPrefab, targetCharacter.transform.position, Quaternion.identity) as GameObject;
		heal.transform.parent = targetCharacter.transform;
		cancelSkill();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
