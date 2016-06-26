using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	private LevelManager levelManager;

	public int goToLevel = 1;

	void OnTriggerEnter2D (Collider2D col){
		if(col.gameObject.tag == "Player") levelManager.LoadLevelWithIndex(goToLevel);
	}

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
