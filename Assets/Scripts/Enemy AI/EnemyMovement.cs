using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour 
{
	public GameObject target;
	
	//------John's Variables----------------------------------------------
    float enemyPatrolRadius;
	float enemyPatrolPoint = 0;
	Vector3 enemyPatrolNext;
	Vector3 enemyDirection;
	Vector3 enemyInitialSpawn;
	float enemySpeed = 5f;
	float enemyX = 0;
	float enemyZ = 0;
	public float enemyPatrolRadiusUpperBound = 15;
	public float enemyPatrolRadiusLowerBound = 20;
	public float enemyAggroRadius = 4;
	public float enemyAttackRadius = 1;
	public float enemyFieldOfVision = 130f; // Somewhat arbitrary, avg effictive human fov between 110~140

	//--------------------------------------------------------------------
		
	void Start () 
    {
        enemyPatrolRadius = Random.Range(enemyPatrolRadiusLowerBound, enemyPatrolRadiusUpperBound);
		enemyInitialSpawn = transform.position;
		enemyPatrolNext.Set (0, 0, 0);
		enemyDirection.Set (0, 0, 0);
	}

	//========================================================================
	//---------------------Enemy AI State Machine----------------------------
	//========================================================================

	//----------------Variables and States-------------------
	float idleTimer = 0;
	public enum stateEnemyAI {init, patrol, idle, aggro, attack, idlePause, returnToPost};
	private stateEnemyAI EnemyAI = stateEnemyAI.init;
	//-------------------------------------------------------

	//=================-------State Machine------===================
	void enemyAIStateMachine() 
    {
        //========State Machine==========
		switch (EnemyAI) 
        {

		    //========Initial state==========
		    case stateEnemyAI.init:
            {
                EnemyAI = stateEnemyAI.idle;
                Debug.Log("Enemy Patrol Radius: " + enemyPatrolRadius);
                Debug.Log("Going to Idle.");
                break;
            }

		    //========Idle State===========
            case stateEnemyAI.idle:
            {
				idleState();
                break;
            }
            
		    //=========Patrol State=========
            case stateEnemyAI.patrol:
            {
				patrolState();
                break;
            }
            
		    //=========aggro State==========
            case stateEnemyAI.aggro:
            {
				aggroState();
                break;
            }
            
		    //==========Attack State==========
            case stateEnemyAI.attack://TODO: Finish attack state
            {
				attackState();
                break;
            }

		//=========Idle Pause==============
            case stateEnemyAI.idlePause:
            {
                if (idleTimer >= 2)
                {
                    EnemyAI = stateEnemyAI.idle;
                    idleTimer = 0;
                }
                else
                {
                    idleTimer += Time.deltaTime; //update timer
                }
                break;
            }

		    //============Return to Post State=============
            case stateEnemyAI.returnToPost:	//TODO: Finish return to post
            {
                break;
            }

		    default:
			    break;	
		}
	}

	//===========================================~~~ Lazy Inheritance Classes ~~~===========================================//

	bool PersonWithinRange(float Range) // Player within AI aggro range
	{
		return (Mathf.Pow((target.transform.position.x - transform.position.x), 2) +
		        Mathf.Pow((target.transform.position.y - transform.position.y), 2)) <= Mathf.Pow(Range, 2);
	}

	bool AIWithinRange(float Range) // AI within radius of AI range
	{
		return (Mathf.Pow((transform.position.x - enemyInitialSpawn.x), 2) +
		        Mathf.Pow((transform.position.y - enemyInitialSpawn.y), 2)) <= Mathf.Pow(Range, 2);
	}

	bool WithinAIVision() // Player within AI Field of Vision
	{
		Vector3 enemyDist = target.transform.position - transform.position; // Distance between player and AI
		float posAngle = Vector3.Angle (enemyDist, transform.forward); // angle from forward view and player
		
		if (posAngle < (enemyFieldOfVision / 2.0f)) // if true, player potentially in range
		{
			RaycastHit pewpew;
			if(Physics.Raycast(transform.position, enemyDist.normalized, out pewpew))
				if( pewpew.collider.gameObject.layer == Layers.Player ) // get lucky!
					return true;
			return false;
		}
		return false; 
	}

	void idleState()
	{
		if (PersonWithinRange(enemyAggroRadius) && WithinAIVision())
		{
			Debug.Log("Player spotted in idle state!!!");
			EnemyAI = stateEnemyAI.aggro;
		}
		else
		{
			if (idleTimer >= 1)
			{	//then calculate next patrol point and switch states
				EnemyAI = stateEnemyAI.patrol; //switch states
				enemyX = enemyInitialSpawn.x + Random.insideUnitCircle.x * enemyPatrolRadius;
				enemyZ = enemyInitialSpawn.z + Random.insideUnitCircle.y * enemyPatrolRadius;
				enemyPatrolNext.Set(enemyX, 0, enemyZ);
				idleTimer = 0;
				Debug.Log("Going to Patrol.");
			}
			else
			{
				idleTimer += Time.deltaTime; //update timer
			}
		}
	}

	void attackState()
	{
		if (!AIWithinRange(enemyPatrolRadius))
			//if not in range then go back to idle
			EnemyAI = stateEnemyAI.idlePause;

		Debug.Log(AIWithinRange(enemyAttackRadius));
		Debug.Log("Back to aggro!");
		MoveTowards(target.transform.position); 
		EnemyAI = stateEnemyAI.aggro; // A little lazy, probably potential problems later on.
	}

	void aggroState()
	{
		if (!AIWithinRange(enemyPatrolRadius))
			//if not in range then go back to idle
			EnemyAI = stateEnemyAI.idlePause;
		else
		{
			if (AIWithinRange(enemyAttackRadius))
			{
				// if in attack range, switch to attack state
				EnemyAI = stateEnemyAI.attack;
			}
			else
			{
				//follow the player
				MoveTowards(target.transform.position);
			}
		}
	}

	void patrolState()
	{
		if (PersonWithinRange(enemyAggroRadius) && WithinAIVision())
		{
			Debug.Log("Player spotted in patrol state!!!");
			EnemyAI = stateEnemyAI.aggro;
		}
		else
		{
			if ((transform.position - enemyPatrolNext).sqrMagnitude < enemySpeed)
			{
				EnemyAI = stateEnemyAI.idle; //switch states
				Debug.Log("Going to Idle.");
			}
			else
			{
				//enemy movement action
				MoveTowards(enemyPatrolNext);
			}
		}
	}
	//==========================================================================================
	//--------------------------------------UPDATE FUNCTION-------------------------------------
	//==========================================================================================
	// Update is called once per frame
	void Update () 
    {
		enemyAIStateMachine();
	}

    void MoveTowards(Vector3 destination)
    {
        transform.GetComponent<NavMeshAgent>().SetDestination(destination);
    }

	void MoveAway() // #YOLO
	{
		// Creates a vector that is in the opposite direction of the player
		Vector3 newPos = (transform.position - target.transform.position).normalized * enemyPatrolRadius + target.transform.position;
		transform.GetComponent<NavMeshAgent>().SetDestination (newPos);
	}
}

