using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillbarPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private bool inside;
	public char c;
	private Keyboard keyboard;

	//Same stuff as in MySkillButton

	public void OnPointerExit (PointerEventData eventData) {
		inside = false;
	}

	public void OnPointerEnter (PointerEventData eventData) {
		inside = true;
	}

	// Use this for initialization
	void Start () {
		//Initializing stuff
		inside = false;
		keyboard = GameObject.FindObjectOfType<Keyboard>();
	}

	void setMySkill(GameObject o){

		//If the dragged item comes from another position of the bar, swap!
		MySkillButton mySkill = this.GetComponentInChildren<MySkillButton>();
		if (mySkill != null){
			SkillbarPanel panel = o.GetComponent<MySkillButton>().myParent;
			if (panel != null) mySkill.putInBar(panel);
		}

		destroyAllMyChildren(); //Whenever we drag a new skill, we destroy the previous one

		//Just in case
		MySkillButton skill = o.GetComponent<MySkillButton>();
		if (skill == null) return;

		Debug.Log("hola amigo");

		//We make a copy of the skill and attach it here
		skill.putInBar(this);
	}

	void destroyAllMyChildren(){
		MySkillButton[] skills = this.GetComponentsInChildren<MySkillButton>();
		foreach (MySkillButton skill in skills) Destroy(skill.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (inside){
			if (Input.GetMouseButtonUp(0)){
				if (MySkillButton.draggedItem != null){ //Dragging an actual skill
					setMySkill(MySkillButton.draggedItem);
				}
			}
		}
	}
}
