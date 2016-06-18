using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {

	private float time;
	private bool countDown = false;
	private bool destroyed = false;
	private int status;
	private Skill parent;

	public int getStatus() {
		return status;
	}

	public bool isDestroyed() {
		return destroyed;
	}


	public void setParameters (Skill sk, float t, bool b, int st){
		time = t;
		countDown = b;
		status = st;
		parent = sk;
		Character myCharacter = GetComponentInParent<Character>();
		myCharacter.UpdateStatus();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (countDown){
			time -= Time.deltaTime;
			if (time <= 0){
				destroyed = true;
				Character myCharacter = GetComponentInParent<Character>();
				myCharacter.UpdateStatus();
			}
		}
		if (parent == null){
			destroyed = true;
			Character myCharacter = GetComponentInParent<Character>();
			myCharacter.UpdateStatus();
		}
	}
}
