using UnityEngine;
using System.Collections;

public class QuakeCollider : MonoBehaviour {

	public GameObject Status;
	private const float duration = 0.25f;
	private const float renew = 0.03f;
	private const float crippleEffectiveness = 0.25f;
	Character myCharacter;

	public void setParameters (Character character){
		myCharacter = character;
	}

	public void updateCharacter(Character character){
		Status[] statusArray = character.GetComponentsInChildren<Status>();
		float maxTime = 0;
		foreach (Status status in statusArray){
			if (status.getStatus() == Constants.quake){
				maxTime = Mathf.Max(maxTime, status.getParameter(Constants.duration));
			}
		}
		if (maxTime < renew){
			//renew quake status
			GameObject statusObject = Instantiate(Status, character.transform.position, Quaternion.identity) as GameObject;
			statusObject.transform.parent = character.transform;
			Status status = statusObject.GetComponent<Status>();
			float[] parameters = {duration};
			status.setParameters(null, parameters, true, Constants.quake, false);
			//slow
			GameObject statusObject2 = Instantiate(Status, character.transform.position, Quaternion.identity) as GameObject;
			statusObject2.transform.parent = character.transform;
			Status status2 = statusObject2.GetComponent<Status>();
			float[] parameters2 = {duration, crippleEffectiveness};
			status2.setParameters(null, parameters2, true, Constants.crippled, false);
			//deal damage
			Health health = character.GetComponent<Health>();
			health.decreaseHealthPassively(1, myCharacter, Constants.magical, Constants.neutral);
		}
	}

	void OnTriggerEnter (Collider col){
		Character character = col.GetComponentInParent<Character>();
		if (character != null){
			updateCharacter(character);
		}
	}

	void OnTriggerStay (Collider col){
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
