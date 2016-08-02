using UnityEngine;
using System.Collections;

public class Earthquake : MonoBehaviour {

	private float time = 10;

	public void setParameters (Character character){
		QuakeCollider qc = GetComponentInChildren<QuakeCollider>();
		qc.setParameters(character);
	}

	void Update(){
		time -= Time.deltaTime;
		if (time <= 0){
			Destroy(this.gameObject);
		}
	}
}
