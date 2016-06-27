using UnityEngine;
using System.Collections;

public class MagicMirror : MonoBehaviour {

	private Character myCharacter;
	public float t = 15;
	private bool countDown = false;
	//private Vector3 offset = new Vector3 (0,0, 0);
	Skill mySkill;
	public GameObject effect;

	void Awake () {
		mySkill = this.GetComponent<Skill>();
		mySkill.setSkillNumber(Constants.magicMirror);
	}

	public void cancelSkill(){
		Destroy (this.gameObject);
	}

	public void setParameters(Character character){ //Are we ever gonna use this?
		myCharacter = character;
		//transform.position += offset;
	}

	public void startSkill(){
		t = 15;
		countDown = true;
		mySkill.setCancel(true);
		SkillManager skillManager = GameObject.FindObjectOfType<SkillManager>();
		skillManager.cancelAllOtherSkills(myCharacter, mySkill);

		GameObject statusObject = Instantiate(effect, myCharacter.transform.position, Quaternion.identity) as GameObject;
		statusObject.transform.parent = myCharacter.transform;
		Status status = statusObject.GetComponent<Status>();
		status.setParameters(mySkill, 0, false, Constants.invulnerableMagic, true);
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
