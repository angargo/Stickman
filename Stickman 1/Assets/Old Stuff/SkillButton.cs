using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	const float invisibleAlpha = 0;
	const float visibleAlpha = 100.0f/255.0f;

	Player player;
	public int skillNumber;
	public char c;
	Image image;
	Color initialColor, inviColor;
	private Keyboard keyboard;

	UISkill mySkill;

	GameObject movedSkills;
	UISkill auxSkill;

	// Use this for initialization
	void Start () {
		keyboard = GameObject.FindObjectOfType<Keyboard>();
		player = GameObject.FindObjectOfType<Player>();
		image = this.GetComponent<Image>();
		initialColor = image.color;
		inviColor = image.color;
		inviColor.a = invisibleAlpha;
		mySkill = this.transform.parent.GetComponentInChildren<UISkill>();
		if (mySkill != null) skillNumber = mySkill.skillNumber;

		movedSkills = GameObject.Find("Moved Skills");
		if (movedSkills != null) auxSkill = movedSkills.GetComponentInChildren<UISkill>();
	}

	public void entering(){
		if (movedSkills != null) auxSkill = movedSkills.GetComponentInChildren<UISkill>();
	}

	public void exiting(){
		auxSkill = null;
	}

	void eliminateSkillsInParent(){
		foreach (UISkill skill in this.transform.parent.GetComponentsInChildren<UISkill>()){
			Destroy(skill.gameObject);
		}
	}

	void detectNewSkill(){
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
			if (auxSkill != null){
				//auxSkill.setMovable(false);
				eliminateSkillsInParent();
				auxSkill.gameObject.GetComponent<RectTransform>().SetParent(this.transform.parent);
				auxSkill.transform.position = this.transform.position;
				mySkill = auxSkill;
				skillNumber = auxSkill.skillNumber;
				//keyboard.setSkill(c, this.mySkill);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		detectNewSkill();
		Skill[] skills = player.GetComponentsInChildren<Skill>();
		bool usingSkill = false;
		if (mySkill == null) usingSkill = true;
		foreach (Skill skill in skills){
			if (skill.getSkillNumber() == skillNumber) usingSkill = true;
		}
		if (usingSkill) image.color = initialColor;
		else image.color = inviColor;
	}
}
