using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int MaxHP = 50;
	private int HP = 50;
	private Enemy enemy;
	private Character myCharacter;
	private SpritePosition spritePosition;
    public GameObject dmgin;
    private bool isBurning;

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
		HP = Mathf.Min(MaxHP, HP);

		//Dmg text
		putDmgText(a);
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
        putDmgText(a);
    }

    void putDmgText(int a){
		GameObject dmg = Instantiate(dmgin, myCharacter.transform.position + new Vector3(0,0,-1), Camera.main.transform.rotation) as GameObject;
        TextMesh info = dmg.GetComponentInChildren<TextMesh>();
        info.text = a.ToString();
        dmg.transform.parent = spritePosition.transform;
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
		spritePosition = this.GetComponentInChildren<SpritePosition>();
		HP = MaxHP;
		isBurning = false;
	}

	public void BurnDmg(){
		decreaseHealthPassively(2, myCharacter, Constants.magical, Constants.neutral);
	}
	
	// Update is called once per frame
	void Update () {
		if (myCharacter.isDead()){
			CancelInvoke();
			return;
		}
		if (!isBurning && myCharacter.getStatus(Constants.burn)){
			isBurning = true;
			InvokeRepeating("BurnDmg", 0, 0.5f);
		}
		else if (isBurning && !myCharacter.getStatus(Constants.burn)){
			isBurning = false;
			CancelInvoke("BurnDmg");
		}
	}
}
