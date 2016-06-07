using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  private Character myCharacter;

	// Use this for initialization
	void Start () {
		myCharacter = this.GetComponent<Character>();
	}

	void Update () {

	}
  
  	public void MoveToPosition(Vector3 position) {
		myCharacter.stopChasing ();
  		myCharacter.SetTargetPosition(position);
  	}

	public void attackEnemy(Character givenEnemy){
		myCharacter.chaseEnemy(givenEnemy);
	}

	public void castSkill(Vector3 position){
		myCharacter.performDefaultSkill(position);
	}
}


