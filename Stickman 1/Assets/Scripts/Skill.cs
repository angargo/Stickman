using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

	private bool canBeCanceled;
	private int skillNumber;
	private bool casting;
	private Character myCharacter;
	private Vector3 startingPos;
	private bool projectile;

	public void setCancel(bool b){
		canBeCanceled = b;
	}

	public void SetCasting(bool b){
		casting = b;
	}

	public bool isCasting(){
		return casting;
	}

	public bool canCancel() {
		return canBeCanceled;
	}

	public int getSkillNumber(){
		return skillNumber;
	}

	public void setSkillNumber(int a){
		skillNumber = a;
	}

	public void SetParameters(Character character, bool p){
		myCharacter = character;
		projectile = p;
	}

	void Awake(){
		startingPos = this.transform.position;
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
