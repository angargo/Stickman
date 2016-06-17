using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	const int fireball = 0;
	public GameObject fireballPrefab;
	const int smokeTeleport = 1;
	public GameObject smokePrefab;



	//clickNumber: some skills need to be used more than once [for instance 'smokeTeleport']
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
			SmokeTeleport smoke = character.GetComponentInChildren<SmokeTeleport>();
			if (smoke == null){
				GameObject newSmoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;
				newSmoke.transform.parent = character.transform;
				smoke = newSmoke.GetComponent<SmokeTeleport>();
				smoke.setParameters(character, mousePos);
				smoke.startSkill();
			}
			else{
				smoke.setParameters(character, mousePos);
				smoke.secondCast();
				//smoke.secondCast();
			}
			/*SpriteRenderer sr = character.GetComponentInChildren<SpriteRenderer>();
			if (clickNumber == 0){
				//We set 3 seconds to wait for the second click, also we gain invulnerability [not fully implemented yet]
				character.setStatus(0,3);
				character.setStatus(1,3);
				sr.enabled = false;
				GameObject smoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;


			}
			else if (clickNumber == 1){
				//We finish waiting for clicks
				character.setStatus(0,0);
				character.setStatus(1,0);
				character.gameObject.transform.position = mousePos;
				sr.enabled = true;
				GameObject smoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;
			}
			else { //clickNumber == -1
				//We finish waiting for clicks
				character.setStatus(0,0);
				character.setStatus(1,0);
				sr.enabled = true;
				GameObject smoke = Instantiate(smokePrefab, character.transform.position, Quaternion.identity) as GameObject;
			}*/
		}
	}

	public void cancelSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == fireball) return;
		if (n == smokeTeleport){
			SmokeTeleport smoke = skill.GetComponent<SmokeTeleport>();
			smoke.cancelSkill();
		}
	}
}
