using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	private Vector3 targetPosition, direction;
	private Projectile myProjectile;
	private FireballCollider fireCollider;
	public float speed = 5;

	// Use this for initialization
	void Awake () {
		myProjectile = GetComponent<Projectile>();
		fireCollider = GetComponentInChildren<FireballCollider>();
	}

	private void explode(){
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (myProjectile.hasArrived()) explode();
	}

	public void setParameters(Vector3 v, Character character){
		myProjectile.SetParameters(v, speed, character);
		fireCollider.SetParameters(character);
	}
}
