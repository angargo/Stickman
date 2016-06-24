﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {

	private Player player;
	private GameObject mainMenu;
	private GameObject optionsMenu;
	private GameObject skillsMenu;
    private SkillbarPanel[] panels;

	void Awake() {
		player = GameObject.FindObjectOfType<Player>();
		mainMenu = GameObject.Find("Main Menu");
		optionsMenu = GameObject.Find("Options Menu");
		skillsMenu = GameObject.Find("Skills Menu");
	}
  
	// Use this for initialization
	void Start () {
    	mainMenu.SetActive(false);
    	optionsMenu.SetActive(false);
    	skillsMenu.SetActive(false);
    	panels = GameObject.FindObjectsOfType<SkillbarPanel>();
	}
	
	// Update is called once per frame
	void Update () {
		string s = Input.inputString;
		foreach (char c in s){
			foreach (SkillbarPanel panel in panels){
				if (panel.c == c){
					UISkill skill = panel.GetComponentInChildren<UISkill>();
					if (skill != null){
						int a = skill.skillNumber;
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    		foreach (RaycastHit2D rc in rcArray){
					    	if (isFloor(rc)){
					    		player.castSkill(a, rc.point);
					    	}
					    }
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape)){
			Menu[] menus = GameObject.FindObjectsOfType<Menu>();
			bool noMenus = true;
			foreach (Menu menu in menus){
				noMenus = false;
				menu.close();
			}
			if (noMenus) openMainMenu();
		}
	}

	public void openMainMenu(){
		mainMenu.SetActive(true);
	}

	public void openOptionsMenu(){
		optionsMenu.SetActive(true);
	}

	public void openSkillsMenu(){
		skillsMenu.SetActive(true);
	}



  	bool isFloor(RaycastHit2D rc){
    	return rc.collider.gameObject.tag == "Floor";
  	}

}
