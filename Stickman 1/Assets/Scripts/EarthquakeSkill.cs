using UnityEngine;
using System.Collections;

public class EarthquakeSkill : MonoBehaviour {

	private Skill mySkill;
	private Character myCharacter;
	private Vector3 targetPosition;
	public GameObject earthquakePrefab;
	public GameObject earthquakeZonePrefab;

	public void cancelSkill(){
		Destroy (this.gameObject);
	}

	public void startSkill(){
		SkillManager skillManager = GameObject.FindObjectOfType<SkillManager>();
		skillManager.cancelAllOtherSkills(myCharacter, mySkill);
		mySkill.setCancel(false);
		myCharacter.setCasting(1);
		mySkill.SetCasting(true);
		mySkill.setCancel(true);
		GameObject earthquakeZone = Instantiate(earthquakeZonePrefab, targetPosition, Quaternion.identity) as GameObject;
		earthquakeZone.transform.parent = this.transform;
	}

	public void finishSkill(){
		GameObject earthquake = Instantiate(earthquakePrefab, targetPosition, Quaternion.identity) as GameObject;
		Earthquake quake = earthquake.GetComponent<Earthquake>();
		quake.setParameters(myCharacter);
		Destroy(this.gameObject);
	}

	public void setParameters (Character character, Vector3 v){
		myCharacter = character;
		targetPosition = v;
	}

	// Use this for initialization
	void Awake () {
		mySkill = this.GetComponent<Skill>();
		mySkill.setSkillNumber(Constants.earthquake);
	}
	
	// Update is called once per frame
	void Update () {
	}
}

