using UnityEngine;
using System.Collections;

public class AnimatorFunctions : MonoBehaviour {

	Character myCharacter;


	public void autoAttack(){
		myCharacter.autoAttack();
	}

	public void setAttacking (int a){
		myCharacter.setAttacking(a);
	}

	public void setCasting (int a){
		myCharacter.setCasting(a);
	}

	public void setBeingHit (int a){
		myCharacter.setBeingHit(a);
	}

	public void finishCasting(){
		myCharacter.finishCasting();
	}

	public void playClip(int a){
  		myCharacter.playClip(a);
  	}

	// Use this for initialization
	void Start () {
		myCharacter = this.GetComponentInParent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
