using UnityEngine;
using System.Collections;

public class MagicMirrorCollider : MonoBehaviour {

	public GameObject Body;

	public float intensity = 50.0f;
	public float radius = 0.1f;
	public float lifetime = 2.5f;
	public float R = 2f;

	public float shotSpeed = 3.0f;

	void OnTriggerEnter (Collider col){
		Projectile p = col.gameObject.GetComponentInParent<Projectile>();
		if (p != null){
			p.reflect(GetComponentInParent<Character>());
			var effect = Body.GetComponent<IHittable>();
			if (effect != null)
			{
				if (true)
				//if (true)
				{
					effect.Add(computeNormalizedPosition(col.gameObject),intensity, radius, lifetime, shotSpeed);
				}
				/*else 
				{
					effect.Add(p.transform.position, 2.0f, 2.0f, 2.0f, shotSpeed);
				}*/

				//GameObject.Destroy (p.gameObject);

			}
		}

	}

	Vector3 computeNormalizedPosition(GameObject p){
		Vector3 diff = p.transform.position - transform.position;
		diff.Normalize();
		diff*=R;
		return transform.position + diff;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
