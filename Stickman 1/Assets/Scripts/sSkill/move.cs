using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

    public float v = 2;
    private Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = transform.position;
        GetComponent<Rigidbody>().velocity = new Vector3( v, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x > 10) transform.position = pos;
	}
}
