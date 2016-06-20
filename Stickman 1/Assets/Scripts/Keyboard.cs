using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {

	private Player player;
	private GameObject mainMenu;
	private UISkill QSkill = null;
	private UISkill WSkill = null;

	void Awake() {
		player = GameObject.FindObjectOfType<Player>();
		mainMenu = GameObject.Find("Main Menu");
	}
  
	// Use this for initialization
	void Start () {
		mainMenu.GetComponent<MainMenu>().startRun();
    	mainMenu.SetActive(false);
	}

	public void setSkill(char c, UISkill skill){
		if (c == 'Q') QSkill = skill;
		if (c == 'W') WSkill = skill;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)){
			if (QSkill != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(QSkill.skillNumber, rc.point);
			    	}
			    }
			}
		}

		if (Input.GetKeyDown(KeyCode.W)){
			if (WSkill != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(WSkill.skillNumber, rc.point);
			    	}
			    }
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			switchMainMenu();
		}
	}


	public void switchMainMenu(){
		bool b = mainMenu.activeSelf;
		mainMenu.SetActive(!b);
	}



  	bool isFloor(RaycastHit2D rc){
    	return rc.collider.gameObject.tag == "Floor";
  	}

}
