using UnityEngine;
using System.Collections;

public class Earthquake : MonoBehaviour {

	Character myCharacter;
	private float time = 10;

	public void setParameters (Character character){
		myCharacter = character;
	}

	void Update(){
		time -= Time.deltaTime;
		if (time <= 0){
			Destroy(this.gameObject);
		}
	}
}
