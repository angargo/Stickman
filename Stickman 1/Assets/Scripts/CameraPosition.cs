using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {
  
  private Player player;
  private Vector3 drag;
  private Vector3 offset;
  public const float minAngle = 25, maxAngle = 50;
  private float midAngle, maxDiff;

    // Use this for initialization
    void Start () {
      player = GameObject.FindObjectOfType<Player>();
      midAngle = (minAngle + maxAngle)/2;
      maxDiff = 179 - (maxAngle - minAngle)/2;
      initializeOffset();
      offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update () {
      CheckCustomEvents();
	}

  void initializeOffset(){
  	offset = new Vector3(0,0, -7.5f);
  	this.transform.position = player.transform.position + offset;
  	correctPosition();
  }

  void correctPosition(){ //Super cutre, ya lo cambiare. Mantiene la cam entre minAngle y maxAngle respecto a la vertical.
		Vector3 normalVec = transform.TransformDirection(Vector3.forward);
		float angle = Vector3.Angle(normalVec, Vector3.forward);
		float sign = -Vector3.Dot(transform.TransformDirection(Vector3.right), Vector3.Cross(Vector3.forward, normalVec));
		if (sign < 0) angle = -angle;
		float dif = 0;
		if (angle < minAngle && angle > midAngle - 180){
			dif = angle - minAngle;
		}
		else if (angle > maxAngle || angle < 0){
			dif = angle - maxAngle;
		}
		if (dif == 0) return;
		transform.RotateAround(player.transform.position, transform.TransformDirection(Vector3.right), dif);
		offset = transform.position - player.transform.position;
  }
  
  void CheckCustomEvents() {
    if (Input.GetMouseButtonDown(0)) OnMouseLeftDown();
    if (Input.GetMouseButton(0)) OnMouseLeftDrag();
    if (Input.GetMouseButtonUp(0)) OnMouseLeftUp();
	//player.changeSprite();
  }
  
  void OnMouseLeftDrag() {
        if (GameObject.FindObjectsOfType<Menu>() == null) {
            Vector3 diff = Input.mousePosition - drag;
            if (Input.GetKey(KeyCode.LeftShift)) {
                //Debug.Log("Vertical drag");
                if (diff.y > maxDiff) diff.y = maxDiff;
                if (diff.y < -maxDiff) diff.y = -maxDiff;
                transform.RotateAround(player.transform.position, transform.TransformDirection(Vector3.right), diff.y);
            } else {
                transform.RotateAround(player.transform.position, Vector3.forward, diff.x);
            }
            correctPosition();
            offset = transform.position - player.transform.position;

        }
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
