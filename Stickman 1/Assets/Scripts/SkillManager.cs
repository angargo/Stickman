using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	const int fireball = 0;
	public GameObject fireballPrefab;
	const int smokeTeleport = 1;
	public GameObject smokePrefab;



	//some skills need to be used more than once [for instance 'smokeTeleport']
	public void performSkill(Character character, int skill, bool finishedCasting, int clickNumber, Vector3 mousePos){
		if (skill == fireball){
			if (!finishedCasting){
				character.setCasting(1);
			}
			else {
				GameObject fireb = Instantiate(fireballPrefab, character.transform.position, Quaternion.identity) as GameObject;
				Fireball fire = fireb.GetComponent<Fireball>();
				fire.setTarget(mousePos, character);
			}
		}

		else if (skill == smokeTeleport){
			SpriteRenderer sr = character.GetComponentInChildren<SpriteRenderer>();
			if (clickNumber == 0){
				character.setStatus(0,3);
				character.setStatus(1,3);
				sr.enabled = false;
				GameObject smoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;


			}
			else if (clickNumber == 1){
				character.setStatus(0,0);
				character.setStatus(1,0);
				character.gameObject.transform.position = mousePos;
				sr.enabled = true;
				GameObject smoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;
			}
			else { //clickNumber == -1
				character.setStatus(0,0);
				character.setStatus(1,0);
				//character.gameObject.transform.position = mousePos;
				sr.enabled = true;
				GameObject smoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;
			}
		}

	}

}
