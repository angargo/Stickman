using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {


	private bool countDown = true;
	private bool destroyed = false;
	private bool depending = true;
	private int status;
	private Skill parent;
	private float[] parameters;

	public int getStatus() {
		return status;
	}

	public bool isDestroyed() {
		return destroyed;
	}

	public float getParameter(int a){
		return parameters[a];
	}


	public void setParameters (Skill auxSkill, float[] auxParameters, bool auxCountdown, int auxStatus, bool auxDepending){
		parameters = auxParameters;
		countDown = auxCountdown;
		status = auxStatus;
		parent = auxSkill;
		depending = auxDepending;
		Character myCharacter = GetComponentInParent<Character>();
		myCharacter.UpdateStatus();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (countDown){
			parameters[Constants.duration] -= Time.deltaTime;
			if (parameters[Constants.duration] <= 0){
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
