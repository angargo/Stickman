using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	private Vector3 targetPosition, direction;
	public GameObject explosion;
	private Character myCharacter;
	public float speed = 8;

	// Use this for initialization
	void Start () {
		//targetPosition = new Vector3(100,100,100); //randomshit
	}

	private void explode(){
		GameObject expl = Instantiate (explosion, this.transform.position, Quaternion.identity) as GameObject;
		expl.GetComponentInChildren<ExplosionCollider>().setOwner(myCharacter); 
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Target position : " + targetPosition);
		//Debug.Log("Dot product : " +  Vector3.Dot(targetPosition - this.transform.position, direction));
		if (Vector3.Dot(targetPosition - this.transform.position, direction) < Mathf.Epsilon){
			explode();
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
	    else explode();
	}

	public void setTarget(Vector3 v, Character character){
		targetPosition = v;
		direction = targetPosition - this.transform.position;
		if (direction.magnitude > Mathf.Epsilon) direction.Normalize();
		myCharacter = character;
		//Debug.Log("Target set to " + v);
	}

}
