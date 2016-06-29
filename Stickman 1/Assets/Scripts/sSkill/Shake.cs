using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    public float mida;
    public float temps_shacke = 0.25f;
    private Vector3 pos;
    private float temps;
    void Start()
    {
        mida = 0.2f;
        pos = transform.position;
    }

	// Update is called once per frame
	void Update () {
        temps += Time.deltaTime;
        if (temps > temps_shacke)
        {
            transform.position = new Vector3(pos.x + Random.Range(0, mida), pos.y + Random.Range(0, mida), pos.z);
            temps = 0;
        }
        
	}
}
