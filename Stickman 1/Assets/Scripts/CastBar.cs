using UnityEngine;
using System.Collections;

public class CastBar : MonoBehaviour {

	Character myCharacter;

	public void finishCasting(){
		myCharacter.finishCasting();
	}

	// Use this for initialization
	void Start () {
		myCharacter = this.GetComponentInParent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

