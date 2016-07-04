using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	public GameObject bombPrefab;
	public GameObject smokePrefab;
	public GameObject magicMirrorPrefab;
	public GameObject earthquakePrefab;
	public GameObject healPrefab;
	public GameObject fireballPrefab;

	//clickNumber: some skills need to be used more than once [for instance 'smokeTeleport']
	public void performSkill(Character character, int skill, Vector3 mousePos, Character targetCharacter){
		if (skill == Constants.bomb){
			GameObject newBomb = Instantiate(bombPrefab, character.transform.position, Quaternion.identity) as GameObject;
			newBomb.transform.parent = character.transform;
			BombSkill bomb = newBomb.GetComponent<BombSkill>();
			bomb.setParameters(character, mousePos);
			bomb.startSkill();
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
		else if (skill == Constants.earthquake){
			GameObject newEarthquake = Instantiate(earthquakePrefab, character.transform.position, Quaternion.identity) as GameObject;
			newEarthquake.transform.parent = character.transform;
			EarthquakeSkill earthquake = newEarthquake.GetComponent<EarthquakeSkill>();
			earthquake.setParameters(character, mousePos);
			earthquake.startSkill();
		}
		else if (skill == Constants.heal){
			GameObject newHeal = Instantiate(healPrefab, character.transform.position, Quaternion.identity) as GameObject;
			newHeal.transform.parent = character.transform;
			HealSkill heal = newHeal.GetComponent<HealSkill>();
			heal.setParameters(character, targetCharacter);
			heal.startSkill();
		}
		else if (skill == Constants.fireball){
			GameObject newFireball = Instantiate(fireballPrefab, character.transform.position, Quaternion.identity) as GameObject;
			newFireball.transform.parent = character.transform;
			FireballSkill fireball = newFireball.GetComponent<FireballSkill>();
			fireball.setParameters(character, mousePos);
			fireball.startSkill();
		}
	}

	public void cancelSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == Constants.bomb){
			BombSkill bomb = skill.GetComponent<BombSkill>();
			bomb.cancelSkill();
		}
		if (n == Constants.smokeTeleport){
			SmokeTeleport smoke = skill.GetComponent<SmokeTeleport>();
			smoke.cancelSkill();
		}
		if (n == Constants.magicMirror){
			MagicMirror magic = skill.GetComponent<MagicMirror>();
			magic.cancelSkill();
		}
		if (n == Constants.earthquake){
			EarthquakeSkill earthquake = skill.GetComponent<EarthquakeSkill>();
			earthquake.cancelSkill();
		}
		if (n == Constants.fireball){
			FireballSkill fireball = skill.GetComponent<FireballSkill>();
			fireball.cancelSkill();
		}
	}

	public void finishSkill(Skill skill){
		int n = skill.getSkillNumber();
		if (n == Constants.bomb){
			BombSkill bomb = skill.GetComponent<BombSkill>();
			bomb.finishSkill();
		}
		if (n == Constants.smokeTeleport) return;
		if (n == Constants.magicMirror) return;
		if (n == Constants.earthquake){
			EarthquakeSkill earthquake = skill.GetComponent<EarthquakeSkill>();
			earthquake.finishSkill();
		}
		if (n == Constants.fireball){
			FireballSkill fireball = skill.GetComponent<FireballSkill>();
			fireball.finishSkill();
		}
	}

	public void cancelAllOtherSkills (Character character, Skill skill){
		skill.setCancel(false);
		character.cancelAllSkills();
		skill.setCancel(true);
	}

}
