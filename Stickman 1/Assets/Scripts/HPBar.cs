using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

	Player player;
	Health health;
	RectTransform rectTransform;


	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<Player>();
		health = player.GetComponent<Health>();
		rectTransform = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		float maxHP = health.getMaxHP(); //never 0
		float currentHP = health.getCurrentHP();
		float proportion = currentHP/maxHP;
		proportion = Mathf.Max(proportion, 0);
		rectTransform.localScale = new Vector3(proportion, 1, 1);
	}
}
