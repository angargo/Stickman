using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
  
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
    //Vector3 pixelPosition = Input.mousePosition;
    //Debug.Log("Input.mousePosition = " + pixelPosition);
    //ctor3 worldPosition = Camera.main.ScreenToWorldPoint(pixelPosition);
    //Debug.Log("Input.mousePosition = " + pixelPosition + " -> " + worldPosition);
    
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D rc = Physics2D.GetRayIntersection(ray);
    
    if (isFloor(rc)) player.MoveToPosition(rc.point);
  }
  
  void OnMouseDrag() {
  }
}
