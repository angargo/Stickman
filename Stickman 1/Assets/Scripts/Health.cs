using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int HP = 50;
	private Enemy enemy;
	private Character myCharacter;

	public void decreaseHealth (int a, Character character){
		HP -= a;
		if (a > 0)
			myCharacter.isHit ();
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
		myCharacter = this.GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
