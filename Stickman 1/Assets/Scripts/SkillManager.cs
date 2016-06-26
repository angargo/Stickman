using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	public GameObject fireballPrefab;
	public GameObject smokePrefab;
	public GameObject magicMirrorPrefab;

	//clickNumber: some skills need to be used more than once [for instance 'smokeTeleport']
	public void performSkill(Character character, int skill, Vector3 mousePos){
		if (skill == Constants.fireball){
			GameObject newFireball = Instantiate(fireballPrefab, character.transform.position, Quaternion.identity) as GameObject;
			newFireball.transform.parent = character.transform;
			FireballSkill fireb = newFireball.GetComponent<FireballSkill>();
			fireb.setParameters(character, mousePos);
			fireb.startSkill();
		}
		else if (skill == Constants.smokeTeleport){
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
		else if (skill == Constants.magicMirror){
			GameObject newMagicMirror = Instantiate(magicMirrorPrefab, character.transform.position, Quaternion.identity) as GameObject;
			newMagicMirror.transform.parent = character.transform;
		    MagicMirror mm = newMagicMirror.GetComponent<MagicMirror>();
			mm.setParameters(character);
			mm.startSkill();
		}
	}

	public void cancelSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == Constants.fireball){
			FireballSkill fireb = skill.GetComponent<FireballSkill>();
			fireb.cancelSkill();
		}
		if (n == Constants.smokeTeleport){
			SmokeTeleport smoke = skill.GetComponent<SmokeTeleport>();
			smoke.cancelSkill();
		}
		if (n == Constants.magicMirror){
			MagicMirror magic = skill.GetComponent<MagicMirror>();
			magic.cancelSkill();
		}
	}

	public void finishSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == Constants.fireball){
			FireballSkill fireb = skill.GetComponent<FireballSkill>();
			fireb.finishSkill();
		}
		if (n == Constants.smokeTeleport) return;
		if (n == Constants.magicMirror) return;
	}

	public void cancelAllOtherSkills (Character character, Skill skill){
		skill.setCancel(false);
		character.cancelAllSkills();
		skill.setCancel(true);
	}

}
