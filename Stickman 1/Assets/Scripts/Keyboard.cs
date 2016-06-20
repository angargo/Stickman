using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {

	private Player player;
	private GameObject mainMenu;

	void Awake() {
		player = GameObject.FindObjectOfType<Player>();
		mainMenu = GameObject.Find("Main Menu");
	}
  
	// Use this for initialization
	void Start () {
		mainMenu.GetComponent<MainMenu>().startRun();
    	mainMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
		    foreach (RaycastHit2D rc in rcArray){
		    	if (isFloor(rc)){
		    		player.castSkill(0, rc.point);
		    	}
		    }
		}

		if (Input.GetKeyDown(KeyCode.W)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
		    foreach (RaycastHit2D rc in rcArray){
		    	if (isFloor(rc)){
		    		player.castSkill(1, rc.point);
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
