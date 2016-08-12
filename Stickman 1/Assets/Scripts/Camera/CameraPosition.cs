using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {
  
  private Player player;
  private Vector3 drag;
  private Vector3 offset;
  public  float minAngle = 5, maxAngle = 70;
  private float midAngle, maxDiff;
  bool quake;
  public const float quakeMax = 0.2f;

  private Vector3 quakeOffset = Vector3.zero;

    // Use this for initialization
    void Start () {
      player = GameObject.FindObjectOfType<Player>();
      midAngle = (minAngle + maxAngle)/2;
      maxDiff = 179 - (maxAngle - minAngle)/2;
      initializeOffset();
      offset = transform.position - player.transform.position;
      quake = false;
    }

    // Update is called once per frame
    void Update () {
      CheckCustomEvents();
      quakeEffect();
	}

  void initializeOffset(){
  	offset = new Vector3(0, 7.5f, 0);
  	this.transform.position = player.transform.position + offset;
  	correctPosition();
  }

  public void setQuake(bool b){
  	quake = b;
  }

  bool isQuake(){
  	Status[] statusArray = player.GetComponentsInChildren<Status>();
  	foreach (Status status in statusArray){
  	  if(status.getStatus() == Constants.quake) return true;
  	}
  	return false;
  }

  void quakeEffect(){
  		this.transform.localPosition -= quakeOffset;
  		quakeOffset = Vector3.zero;
  		if (isQuake()){
			quakeOffset.x = Random.value*quakeMax;
			quakeOffset.y = 0;
			quakeOffset.z = Random.value*quakeMax;
			this.transform.localPosition += quakeOffset;
		}
  }

  void correctPosition(){ //Super cutre, ya lo cambiare. Mantiene la cam entre minAngle y maxAngle respecto a la vertical.
		Vector3 normalVec = transform.TransformDirection(Vector3.forward);
		float angle = Vector3.Angle(normalVec, Vector3.down);
		float sign = -Vector3.Dot(transform.TransformDirection(Vector3.right), Vector3.Cross(Vector3.down, normalVec));
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

  bool noMenus(){
  	Menu[] menus = GameObject.FindObjectsOfType<Menu>();
  	foreach (Menu menu in menus) if (menu.isActiveAndEnabled) return false;
  	return true;
  }
  
  bool noDraggedStuff()
    {
        GameObject DraggedSkills = GameObject.Find("Moved Skills");
        return (DraggedSkills.transform.childCount <= 0);
    }
  
  void OnMouseLeftDrag() {
        if (noMenus() && noDraggedStuff()) {
            Vector3 diff = Input.mousePosition - drag;
            if (Input.GetKey(KeyCode.LeftShift)) {
                //Debug.Log("Vertical drag");
                if (diff.y > maxDiff) diff.y = maxDiff;
                if (diff.y < -maxDiff) diff.y = -maxDiff;
                transform.RotateAround(player.transform.position, transform.TransformDirection(Vector3.right), -diff.y);
            } else {
                transform.RotateAround(player.transform.position, Vector3.up, diff.x);
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
