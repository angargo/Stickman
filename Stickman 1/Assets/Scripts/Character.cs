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
  private const int beinghit = 3;
  private const int dying = 4;
  private int currentState;

  private bool isChasing;
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
	    isChasing = false;

		sprites = (Sprite[]) Resources.LoadAll<Sprite>("Sprites/" + myFolder);
		audioClips = (AudioClip[]) Resources.LoadAll<AudioClip> ("Audio/" + myFolder);
		audioSource = this.GetComponent<AudioSource>();
		createSpriteMatrix();
	}

	void UpdatePosition(){
		if (currentState > walking)
			return;
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
			targetPosition = transform.position;
			SetCurrentState (walking, false);
      }
	}

	void UpdateCamera() {
		if (this.GetComponent<Player>() != null) cameraPosition.FollowPlayer(transform.position);
	}

	public void setAttacking (int a){
		Debug.Log ("Set Attacking: " + a);
		SetCurrentState (attacking, a == 1);
	}

	public void setBeingHit (int a){
		Debug.Log ("Set Being Hit: " + a);
		SetCurrentState (beinghit, a == 1);
	}

	public void setAttackingBool (bool b){
		Debug.Log ("Set Attacking: " + b);
		SetCurrentState (attacking, b);
	}

	public void autoAttack(){
		Debug.Assert (enemy != null);
		Health health = enemy.gameObject.GetComponent<Health>();
		Debug.Assert (health != null);
		int extraAttack = (int) Mathf.Floor(Random.value * (float)(maxAttack - minAttack + 1));
		int attack = minAttack + extraAttack;
		health.decreaseHealth(attack, this);
	}
		

	public void UpdateSprite(){ //Again, solo funciona para suelo plano de momento
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
		UpdateCurrentState ();
		UpdateSprite();
	    UpdatePosition(); // Consider moving this to FixedUpdate
		UpdateCamera ();
	}

	void UpdateCurrentState() {
		if (isChasing) {
			if (enemy == null) {
				// If the enemy disappears, cry.
				isChasing = false;
				targetPosition = transform.position;
				SetCurrentState (attacking, false);
				SetCurrentState (walking, false);
			} else if (IsObjectInRange (enemy.gameObject)) {
				SetCurrentState (attacking);
			} else if (currentState != attacking) {
				SetTargetPosition (enemy.transform.position);
			}
		}
		if (HasTargetPosition())
			SetCurrentState (walking);
		animator.SetInteger("currentState", currentState);
	}

	bool IsObjectInRange(GameObject obj) {
		return (obj.transform.position - transform.position).magnitude <= 0.5f;
	}

	void SetCurrentState(int state, bool b = true) {
		if (!b && state == currentState)
			currentState = idle;
		if (b) {
			if (state > currentState || (currentState == attacking && state == walking)) {
				currentState = state;
				//change animator? probably not
			}
		}
	}

	bool HasTargetPosition() {
		return transform.position != targetPosition;
	}
  
  void FixedUpdate () {
    
	}
  
	  public void SetTargetPosition(Vector3 position) {
	  	//Debug.Log (position);
	  	if (position != targetPosition){
		    targetPosition = position;
		    direction = (targetPosition - this.transform.position);
		    direction.Normalize();
		}
	  }

	public bool IsIdle() {
		return currentState == idle;
	}

	public void chaseEnemy(Character givenEnemy){
		isChasing = true;
		enemy = givenEnemy;
	}

	public void stopChasing() {
		isChasing = false;
	}

	public void isHit(){
		SetCurrentState (beinghit);
	}

}