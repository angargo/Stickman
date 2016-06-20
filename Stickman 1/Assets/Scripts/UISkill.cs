using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISkill : MonoBehaviour {

	public int skillNumber;
	private bool movable;
	RectTransform rectTransform;
	GameObject movedSkills;
	Button button;

	public void copyMyself(){
		if(movable) return;
		GameObject newButton = Instantiate(this.gameObject, this.rectTransform.position, Quaternion.identity) as GameObject;
		UISkill skill = newButton.GetComponent<UISkill>();
		skill.setMovable(true);
		skill.GetComponent<RectTransform>().SetParent(movedSkills.GetComponent<RectTransform>());
		//skill.rectTransform.position = rectTransform.position;
	}

	public void setMovable(bool b){
		movable = b;
		if (movable) button.enabled = false;
		else button.enabled = true;
	}

	public bool isMovable (){
		return movable;
	}

	void Awake(){
		movable = false;
		button = this.GetComponent<Button>();
		movedSkills = GameObject.Find("Moved Skills");
	}

	// Use this for initialization
	void Start () {
		rectTransform = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (movable){
			Debug.Log("Hi bruhs!");
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) Destroy (this.gameObject);
			rectTransform.position = Input.mousePosition;
		}
	}
}
