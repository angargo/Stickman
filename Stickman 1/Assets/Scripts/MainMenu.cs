using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	GameObject optionsMenu;
	GameObject skillsMenu;

	public void startRun(){
		optionsMenu = GameObject.Find("Options Menu");
		skillsMenu = GameObject.Find("Skills Menu");
		optionsMenu.SetActive(false);
		skillsMenu.SetActive(false);

	}

	public void openOptionsMenu(){
		optionsMenu.SetActive(true);
	}

	public void openSkillsMenu(){
		skillsMenu.SetActive(true);
	}

}
