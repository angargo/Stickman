using UnityEngine;
using System.Collections;

public class ForceFieldAnimate : MonoBehaviour {

    public float scrollspeed = 0.5f;

    private bool U = true;
    private bool V = false;

    private float offset;

    public Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        offset = Time.time * scrollspeed % 1;

        if (U & V) mat.mainTextureOffset = new Vector2(offset, offset);
        else if (U) mat.mainTextureOffset = new Vector2(0, offset);
        else if (V) mat.mainTextureOffset = new Vector2(offset, 0);
    }
}
