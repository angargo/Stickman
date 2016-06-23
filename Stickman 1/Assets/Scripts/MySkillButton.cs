using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MySkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	static public GameObject draggedItem;

	public Color defaultColor = Color.white, highlightedColor, pressedColor;
	private Image myImage;

	private GameObject movedSkills; //We store the dragged objects into the 'Moved Skills' object. [maybe better 'Dragged Skills'?]

	private bool isMouseOverMe; //If cursor is over the button or not


	//Possible states of the button:
	private int myState;
	const int inSkillMenu = 0;
	const int dragged = 1;
	const int inSkillBar = 2;

	//Button stuff!

	public void copyMyself(){ //We instantiate this object, set it to 'dragged', and attach it to movedSkills
		draggedItem = Instantiate(this.gameObject , this.transform.position, Quaternion.identity) as GameObject;
		draggedItem.transform.SetParent(movedSkills.transform);
		draggedItem.GetComponent<MySkillButton>().setParameters(dragged);
		draggedItem.name = this.gameObject.name; //To avoid the extra '(copy)' in its name.
	}

	public void OnPointerDown (PointerEventData eventData) { //When I press the skill [with any button]

		if (!Input.GetMouseButtonDown(0)) return; //We only want the left button
		
		//Button aesthetics
		if (myState != dragged) myImage.color = pressedColor;
		else myImage.color = defaultColor;

		//Creating the 'dragged version' of the object
		copyMyself();
		if (myState == inSkillBar) Destroy(this.gameObject); //If it is in skillbar we get the dragged one and destroy this one
	}

	public void OnPointerUp (PointerEventData eventData) { //When we stop pressing any button
		if (isMouseOverMe) myImage.color = highlightedColor; //Button aesthetics
	}

	public void OnPointerEnter (PointerEventData eventData) //Cursor gets in
	{	
		//Button aesthetics
		if (myState != dragged) myImage.color = highlightedColor;
		else myImage.color = defaultColor;

		isMouseOverMe = true; //Of course...
	}

	public void OnPointerExit (PointerEventData eventData)  //Cursor gets out
	{
		myImage.color = defaultColor; //Button aesthetics

		isMouseOverMe = false;
	}

	public void setParameters(int state){ //set myState to state (+ consequences)
		myState = state;
		if (myState == dragged){
			//aesthetics
			myImage = this.GetComponent<Image>();
			myImage.color = defaultColor;

			//We don't want this object to bother our raycasts if we are dragging it
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		else {
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	public void putInBar (SkillbarPanel panel){ //We instantiate the object and put it as a child of the panel
		GameObject newChildren = Instantiate(this.gameObject, panel.transform.position, Quaternion.identity) as GameObject;
		newChildren.transform.SetParent(panel.transform);
		newChildren.GetComponent<MySkillButton>().setParameters(inSkillBar);
		newChildren.name = this.gameObject.name;
	}


	void Awake(){
		myState = inSkillMenu; //just in case
	}

	void Start () {
		//Initializing stuff
		if (myImage == null) myImage = this.GetComponent<Image>();
		myImage.color = defaultColor;
		movedSkills = GameObject.Find("Moved Skills");
		isMouseOverMe = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (myState == dragged) this.transform.position = Input.mousePosition; //obv
		if (Input.GetMouseButtonUp(0)){
			//The SkillbarPanel script is executed first, and thus it will be cloned before destroying itself whenever we drag it to a panel
			if (myState == dragged) Destroy(this.gameObject);
		}
	}
}
