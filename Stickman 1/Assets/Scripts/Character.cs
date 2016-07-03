using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

  //Input given
  public string myFolder;
  public float speed;
  public int[] directions;
  public int[] stateLengths;
  public int totalStates;
  public float speedMult;

  //Movement stuff
  private Vector3 direction;
  private Vector3 targetPosition;
  private int dir;

  //Sprite stuff
  private Sprite[] sprites;
  private Sprite[][][] spriteMatrix;
  public float spriteIndex;
  private int spriteDirection;

  //Status
  private bool[] statusArray;

  //States
  private const int idle = 0;
  private const int walking = 1;
  private const int casting = 2;
  private const int beinghit = 3;
  private const int attacking = 4;
  private const int dying = 5;
  public int currentState;

  //State conditioners
  private bool isChasing;
  private Character enemy;
  private Vector3 targetSkill;
  private int currentSkill;
  private bool isMoving;
  private bool canMove; //not being moved by an exterior source


  //Autoattacks
  public int minAttack = 7;
  public int maxAttack = 10;


  //Other components
  private Animator animator;
  private CameraPosition cameraPosition;
  private AudioSource audioSource;
  private AudioClip[] audioClips;
  private SkillManager skillManager;
  private BodyRenderer bodyRenderer;
  private CastBar castBar;



	//START FUNCTIONS!!

	void Start () {
		initializeVariables();
		createSpriteMatrix();
	}

	void initializeVariables(){

		//Get components
		animator = this.GetComponent<Animator>();
	    cameraPosition = GameObject.FindObjectOfType<CameraPosition>();
		audioSource = this.GetComponent<AudioSource>();
		skillManager = GameObject.FindObjectOfType<SkillManager>();
		bodyRenderer = this.GetComponentInChildren<BodyRenderer>();

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
	    isMoving = false;
	    canMove = true;

	    //Status
	    statusArray = new bool[Constants.statusNumber];
	   	speedMult = 1;
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
		UpdateCurrentState ();
		UpdateSprite();
	    UpdatePosition(); // Consider moving this to FixedUpdate
		//UpdateCamera ();
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
		if (sign < 0 && (directions[currentState] == 2 || (spriteDirection > 0 && spriteDirection < 4))){
			bodyRenderer.transform.localScale = new Vector3(-1,1,1);
		}
		else{ 
			bodyRenderer.transform.localScale = new Vector3(1,1,1);
		}

		//Get the correct frame from the animator
		int j = (int) Mathf.Floor (spriteIndex);
		j %= stateLengths[currentState];

		//Put all together.
		bodyRenderer.setSprite(spriteMatrix[currentState][spriteDirection][j]);
	}


	void UpdatePosition(){

	  float s = speed*speedMult;

	  if (!canMove) return;

	  if (currentState > walking || !isMoving) return; //We have to be walking!

	  Vector3 newPosition;
	  //if we are close enough to our target or not
	  if ((this.transform.position - targetPosition).magnitude <= s * Time.deltaTime) newPosition = targetPosition;
      else  newPosition = this.transform.position + direction * s * Time.deltaTime;

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
			isMoving = false;
			SetCurrentState (walking, false);
      }
	}

	void UpdateCamera() { //Player always perpendicular to camera!
		if (this.GetComponent<Player>() != null) cameraPosition.FollowPlayer(transform.position);
	}

	public void UpdateStatus() { //waitingForSkill, poison, sleep, etc.
		speedMult = 1;
		float aux = 0;
		for (int i = 0; i < statusArray.Length; ++i) statusArray[i] = false;
		Status[] status = this.GetComponentsInChildren<Status>();
		foreach (Status st in status){
			if (!st.isDestroyed()){
				statusArray[st.getStatus()] = true;
				if (st.getStatus() == Constants.crippled){
					aux = Mathf.Max(aux, st.getParameter(Constants.effectiveness));
				}
			}
		}
		speedMult -= aux;
		bodyRenderer.setInvisible(statusArray[Constants.invisible]);
		canMove = !statusArray[Constants.controlled];
		if (statusArray[Constants.quake]) cameraPosition.setQuake(true);
		else cameraPosition.setQuake(false);
	}

	//STATUS FUNCTIONS!!


	void UpdateCurrentState() {
		if (!canMove) return;
		if (isChasing) {
			if (enemy == null || enemy.isDead()) {
				// If the enemy disappears, cry.
				isChasing = false;
				isMoving = false;
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
		if (!canMove) return;
		if (b) cancelAllSkills();
		if (currentState == dying) return;
		if (state == currentState && state == casting){
			animator.SetTrigger("newCast");
			direction = targetSkill - this.transform.position;
		}
		if (!b && state == currentState){
			// If we stop what we are doing --> idle.
			if (state == walking) isMoving = false;
			currentState = idle;
		}
		if (b) {
			//Order: '>' except walking > attacking and casting
			if (state > currentState || ((currentState == attacking || currentState == casting) && state == walking)) {
				currentState = state;
				if (state == casting){
					enemy = null;
					isMoving = false;
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
		//if (!canMove) return;
	  	if (position != targetPosition){ //We don't want to compute everything again
		    targetPosition = position;
		    direction = (targetPosition - this.transform.position);
		    direction.Normalize();
		    isMoving = true;
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

	public void finishCasting(){
		castAllSkills();
		setCasting(0);
		//skillManager.performSkill(this, currentSkill, true, 0, targetSkill);
	}

	public void performSkill (int skill, Vector3 target, Character character){ //Performing skill
		if (!canMove) return;

		//Which skill and where
		currentSkill = skill;
		targetSkill = target;

		//Stop doing other stuff [same as casting]
		stopChasing();

		//Depending if we were waiting for the click or it was the first one
		skillManager.performSkill(this, currentSkill, targetSkill, character);
	}

	public void chaseEnemy(Character givenEnemy){
		//if (!canMove) return;
		isChasing = true;
		enemy = givenEnemy;
		SetTargetPosition(givenEnemy.transform.position);
	}

	public void stopChasing() {
		SetCurrentState(currentState, false);
		isChasing = false;
		isMoving = false;
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
		health.decreaseHealth(attack, this, Constants.physical, Constants.neutral);
	}

	bool HasTargetPosition() {
		return isMoving;
	}

	public bool IsIdle() {
		return currentState == idle;
	}

	public bool isDead(){
		return currentState == dying;
	}

	public bool isChasingNow(){
		return isChasing;
	}

	public bool IsObjectInRange(GameObject obj) {
		return (obj.transform.position - transform.position).magnitude <= 0.5f;
	}

	public void playClip(int a){
  		audioSource.clip = audioClips[a];
  		audioSource.Play();
  	}

  	public void cancelAllSkills(){
  		Skill[] S = GetComponentsInChildren<Skill>();
  		foreach (Skill skill in S){
  			if (skill.canCancel()){
  				skillManager.cancelSkill(skill);
  			}
  		}
  	}

  	private void castAllSkills(){
		Skill[] S = GetComponentsInChildren<Skill>();
  		foreach (Skill skill in S){
  			if (skill.isCasting()){
  				skillManager.finishSkill(skill);
  			}
  		}
  	}

  	public void moveTo (Vector3 v){
  		this.transform.position = v;
  		UpdateCamera();
  	}

  	public bool getStatus (int a){
  		return statusArray[a];
  	}

  	public bool canPerformSkill(){
  		if (currentState != casting && currentState != beinghit) return true;
  		return false;
  	}

}