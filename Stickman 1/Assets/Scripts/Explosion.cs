using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	private float timeWhenCreated;

	// Use this for initialization
	void Start () {
		timeWhenCreated = Time.time;
	
	}

	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeWhenCreated > 3) Destroy(this.gameObject);
	}
}
