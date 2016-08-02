using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour {

	float time = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time <= 0) Destroy(this.gameObject);
	}
}
