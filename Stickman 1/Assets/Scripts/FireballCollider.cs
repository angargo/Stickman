using UnityEngine;
using System.Collections;

public class FireballCollider : MonoBehaviour {

	private Character myCharacter;
	private const float duration = 3;
	private const float renew = 0.1f;
	public GameObject Status;

	void OnTriggerEnter2D (Collider2D col){
		Character character = col.gameObject.GetComponentInParent<Character>();
		if (character == null || character == myCharacter) return;
		Health health = character.GetComponent<Health>();
		health.decreaseHealth(8, myCharacter, Constants.magical, Constants.neutral);
		checkBurn(character);
	}

	/*void OnTriggerStay2D (Collider2D col){
		Character character = col.gameObject.GetComponentInParent<Character>();
		if (character == null) return;
		checkBurn(character);
	}*/

	void checkBurn(Character character){
		//put burn status
		GameObject statusObject = Instantiate(Status, character.transform.position, Quaternion.identity) as GameObject;
		statusObject.transform.parent = character.transform;
		Status status = statusObject.GetComponent<Status>();
		float[] parameters = {duration};
		status.setParameters(null, parameters, true, Constants.burn, false);
	}

	public void SetParameters(Character character){
		myCharacter = character;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
