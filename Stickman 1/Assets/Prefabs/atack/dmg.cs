using UnityEngine;
using System.Collections;

public class dmg : MonoBehaviour {

    public float vel = 1;
    public float fadespeed = 0.02f;
    public TextMesh col;
    public SpriteRenderer critic;
    //public Camera cam;
    //publi
    Color c;
    // Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -vel);
        
	}
	
	// Update is called once per frame
	void Update () {
        //transform.LookAt(cam.transform);

        c = critic.color;
        c.a = c.a - fadespeed; 
        critic.color = c;
        if (c.a <= 0) Destroy(this.gameObject);
        //Debug(c.a);
        c = col.color;
        c.a = c.a - fadespeed;
        col.color = c;
        if (c.a <= 0) Destroy(this.gameObject);

	}
}
