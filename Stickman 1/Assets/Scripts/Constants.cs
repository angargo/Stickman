using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {


	//Status
	static public int statusNumber = 7;
	static public int invulnerable = 0;
	static public int invisible = 1;
	static public int controlled = 2;
	static public int invulnerableMagic = 3;
	static public int invulnerablePhysical = 4;
	static public int quake = 5;
	static public int crippled = 6;

	//Skills
	static public int fireball = 0;
	static public int smokeTeleport = 1;
	static public int earthquake = 2;
	static public int magicMirror = 4;

	//AttackTypes
	static public int physical = 0;
	static public int magical = 1;
	static public int trueDMG = 2;

	//Elements
	static public int neutral = 0;

	//Parameters

	static public int duration = 0;
	static public int effectiveness = 1; 


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
