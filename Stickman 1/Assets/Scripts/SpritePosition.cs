using UnityEngine;
using System.Collections;

public class SpritePosition : MonoBehaviour {

	private CameraPosition cameraPosition;

	// Use this for initialization
	void Start () {
		cameraPosition = GameObject.FindObjectOfType<CameraPosition>();
	}
	
	// Update is called once per frame
	void Update () {
		LookAtCamera();
	}

	public void LookAtCamera(){
		this.transform.rotation = Quaternion.identity;
		this.transform.Rotate(cameraPosition.transform.rotation.eulerAngles);
	}
}
