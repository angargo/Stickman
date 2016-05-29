using UnityEngine;
using System.Collections;

public class SpritePosition : MonoBehaviour {

	private CameraPosition cameraPosition;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
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

	public void setSprite(Sprite s){
		spriteRenderer.sprite = s;
	}
}
