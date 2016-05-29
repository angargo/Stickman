using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
  
  public float speed = 1;
  
  private Vector3 direction;
  private Vector3 targetPosition;
  private bool isMoving = false;
  
  private Animator animator;
  private CameraPosition cameraPosition;

	// Use this for initialization
	void Start () {
    isMoving = false;
    animator = this.GetComponent<Animator>();
    cameraPosition = GameObject.FindObjectOfType<CameraPosition>();
	}
	
	// Update is called once per frame
	void Update () {
    if (isMoving) {
      Vector3 newPosition = this.transform.position + direction * Mathf.Min(speed * Time.deltaTime, (this.transform.position - targetPosition).magnitude);
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
    }
    cameraPosition.FollowPlayer(transform.position);
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
