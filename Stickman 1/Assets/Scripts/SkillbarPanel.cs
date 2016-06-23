using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillbarPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private bool inside;
	public char c;
	private Keyboard keyboard;

	public void OnPointerExit (PointerEventData eventData) {
		inside = false;
	}

	public void OnPointerEnter (PointerEventData eventData) {
		inside = true;
	}

	// Use this for initialization
	void Start () {
		inside = false;
		keyboard = GameObject.FindObjectOfType<Keyboard>();
	}

	void setMySkill(GameObject o){
		
		destroyAllMyChildren();
		MySkillButton skill = o.GetComponent<MySkillButton>();
		if (skill == null) return;
		skill.putInBar(this);
		UISkill uiSkill = this.GetComponentInChildren<UISkill>();
		keyboard.setSkill(c, uiSkill);
	}

	void destroyAllMyChildren(){
		MySkillButton[] skills = this.GetComponentsInChildren<MySkillButton>();
		foreach (MySkillButton skill in skills) Destroy(skill.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (inside){
			if (Input.GetMouseButtonUp(0)){
				if (MySkillButton.draggedItem != null){
					setMySkill(MySkillButton.draggedItem);
				}
			}
		}
	}
}
