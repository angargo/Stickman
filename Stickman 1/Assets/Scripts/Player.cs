using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  	private Character myCharacter;
  	//private SkillManager skillManager;

	// Use this for initialization
	void Start () {
		myCharacter = this.GetComponent<Character>();
		//skillManager = GameObject.FindObjectOfType<SkillManager>();
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

	public void castSkill(int skill, Vector3 position, Character character){
		//myCharacter.performDefaultSkill(position);
		myCharacter.performSkill(skill, position, character);
	}
}


