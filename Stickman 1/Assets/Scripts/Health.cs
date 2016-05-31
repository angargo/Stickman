using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int HP = 50;
	public Enemy enemy;

	public void decreaseHealth (int a, Character character){
		HP -= a;
		if (enemy != null && HP <= 0){ //now player is immortal!
			Destroy(this.gameObject);
		}
		if (enemy != null){
			enemy.beingAttacked(character);
		}
	}

	// Use this for initialization
	void Start () {
		enemy = this.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
