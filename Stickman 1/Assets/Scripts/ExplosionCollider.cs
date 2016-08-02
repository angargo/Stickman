using UnityEngine;
using System.Collections;

public class ExplosionCollider : MonoBehaviour {

	private float timeWhenCreated;
	private Character myCharacter;

	// Use this for initialization
	void Start () {
		timeWhenCreated = Time.time;
	}

	public void setOwner (Character character){
		myCharacter = character;
	}

	void OnTriggerEnter (Collider col){
		Health health = col.gameObject.GetComponentInParent<Health>();
		if (health == null) return;
		health.decreaseHealth(20, myCharacter, Constants.magical, Constants.neutral);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeWhenCreated > 0.1) Destroy(this.gameObject);
	}
}
