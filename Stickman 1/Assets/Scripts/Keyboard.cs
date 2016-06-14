using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour {

	private Player player;
  
	// Use this for initialization
	void Start () {
    	player = GameObject.FindObjectOfType<Player>();
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



	}

	bool isFloor(RaycastHit2D rc){
    return rc.collider.gameObject.tag == "Floor";
  }

}
