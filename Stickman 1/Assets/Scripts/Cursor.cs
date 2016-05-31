﻿using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
  
  private Player player;
  
	// Use this for initialization
	void Start () {
    player = GameObject.FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(1)) {
	        OnMouseRightDown();
	    }
	}
  
  bool isFloor(RaycastHit2D rc){
    return rc.collider.gameObject.tag == "Floor";
  }
  
  void OnMouseRightDown() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
    Enemy enemy = null;
    bool floorFound = false;
    Vector3 point = Vector3.zero;
    foreach (RaycastHit2D rc in rcArray){
    	if (enemy == null) enemy = rc.collider.gameObject.GetComponentInParent<Enemy>();
    	if (isFloor(rc)){
    		point = rc.point;
    		floorFound = true;
    	}
    }
    if (enemy != null) player.attackEnemy(enemy.GetComponent<Character>());
    else if (floorFound) player.MoveToPosition(point, false);
  }

}