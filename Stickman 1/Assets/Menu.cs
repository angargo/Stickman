using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	bool inside = false;
	Vector3 offset = Vector3.zero;

	int myState;
	const int fix = 0;
	const int movable = 1;

	public void OnPointerEnter (PointerEventData eventData){
		inside = true;
	}

	public void OnPointerExit (PointerEventData eventData){
		inside = false;
	}

	public void OnPointerDown (PointerEventData eventData) { //When I press the skill [with any button]
		if (!Input.GetMouseButtonDown(0)) return;
		setState(movable);
		offset = transform.position - Input.mousePosition;
	}

	public void setState(int a){
		myState = a;
	}

	public void close(){
		setState(fix);
		this.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		myState = fix;
	}
	
	// Update is called once per frame
	void Update () {
		if (myState == movable){
			if (Input.GetMouseButtonUp(0)) setState(fix);
			else transform.position = Input.mousePosition + offset;
		}
	}
}
