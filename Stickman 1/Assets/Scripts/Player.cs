using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
  
  public float speed = 1;
  public int spritesByDirection = 8;
  public int directions = 5;
  public float spriteIndex;
  private int spriteDirection;
  private SpritePosition spritePosition;
  
  private Vector3 direction;
  private Vector3 targetPosition;
  private bool isMoving = false;
  private int dir;
  public Sprite[] sprites;

  private Animator animator;
  private CameraPosition cameraPosition;

	// Use this for initialization
	void Start () {
	    isMoving = false;
	    spriteIndex = 0;
	    spriteDirection = 0;
	    direction = Vector3.down;
	    animator = this.GetComponent<Animator>();
	    cameraPosition = GameObject.FindObjectOfType<CameraPosition>();
	    spritePosition = this.GetComponentInChildren<SpritePosition>();
	    //Debug.Log(spriteRenderer);
	}

	void changePosition(){ //Solo funciona para suelo plano, else flota por ahi! [se puede arreglar 'ez']
	  Vector3 newPosition;
	  if ((this.transform.position - targetPosition).magnitude <= speed * Time.deltaTime) newPosition = targetPosition;
      else  newPosition = this.transform.position + direction * speed * Time.deltaTime;
      RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(newPosition)));
      bool collision = false;
      foreach (RaycastHit2D hit in hits) {
        if (hit.collider.gameObject.tag == "Obstacle") {
          collision = true;
          break;
        }
      }
      if (!collision) this.transform.position = newPosition;
      if (collision || Vector3.Dot(targetPosition - this.transform.position, direction) < Mathf.Epsilon) {
        isMoving = false;
        animator.SetBool("isWalking", false);
      }
      cameraPosition.FollowPlayer(transform.position);
	}

	public void changeSprite(){ //Again, solo funciona para suelo plano de momento
		Vector3 playerToCam = transform.position - cameraPosition.transform.position;
		Vector3 normalVector = Vector3.forward;
		playerToCam.z = 0;
		float angle = Vector3.Angle(playerToCam, direction);
		if (angle < 22.5) spriteDirection = 4;
		else if (angle < 67.5) spriteDirection = 3;
		else if (angle < 112.5) spriteDirection = 2;
		else if (angle < 157.5) spriteDirection = 1;
		else spriteDirection = 0;
		float sign = Vector3.Dot(normalVector, Vector3.Cross(playerToCam, direction));
		if (sign < 0 && spriteDirection > 0 && spriteDirection < 4) spritePosition.transform.localScale = new Vector3(-1,1,1);
		else spritePosition.transform.localScale = new Vector3(1,1,1);
		int j = (int) Mathf.Floor (spriteIndex);
		if (j > 7) j = 7;
		//Debug.Log(spritesByDirection*spriteDirection + j);

		spritePosition.setSprite(sprites[spritesByDirection*spriteDirection + j]);
		//Debug.Log(sprites[spritesByDirection*spriteDirection + j]);
	}
	
	// Update is called once per frame
	void Update () {
	  if (isMoving) changePosition();
	}
  
  void FixedUpdate () {
    
	}
  
  public void MoveToPosition(Vector3 position) {
    targetPosition = position;
    direction = (targetPosition - this.transform.position);
    direction.Normalize();
    isMoving = true;
    animator.SetBool("isWalking", true);
  }
}
