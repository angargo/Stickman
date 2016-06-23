using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MySkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	static public GameObject draggedItem;

	public Color defaultColor = Color.white, highlightedColor, pressedColor;
	private Image myImage;
	private GameObject movedSkills;
	private bool inside;

	private int myState;

	const int inSkillMenu = 0;
	const int dragged = 1;
	const int inSkillBar = 2;

	//Button stuff!

	public void copyMyself(){
		draggedItem = Instantiate(this.gameObject , this.transform.position, Quaternion.identity) as GameObject;
		draggedItem.transform.SetParent(movedSkills.transform);
		draggedItem.GetComponent<MySkillButton>().setParameters(dragged);
		draggedItem.name = this.gameObject.name;
	}

	public void OnPointerDown (PointerEventData eventData) {
		if (myState != dragged) myImage.color = pressedColor;
		else myImage.color = defaultColor;
		copyMyself();
		if (myState == inSkillBar) Destroy(this.gameObject);
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (inside) myImage.color = highlightedColor;
	}

	public void OnPointerEnter (PointerEventData eventData) 
	{
		if (myState != dragged) myImage.color = highlightedColor;
		else myImage.color = defaultColor;
		inside = true;
	}

	public void OnPointerExit (PointerEventData eventData) 
	{
		myImage.color = defaultColor;
		inside = false;
	}

	public void setParameters(int state){
		myState = state;
		if (myState == dragged){
			myImage = this.GetComponent<Image>();
			myImage.color = defaultColor;
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		else {
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	public void putInBar (SkillbarPanel panel){
		GameObject newChildren = Instantiate(this.gameObject, panel.transform.position, Quaternion.identity) as GameObject;
		newChildren.transform.SetParent(panel.transform);
		newChildren.GetComponent<MySkillButton>().setParameters(inSkillBar);
		newChildren.name = this.gameObject.name;
	}


	void Awake(){
		myState = inSkillMenu;
	}

	// Use this for initialization
	void Start () {
		if (myImage == null) myImage = this.GetComponent<Image>();
		myImage.color = defaultColor;
		movedSkills = GameObject.Find("Moved Skills");
		inside = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (myState == dragged) this.transform.position = Input.mousePosition;
		if (Input.GetMouseButtonUp(0)){
			if (myState == dragged) Destroy(this.gameObject);
		}
	}
}
