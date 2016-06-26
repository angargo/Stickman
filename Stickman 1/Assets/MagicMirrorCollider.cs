using UnityEngine;
using System.Collections;

public class MagicMirrorCollider : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col){
		Projectile p = col.gameObject.GetComponentInParent<Projectile>();
		if (p != null){
			p.reflect(GetComponentInParent<Character>());
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
