using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	const int fireball = 0;
	public GameObject fireballPrefab;
	const int smokeTeleport = 1;
	public GameObject smokePrefab;



	//clickNumber: some skills need to be used more than once [for instance 'smokeTeleport']
	public void performSkill(Character character, int skill, Vector3 mousePos){
		if (skill == fireball){
			GameObject newFireball = Instantiate(fireballPrefab, character.transform.position, Quaternion.identity) as GameObject;
			newFireball.transform.parent = character.transform;
			FireballSkill fireb = newFireball.GetComponent<FireballSkill>();
			fireb.setParameters(character, mousePos);
			fireb.startSkill();
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
			}
		}
	}

	public void cancelSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == fireball){
			FireballSkill fireb = skill.GetComponent<FireballSkill>();
			fireb.cancelSkill();
		}
		if (n == smokeTeleport){
			SmokeTeleport smoke = skill.GetComponent<SmokeTeleport>();
			smoke.cancelSkill();
		}
	}

	public void finishSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == fireball){
			FireballSkill fireb = skill.GetComponent<FireballSkill>();
			fireb.finishSkill();
		}
		if (n == smokeTeleport) return;
	}
}
