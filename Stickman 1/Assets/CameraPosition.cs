using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {
  
  private Player player;
  private Vector3 drag;
  private Vector3 offset;

	// Use this for initialization
	void Start () {
    player = GameObject.FindObjectOfType<Player>();
    offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
    CheckCustomEvents();
	}
  
  void CheckCustomEvents() {
    if (Input.GetMouseButtonDown(0)) OnMouseLeftDown();
    if (Input.GetMouseButton(0)) OnMouseLeftDrag();
    if (Input.GetMouseButtonUp(0)) OnMouseLeftUp();
    
  }
  
  void OnMouseLeftDrag() {
    Vector3 diff = Input.mousePosition - drag;
    if (Input.GetKey(KeyCode.LeftShift)) {
      Debug.Log("Vertical drag");
      transform.RotateAround(player.transform.position, transform.TransformDirection(Vector3.right), diff.y);
    } else {
      transform.RotateAround(player.transform.position, Vector3.forward, diff.x);
    }
    offset = transform.position - player.transform.position;
    
    drag = Input.mousePosition;
  }
  
  void OnMouseLeftDown() {
    drag = Input.mousePosition;
  }
  
  void OnMouseLeftUp() {
  }
  
  public void FollowPlayer(Vector3 position) {
    transform.position = offset + player.transform.position;
  }
}
