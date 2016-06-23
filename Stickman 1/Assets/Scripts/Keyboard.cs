using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {

	private Player player;
	private GameObject mainMenu;
    private UISkill[] Skill = null;

	void Awake() {
		player = GameObject.FindObjectOfType<Player>();
		mainMenu = GameObject.Find("Main Menu");
        Skill = new UISkill[5];
        //foreach (UISkill a in Skill) a=null;
	}
  
	// Use this for initialization
	void Start () {
		mainMenu.GetComponent<MainMenu>().startRun();
    	mainMenu.SetActive(false);
	}

	public void setSkill(char c, UISkill skill){
		if (c == 'Q') Skill[0] = skill;
		if (c == 'W') Skill[1] = skill;
		if (c == 'E') Skill[2] = skill;
		if (c == 'R') Skill[3] = skill;
		if (c == 'T') Skill[4] = skill;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)){
			if (Skill[0] != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(Skill[0].skillNumber, rc.point);
			    	}
			    }
			}
		}

		if (Input.GetKeyDown(KeyCode.W)){
			if (Skill[1] != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(Skill[1].skillNumber, rc.point);
			    	}
			    }
			}
		}

		if (Input.GetKeyDown(KeyCode.E)){
			if (Skill[2] != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(Skill[2].skillNumber, rc.point);
			    	}
			    }
			}
		}

		if (Input.GetKeyDown(KeyCode.R)){
			if (Skill[3] != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(Skill[3].skillNumber, rc.point);
			    	}
			    }
			}
		}

		if (Input.GetKeyDown(KeyCode.T)){
			if (Skill[4] != null){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
			    foreach (RaycastHit2D rc in rcArray){
			    	if (isFloor(rc)){
			    		player.castSkill(Skill[4].skillNumber, rc.point);
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
