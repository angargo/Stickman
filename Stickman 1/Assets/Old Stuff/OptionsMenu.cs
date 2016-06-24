using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public void close(){
		destroyAllMovableSkills();
		this.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) close();
	}

	void destroyAllMovableSkills(){
		if (MySkillButton.draggedItem != null) Destroy (MySkillButton.draggedItem);
	}
}
