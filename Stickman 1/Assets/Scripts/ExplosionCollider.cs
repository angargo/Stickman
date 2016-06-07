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

	void OnTriggerEnter2D (Collider2D col){
		Debug.Log(col.gameObject);
		Health health = col.gameObject.GetComponentInParent<Health>();
		if (health == null) return;
		health.decreaseHealth(20, myCharacter);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeWhenCreated > 0.5) Destroy(this.gameObject);
	}
}
