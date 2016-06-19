using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	Player player;
	public int skillNumber;
	Image image;
	Color initialColor;

	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<Player>();
		image = this.GetComponent<Image>();
		initialColor = image.color;
	}
	
	// Update is called once per frame
	void Update () {
		Skill[] skills = player.GetComponentsInChildren<Skill>();
		bool usingSkill = false;
		foreach (Skill skill in skills){
			if (skill.getSkillNumber() == skillNumber) usingSkill = true;
		}
		if (usingSkill) image.color = Color.black;
		else image.color = initialColor;
	}
}
