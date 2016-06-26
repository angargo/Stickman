using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

	private bool canBeCanceled;
	private int skillNumber;
	private bool casting;


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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
