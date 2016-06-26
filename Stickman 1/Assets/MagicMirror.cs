using UnityEngine;
using System.Collections;

public class MagicMirror : MonoBehaviour {

	private Character myCharacter;
	public float t = 1;
	private bool countDown = false;
	//private Vector3 offset = new Vector3 (0,0, 0);
	Skill mySkill;

	void Awake () {
		mySkill = this.GetComponent<Skill>();
		mySkill.setSkillNumber(4);
	}

	public void cancelSkill(){
		Destroy (this.gameObject);
	}

	public void setParameters(Character character){ //Are we ever gonna use this?
		myCharacter = character;
		//transform.position += offset;
	}

	public void startSkill(){
		countDown = true;
		mySkill.setCancel(true);
		mySkill.SetParameters(myCharacter, false);
		SkillManager skillManager = GameObject.FindObjectOfType<SkillManager>();
		skillManager.cancelAllOtherSkills(myCharacter, mySkill);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(countDown) t -= Time.deltaTime;
		if (t <= 0) cancelSkill();
	}
}
