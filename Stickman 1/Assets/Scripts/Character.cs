using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

  public string myFolder;
  
  public float speed;
  public int directions;
  public float spriteIndex;
  private int spriteDirection;
  private SpritePosition spritePosition;
  
  private Vector3 direction;
  private Vector3 targetPosition;
  private int dir;
  public Sprite[] sprites;
  private Sprite[][][] spriteMatrix;

  public int totalStates;

  private const int idle = 0;
  private const int walking = 1;
  private const int attacking = 2;
  private int currentState;

  private bool isMovingToAttack;
  private bool isAttacking;
  private Character enemy;

  public int minAttack = 7;
  public int maxAttack = 10;


  public int[] stateLengths;

  private Animator animator;
  private CameraPosition cameraPosition;

	private AudioSource audioSource;
	private AudioClip[] audioClips;

	public void playClip(int a){
  		audioSource.clip = audioClips[a];
  		audioSource.Play();
  	}

	void createSpriteMatrix(){
		if (this.GetComponent<Player>() != null){
			//Debug.Log("Hola!!" + this);
	  		int cont = 0;
	  		spriteMatrix = new Sprite [totalStates][][];
	  		for (int i = 0; i < totalStates; ++i){
	  			spriteMatrix[i] = new Sprite[directions][];
	  			for (int j = 0; j < directions; ++j){
					spriteMatrix[i][j] = new Sprite[stateLengths[i]];
					//Patch for players!!!
					if (i == 1){
		  				for (int k = 0; k < stateLengths[i]; ++k){
		  					spriteMatrix[i][j][k] = sprites[cont];
		  					++cont;
		  				}
		  			}
	  				if (i == 2){
	  					spriteMatrix[0][j][0] = spriteMatrix[1][j][0];
	  					spriteMatrix[2][j][0] = spriteMatrix[1][j][0];

	  					//Debug.Log(spriteMatrix[0][0][0]);
	  				}
	  			}
	  		}
	  	}
	  	else{
			//Debug.Log("Deww!!" + this);
			int cont = 0;
	  		spriteMatrix = new Sprite [totalStates][][];
	  		for (int i = 0; i < totalStates; ++i){
	  			spriteMatrix[i] = new Sprite[directions][];
	  			if (i <= 1){
		  			for (int j = 0; j < directions; ++j){
						spriteMatrix[i][j] = new Sprite[stateLengths[i]];
		  				for (int k = 0; k < stateLengths[i]; ++k){
		  					spriteMatrix[i][j][k] = sprites[cont];
							//Debug.Log(spriteMatrix[0][0][0]);
		  					++cont;
		  				}
		  			}
		  		}
		  		else {
					for (int j = 0; j < directions; ++j){
						spriteMatrix[i][j] = new Sprite[stateLengths[i]];
		  				for (int k = 0; k < stateLengths[i]; ++k){
		  					spriteMatrix[i][j][k] = spriteMatrix[i-1][j][k];
		  				}
		  			}
		  		}

	  		}
	  	}
  	}

	// Use this for initialization
	void Start () {
	    spriteIndex = 0;
	    spriteDirection = 0;
	    direction = Vector3.down;
	    animator = this.GetComponent<Animator>();
	    cameraPosition = GameObject.FindObjectOfType<CameraPosition>();
	    spritePosition = this.GetComponentInChildren<SpritePosition>();

	    currentState = idle;
	    isMovingToAttack = false;
	    isAttacking = false;

		sprites = (Sprite[]) Resources.LoadAll<Sprite>("Sprites/" + myFolder);
		audioClips = (AudioClip[]) Resources.LoadAll<AudioClip> ("Audio/" + myFolder);
		audioSource = this.GetComponent<AudioSource>();
		createSpriteMatrix();
	}

	void changePosition(){ //Solo funciona para suelo plano, else flota por ahi! [se puede arreglar 'ez']
	  //Debug.Log ("Change Position!!!");
	  if (enemy != null && isMovingToAttack) {
	  	MoveToPosition(enemy.transform.position, true);
		if ((this.transform.position - targetPosition).magnitude < 0.5f){
			startAttacking();
			return;
		}
	  }
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
        currentState = idle;
        animator.SetInteger("currentState", idle);
      }
	  if (this.GetComponent<Player>() != null) cameraPosition.FollowPlayer(transform.position);
	}

	void startAttacking(){
		if (currentState != attacking){
			currentState = attacking;
			animator.SetInteger("currentState", attacking);
			//Debug.Log("I'm Attacking!!!");
		}
	}

	public void setAttacking (int a){
		Debug.Log ("Set Attacking: " + a);
		if (a > 0) isAttacking = true;
		else isAttacking = false;
	}

	public void autoAttack(){
		if (enemy == null){
			Debug.Log("No enemy!!!");
			return;
		}
		int extraAttack = (int) Mathf.Floor(Random.value * (float)(maxAttack - minAttack + 1));
		int attack = minAttack + extraAttack;
		Health health = enemy.gameObject.GetComponent<Health>();
		if (health == null){
			Debug.Log ("No health component!!!!");
			return;
		}
		health.decreaseHealth(attack, this);
		if (enemy == null){
			isMovingToAttack = false;
			isAttacking = false;
			currentState = idle;
			animator.SetInteger("currentState", idle);
		}
	}
		

	public void changeSprite(){ //Again, solo funciona para suelo plano de momento
		Vector3 playerToCam = transform.position - cameraPosition.transform.position;
		Vector3 normalVector = Vector3.forward;
		playerToCam.z = 0;
		float angle = Vector3.Angle(playerToCam, direction);
		if (directions == 5){
			if (angle < 22.5) spriteDirection = 4;
			else if (angle < 67.5) spriteDirection = 3;
			else if (angle < 112.5) spriteDirection = 2;
			else if (angle < 157.5) spriteDirection = 1;
			else spriteDirection = 0;
		}
		else if (directions == 2){
			if (angle < 90) spriteDirection = 1;
			else spriteDirection = 0;
		}
		float sign = Vector3.Dot(normalVector, Vector3.Cross(playerToCam, direction));
		if (sign < 0 && (directions == 2 || (spriteDirection > 0 && spriteDirection < 4))) spritePosition.transform.localScale = new Vector3(-1,1,1);
		else spritePosition.transform.localScale = new Vector3(1,1,1);
		int j = (int) Mathf.Floor (spriteIndex);
		j %= stateLengths[currentState];

		spritePosition.setSprite(spriteMatrix[currentState][spriteDirection][j]);
	}
	
	// Update is called once per frame
	void Update () {
	//Debug.Log(currentState);
	  if (isMovingToAttack && !isAttacking && enemy == null){
	  	isMovingToAttack = false;
	  	isAttacking = false;
	  	currentState = idle;
	  	animator.SetInteger("currentState", idle);
	  }
	  if (this.GetComponent<Player>() == null && enemy != null){
	  	//Debug.Log(enemy + " " + isMovingToAttack + " " + isAttacking);
	  }
	  if (!isAttacking && (currentState == walking || isMovingToAttack)) changePosition();
	  changeSprite();
	  //if (this.GetComponent<Player>() == null && enemy != null) Debug.Log(enemy);
	}
  
  void FixedUpdate () {
    
	}
  
	  public void MoveToPosition(Vector3 position, bool attack) {
	  	//Debug.Log (position);
	  	if (position != targetPosition){
		    targetPosition = position;
		    direction = (targetPosition - this.transform.position);
		    direction.Normalize();
		}
		if (!attack) {
			isMovingToAttack = false;
			isAttacking = false;
			enemy = null;
		}
	    currentState = walking;
	    animator.SetInteger("currentState", walking);
	  }

	public void attackEnemy(Character givenEnemy){
		isMovingToAttack = true;
		enemy = givenEnemy;
	}

	public bool isIdle(){
		return currentState == idle;
	}

	public void setIdle(){
		isMovingToAttack = false;
		isAttacking = false;
		currentState = idle;
		animator.SetInteger("currentState", idle);
	}

}