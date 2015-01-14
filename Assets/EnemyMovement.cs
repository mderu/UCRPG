using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public GameObject target;
	
	//------John's Variables----------------------------------------------
    float enemyPatrolRadius;
	float enemyPatrolPoint = 0;
	Vector3 enemyPatrolNext;
	Vector3 enemyDirection;
	Vector3 enemyInitialSpawn;
	float enemySpeed = 5f;
	//--------------------------------------------------------------------
	
	//Raymond's Variables------------------------------------
	//float timer = 0;	//time
	//float speed = 0.01f;	//speed of the mob
	//bool isFollowing = false;
	//float fRange = 25f;// follow range
	//float aRange = 2.25f;//attack range
	//float outRange = 29f;//out of distance from original position range
	//float inRange = 10f;//in range of aggro
	//float dmg_time = 11; //time since taken damage
	//Vector3 pos;
	//--------------------------------------------------------
	// Use this for initialization
	
	//enum State{Idle, Attack, AttackAnim, };
	
	//State aiState;
	
	void Start () {
        enemyPatrolRadius = Random.Range(15, 25);
		enemyInitialSpawn = transform.position;
		enemyPatrolNext.Set (0, 0, 0);
		enemyDirection.Set (0, 0, 0);
	}
	
	//---------------------Enemy AI State Machine----------------------------
	float idleTimer = 0;
	public enum stateEnemyAI {init, patrol, idle, agro, attack};
	private stateEnemyAI EnemyAI = stateEnemyAI.init;
	
	void enemyAIStateMachine() {
		switch (EnemyAI) { //Transitions
			
		case stateEnemyAI.init:
			EnemyAI = stateEnemyAI.idle;
			break;
			
		case stateEnemyAI.idle:	//idle state
			if (idleTimer >= 2) {
				EnemyAI = stateEnemyAI.patrol; //switch states
				enemyPatrolNext.Set (enemyInitialSpawn.x + (Mathf.Pow (-1, Random.Range (1, 3))) * (Random.Range (5, enemyPatrolRadius)),
                                     0, enemyInitialSpawn.y + (Mathf.Pow(-1, Random.Range(1, 3))) * (Random.Range(5, enemyPatrolRadius)));
				idleTimer = 0;
			}
			break;
			
		case stateEnemyAI.patrol:   //patrol
			if ((transform.position - enemyPatrolNext).sqrMagnitude < enemySpeed) {
				EnemyAI = stateEnemyAI.idle; //switch states
			} else {
				//enemy movement action
				enemyDirection = enemyPatrolNext - transform.position;
				enemyDirection.Normalize ();
				enemyDirection *= (enemySpeed * Time.deltaTime);
				transform.position += enemyDirection;
			}
			break;
			
		case stateEnemyAI.agro:	//agro
			
			break;
			
		case stateEnemyAI.attack:
			
			break;
			
		default:
			break;	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		enemyAIStateMachine();
		idleTimer += Time.deltaTime ; 
		
		//gets the distance between the position of the cube and the position fo the player
		//Vector3 direction = target.transform.position - transform.position;
		//direction.y = 0;
		
		//gets the distance of how far away the cube is from the initial "spawn point"
		//Vector3 direction2 = pos - transform.position;
		
		//if player is within range then cube will start following player
		//if ((isFollowing && direction.magnitude <= fRange) || (direction.magnitude <= inRange
		//     && direction2.magnitude <= outRange)) {
		//	isFollowing = true;
		
		//    if (direction.magnitude >= aRange) {	//keep going towards player until in range for attack
		
		//		direction.Normalize ();
		//		direction *= speed;
		//		transform.position += direction;
		//	}
		//	else if( direction.magnitude < aRange) {//if player is within range, then attack
		//attack
		//		isFollowing = false;
		//	}
		// }
		// else {
		//	isFollowing = false;
		
		//	if( direction2.magnitude > 1 ) {//if not following then head back to initial "spawn point"
		//		direction2.Normalize ();
		//		direction2 *= speed;
		//		transform.position += direction2;
		//	}
	}
	
}

