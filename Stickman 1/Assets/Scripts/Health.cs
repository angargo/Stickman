using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int MaxHP = 50;
	private int HP = 50;
	private Enemy enemy;
	private Character myCharacter;

	public void decreaseHealth (int x, Character character, int damageType, int element){
		int a = computeDamage(x, character , damageType, element);
		if (a > 0){
			HP -= a;
			myCharacter.isHit ();
			if (enemy != null && HP <= 0){ //now player is immortal!
				myCharacter.die();
				//Destroy(this.gameObject);
			}
			if (enemy != null && character != null && character.GetComponent<Enemy>() == null){
				enemy.beingAttacked(character);
			}
		}
		else HP -= a;
	}

	public void decreaseHealthPassively (int x, Character character, int damageType, int element){
		int a = computeDamage(x, character , damageType, element);
		if (a > 0){
			HP -= a;
			if (enemy != null && HP <= 0){ //now player is immortal!
				myCharacter.die();
				//Destroy(this.gameObject);
			}
			if (enemy != null && character != null && character.GetComponent<Enemy>() == null){
				enemy.beingAttacked(character);
			}
		}
		else HP -= a;
	}

	private int computeDamage(int x, Character c, int dT, int e){
		if (myCharacter.getStatus(Constants.invulnerable)) return 0;
		if (myCharacter.getStatus(Constants.invulnerableMagic) && dT == Constants.magical) return 0;
		//more stuff
		return x;
	}

	public int getCurrentHP(){
		return HP;
	}

	public int getMaxHP(){
		return MaxHP;
	}

	// Use this for initialization
	void Start () {
		enemy = this.GetComponent<Enemy>();
		myCharacter = this.GetComponent<Character>();
		HP = MaxHP;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
