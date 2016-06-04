using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	private LevelManager levelManager;

	public int goToLevel = 1;

	void OnTriggerEnter2D (Collider2D col){
		Debug.Log (col.gameObject.tag);
		if(col.gameObject.tag == "Player") levelManager.LoadLevelWithIndex(goToLevel);
		Debug.Log ("Collided with " + col);
	}

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
