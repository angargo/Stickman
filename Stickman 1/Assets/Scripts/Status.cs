using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {

	//EFFICIENCY: DO SUBROUTINES & STATUSMANAGER

	private float time = 10;
	private bool countDown = true;
	private bool destroyed = false;
	private bool depending = true;
	private int status;
	private Skill parent;

	public int getStatus() {
		return status;
	}

	public bool isDestroyed() {
		return destroyed;
	}


	public void setParameters (Skill sk, float t, bool b, int st, bool d){
		time = t;
		countDown = b;
		status = st;
		parent = sk;
		depending = d;
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
		if (depending && parent == null){
			destroyed = true;
			Character myCharacter = GetComponentInParent<Character>();
			myCharacter.UpdateStatus();
		}
		if (destroyed) Destroy(this.gameObject);
	}
}
