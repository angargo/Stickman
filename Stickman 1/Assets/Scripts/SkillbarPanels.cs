using UnityEngine;
using System.Collections;

public class SkillbarPanels : MonoBehaviour {

	RectTransform rectTransform;

	// Use this for initialization
	void Start () {

		rectTransform = this.GetComponent<RectTransform>();
	
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.SetAsLastSibling();
	}
}
