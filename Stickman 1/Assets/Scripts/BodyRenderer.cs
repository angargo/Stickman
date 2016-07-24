using UnityEngine;
using System.Collections;

public class BodyRenderer : MonoBehaviour {

	Vector3 direction = Vector3.zero;

	private SpriteRenderer spriteRenderer;
	private Animator animator;
	bool haveSprites;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		animator = this.GetComponentInChildren<Animator>();
		haveSprites = (spriteRenderer != null);
	}

	public void setInvisible(bool b){
		if (haveSprites){
			if (b) spriteRenderer.enabled = false;
			else spriteRenderer.enabled = true;
		}
		else {
			if (b){
				for (int i = 0; i < animator.transform.childCount; ++i){
					Transform transform = animator.transform.GetChild(i);
					transform.gameObject.SetActive(false);
				}
			}
			else{
				for (int i = 0; i < animator.transform.childCount; ++i){
					Transform transform = animator.transform.GetChild(i);
					transform.gameObject.SetActive(true);
				}
			}
		}
	}

	public void setSprite(Sprite s){
		spriteRenderer.sprite = s;
	}

	public void lookTo (Vector3 dir){
		if (dir != direction){
			direction = dir;
			float angle = Mathf.Atan2(direction.y, direction.x);
			angle *= 180/Mathf.PI;
			angle -= 90;
			this.transform.localRotation = Quaternion.identity;
			this.transform.Rotate(new Vector3 (0,0,angle));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
