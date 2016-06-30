using UnityEngine;
using System.Collections;

public class QuakeCollider : MonoBehaviour {

	public GameObject quakeStatus;
	private const float duration = 0.2f;
	private const float renew = 0.1f;

	public void updateCharacter(Character character){
		Status[] statusArray = character.GetComponentsInChildren<Status>();
		float maxTime = 0;
		foreach (Status status in statusArray){
			if (status.getStatus() == Constants.quake){
				maxTime = Mathf.Max(maxTime, status.getTime());
			}
		}
		if (maxTime < duration){
			GameObject statusObject = Instantiate(quakeStatus, character.transform.position, Quaternion.identity) as GameObject;
			statusObject.transform.parent = character.transform;
			Status status = statusObject.GetComponent<Status>();
			status.setParameters(null, renew, true, Constants.quake, false);
		}
	}

	void OnTriggerEnter2D (Collider2D col){
		Character character = col.GetComponentInParent<Character>();
		if (character != null){
			updateCharacter(character);
		}
	}

	void OnTriggerStay2D (Collider2D col){
		Character character = col.GetComponentInParent<Character>();
		if (character != null){
			updateCharacter(character);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
