using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	private Vector3 targetPosition, direction;
	public GameObject explosion;
	private Character myCharacter;
	private Projectile myProjectile;
	public float speed = 8;

	// Use this for initialization
	void Awake () {
		myProjectile = GetComponent<Projectile>();
	}

	private void explode(){
		GameObject expl = Instantiate (explosion, this.transform.position, Quaternion.identity) as GameObject;
		expl.GetComponentInChildren<ExplosionCollider>().setOwner(myCharacter); 
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (myProjectile.hasArrived()) explode();
	}

	public void setParameters(Vector3 v, Character character){
		myProjectile.SetParameters(v, speed, character);
		myCharacter = character;
		//Debug.Log("Target set to " + v);
	}
}
