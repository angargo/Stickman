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
  
  	public void MoveToPosition(Vector3 position, bool attack) {
  		myCharacter.MoveToPosition(position, attack);
  	}

	public void attackEnemy(Character givenEnemy){
		myCharacter.attackEnemy(givenEnemy);
	}
}


