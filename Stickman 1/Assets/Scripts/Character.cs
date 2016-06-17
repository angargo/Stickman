using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

  //Input given
  public string myFolder;
  public float speed;
  public int[] directions;
  public int[] stateLengths;
  public int totalStates;

  //Movement stuff
  private Vector3 direction;
  private Vector3 targetPosition;
  private int dir;

  //Sprite stuff
  private Sprite[] sprites;
  private Sprite[][][] spriteMatrix;
  public float spriteIndex;
  private int spriteDirection;


  //States
  private const int idle = 0;
  private const int walking = 1;
  private const int casting = 2;
  private const int beinghit = 3;
  private const int attacking = 4;
  private const int dying = 5;
  private int currentState;

  //State conditioners
  private bool isChasing;
  private Character enemy;
  private Vector3 targetSkill;
  private int currentSkill;
  private bool canMove; //not being moved by a skill


  //Autoattacks
  public int minAttack = 7;
  public int maxAttack = 10;


  //Other components
  private Animator animator;
  private CameraPosition cameraPosition;
  private AudioSource audioSource;
  private AudioClip[] audioClips;
  private SkillManager skillManager;
  private SpritePosition spritePosition;

  //Status, ignore by now...
  private float[] status; //test
  const int waitingForSkill = 0;
  const int invulnerability = 1;



	//START FUNCTIONS!!

	void Start () {
		initializeVariables();
		createSpriteMatrix();
	}

	void initializeVariables(){

		//Get components
		animator = this.GetComponent<Animator>();
	    cameraPosition = GameObject.FindObjectOfType<CameraPosition>();
	    spritePosition = this.GetComponentInChildren<SpritePosition>();
		audioSource = this.GetComponent<AudioSource>();
		skillManager = GameObject.FindObjectOfType<SkillManager>();

	    //Read sprites and audio
		sprites = (Sprite[]) Resources.LoadAll<Sprite>("Sprites/" + myFolder);
		audioClips = (AudioClip[]) Resources.LoadAll<AudioClip> ("Audio/" + myFolder);

		//Initial frame
		spriteIndex = 0;
	    spriteDirection = 0;

	    //Current state
	    direction = Vector3.down;
	    currentState = idle;
	    isChasing = false;
	    canMove = true;

	    //Status
	    status = new float[2];
	}

	void createSpriteMatrix(){ //read all sprites and arrange them in a matrix [state][direction][frame]
		int cont = 0;
  		spriteMatrix = new Sprite [totalStates][][];
  		for (int i = 0; i < totalStates; ++i){
  			spriteMatrix[i] = new Sprite[directions[i]][];
	  		for (int j = 0; j < directions[i]; ++j){
				spriteMatrix[i][j] = new Sprite[stateLengths[i]];
  				for (int k = 0; k < stateLengths[i]; ++k){
  					spriteMatrix[i][j][k] = sprites[cont];
  					++cont;
  				}
  			}
  		}
  	}

  	//UPDATE FUNCTIONS!!!

	void Update () {
		if (canMove) UpdateCurrentState ();
		UpdateSprite();
	    if (canMove) UpdatePosition(); // Consider moving this to FixedUpdate
		UpdateCamera ();
		//UpdateStatus ();
	}


	public void UpdateSprite(){ //Updates the sprite in the renderer. Only works for plain surfaces!
		
		Vector3 camToPlayer = transform.position - cameraPosition.transform.position; //Cam --> Player
		Vector3 normalVector = Vector3.forward; //(0,0,1) [only for plain surfaces]
		camToPlayer.z = 0; // we project it to z = 0.

		float angle = Vector3.Angle(camToPlayer, direction); //shortest angle

		//Check the sprite direction depending on the angle and the total number of directions the sprite has.
		if (directions[currentState] == 5){
			if (angle < 22.5) spriteDirection = 4;
			else if (angle < 67.5) spriteDirection = 3;
			else if (angle < 112.5) spriteDirection = 2;
			else if (angle < 157.5) spriteDirection = 1;
			else spriteDirection = 0;
		}
		else if (directions[currentState] == 2){
			if (angle < 90) spriteDirection = 1;
			else spriteDirection = 0;
		}

		//Check if we have to apply symmetry!
		float sign = Vector3.Dot(normalVector, Vector3.Cross(camToPlayer, direction));
		if (sign < 0 && (directions[currentState] == 2 || (spriteDirection > 0 && spriteDirection < 4))) spritePosition.transform.localScale = new Vector3(-1,1,1);
		else spritePosition.transform.localScale = new Vector3(1,1,1);

		//Get the correct frame from the animator
		int j = (int) Mathf.Floor (spriteIndex);
		j %= stateLengths[currentState];

		//Put all together.
		spritePosition.setSprite(spriteMatrix[currentState][spriteDirection][j]);
	}


	void UpdatePosition(){

	  if (currentState > walking) return; //We have to be walking!

	  Vector3 newPosition;
	  //if we are close enough to our target or not
	  if ((this.transform.position - targetPosition).magnitude <= speed * Time.deltaTime) newPosition = targetPosition;
      else  newPosition = this.transform.position + direction * speed * Time.deltaTime;

      //finding obstacles (collisions).
      RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(newPosition)));
      bool collision = false;
      foreach (RaycastHit2D hit in hits) {
        if (hit.collider.gameObject.tag == "Obstacle") {
          collision = true;
          break;
        }
      }
      if (!collision) this.transform.position = newPosition; //Everything goes smoothly.

      //Stop if collision of close enough to target.
      if (collision || Vector3.Dot(targetPosition - this.transform.position, direction) < Mathf.Epsilon) {
			targetPosition = transform.position;
			SetCurrentState (walking, false);
      }
	}

	void UpdateCamera() { //Player always perpendicular to camera!
		if (this.GetComponent<Player>() != null) cameraPosition.FollowPlayer(transform.position);
	}

	void UpdateStatus() { //waitingForSkill, poison, sleep, etc.
		for (int i = 0; i < status.Length; ++i){
			if (status[i] > 0){
				status[i] -= Time.deltaTime;
				if (status[i] <= 0){ //if some status finishes
					status[i] = 0;
					if (i == waitingForSkill){
						skillManager.performSkill(this, currentSkill, false, -1, this.transform.position);
					}
				}
			}
		}
	}

	//STATUS FUNCTIONS!!


	void UpdateCurrentState() {
		if (isChasing) {
			if (enemy == null || enemy.isDead()) {
				// If the enemy disappears, cry.
				isChasing = false;
				targetPosition = transform.position;
				SetCurrentState (attacking, false);
				SetCurrentState (walking, false);
			} else if (IsObjectInRange (enemy.gameObject)) {
				// Close enough to attack.
				SetCurrentState(walking, false);
				SetCurrentState (attacking);
			} else if (currentState != attacking) {
				// Update our target position to our enemy.
				SetTargetPosition (enemy.transform.position);
			}
		}

		if (currentState == attacking && enemy != null){
			direction = enemy.transform.position - this.transform.position;
		}
		//Update state to walking if needed and update animator.
		if (HasTargetPosition()) SetCurrentState (walking);
		animator.SetInteger("currentState", currentState);
	}

	void SetCurrentState(int state, bool b = true) {
		/*if (b && status[waitingForSkill] > 0){ //if we interrupt a skill we send -1
			skillManager.performSkill(this, currentSkill, false, -1, this.transform.position); 
		}*/
		if (b) cancelAllSkills();
		if (state == currentState && state == casting){
			animator.SetTrigger("newCast");
			direction = targetSkill - this.transform.position;
		}
		if (!b && state == currentState){
			// If we stop what we are doing --> idle.
			if (state == walking) targetPosition = transform.position;
			currentState = idle;
		}
		if (b) {
			//Order: '>' except walking, > attacking and casting
			if (state > currentState || ((currentState == attacking || currentState == casting) && state == walking)) {
				currentState = state;
				if (state == casting){
					enemy = null;
					targetPosition = transform.position;
					isChasing = false;
				}
				if (state == casting && (targetSkill - this.transform.position).magnitude > Mathf.Epsilon){
					direction = targetSkill - this.transform.position;
				}
			}
		}
		animator.SetInteger("currentState", currentState); //There are functions called in the animator!
	}

	public void SetTargetPosition(Vector3 position) {
	  	if (position != targetPosition){ //We don't want to compute everything again
		    targetPosition = position;
		    direction = (targetPosition - this.transform.position);
		    direction.Normalize();
		}
	}

	public void setAttacking (int a){ //1 = attacking, 0 = not attacking
		SetCurrentState (attacking, a == 1);
	}

	public void setBeingHit (int a){ //1 = being hit, 0 = else
		SetCurrentState (beinghit, a == 1);
	}

	public void setCasting (int a){ //1 = being hit, 0 = else
		SetCurrentState (casting, a == 1);
	}

	public void setStatus (int stat, float t){ //set the status timer to t
		status[stat] = t; //ToDo maybe some stuff is revoked by other sources!
	}	

	public void finishCasting(){
		setCasting(0);
		skillManager.performSkill(this, currentSkill, true, 0, targetSkill);
	}

	public void performSkill (int skill, Vector3 target){ //Performing skill
		if (status[waitingForSkill] > 0 && skill != currentSkill) return;
		//Which skill and where
		currentSkill = skill;
		targetSkill = target;

		//Stop doing other stuff [same as casting]
		enemy = null;
		targetPosition = transform.position;
		isChasing = false;
		SetCurrentState(currentState, false);

		//Depending if we were waiting for the click or it was the first one
		if (status[waitingForSkill] == 0) skillManager.performSkill(this, currentSkill, false, 0, targetSkill);
		else skillManager.performSkill(this, currentSkill, false, 1, targetSkill); //can be done better
	}

	public void chaseEnemy(Character givenEnemy){
		isChasing = true;
		enemy = givenEnemy;
		SetTargetPosition(givenEnemy.transform.position);
	}

	public void stopChasing() {
		isChasing = false;
		targetPosition = this.transform.position;
		enemy = null;
	}

	public void isHit(){
		SetCurrentState (beinghit);
	}

	public void die(){
		SetCurrentState (dying);
	}

	public void selfDestruct(){
		Destroy(this.gameObject);
	}

	//Other functions

	public void autoAttack(){ //compute a value between minAttack and maxAttack, and tell the enemy that it is being hurt!
		Debug.Assert (enemy != null);
		Health health = enemy.gameObject.GetComponent<Health>();
		Debug.Assert (health != null);
		int extraAttack = (int) Mathf.Floor(Random.value * (float)(maxAttack - minAttack + 1));
		int attack = minAttack + extraAttack;
		health.decreaseHealth(attack, this);
	}

	bool HasTargetPosition() {
		return transform.position != targetPosition;
	}

	public bool IsIdle() {
		return currentState == idle;
	}

	public bool isDead(){
		return currentState == dying;
	}

	public bool IsObjectInRange(GameObject obj) {
		return (obj.transform.position - transform.position).magnitude <= 0.5f;
	}

	public void playClip(int a){
  		audioSource.clip = audioClips[a];
  		audioSource.Play();
  	}

  	private void cancelAllSkills(){
  		Skill[] S = GetComponentsInChildren<Skill>();
  		foreach (Skill skill in S){
  			if (skill.canCancel()){
  				skillManager.cancelSkill(skill);
  			}
  		}
  	}

  	public void moveTo (Vector3 v){
  		bool b = HasTargetPosition();
  		this.transform.position = v;
  		this.targetPosition = this.transform.position;
  		UpdateCamera();
  	}

  	public void setMove(bool b){
  		canMove = b;
  	}

}