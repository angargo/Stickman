using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private bool arrived = false;
	private Vector3 targetPosition;
	private float speed;
	private Vector3 direction;
	private Character myCharacter;
	private Vector3 originalPosition;

	public void SetParameters(Vector3 v, float s, Character character){
		targetPosition = v;
		speed = s;
		direction = targetPosition - this.transform.position;
		if (direction.magnitude > Mathf.Epsilon) direction.Normalize();
		myCharacter = character;
	}

	public bool hasArrived(){
		return arrived;
	}

	public void reflect(Character character){
		Vector3 aux = targetPosition;
		SetParameters(originalPosition, speed, character);
		originalPosition = aux;
	}

	void Awake(){
		originalPosition = transform.position;
	}

	// Use this for initialization
	void Start () {
		arrived = false;
	}

	void moveMyself(){
		if (Vector3.Dot(targetPosition - this.transform.position, direction) < Mathf.Epsilon){
			arrived = true;
		}
		Vector3 newPosition;
	    //if we are close enough to our target or not
	    if ((this.transform.position - targetPosition).magnitude <= speed * Time.deltaTime) newPosition = targetPosition;
        else  newPosition = this.transform.position + direction * speed * Time.deltaTime;

        //finding obstacles (collisions).
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(newPosition)));
        bool collision = false;
        foreach (RaycastHit2D hit in hits) {
          if (hit.collider.gameObject.tag == "Obstacle") {
            collision = true;
            break;
          }
        }
        if (!collision) this.transform.position = newPosition; //Everything goes smoothly.
	    else arrived = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!arrived) moveMyself();
	}
}
