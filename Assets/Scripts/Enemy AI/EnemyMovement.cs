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
	public float enemyPatrolRadiusUpperBound = 10;
	public float enemyPatrolRadiusLowerBound = 15;
	public float enemyAggroRadius = 3;

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
	public enum stateEnemyAI {init, patrol, idle, agro, attack, idlePause, returnToPost};
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
                if (PersonWithinRange(enemyAggroRadius))
                {
                    EnemyAI = stateEnemyAI.agro;
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
                break;
            }
            
		    //=========Patrol State=========
            case stateEnemyAI.patrol:
            {
                if (PersonWithinRange(enemyAggroRadius))
                {
                    EnemyAI = stateEnemyAI.agro;
                    Debug.Log("Going to Agro");
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
                break;
            }
            
		    //=========Agro State==========
            case stateEnemyAI.agro:
            {
                if (!AIWithinRange(enemyPatrolRadius))
                    //if not in range then go back to idle
                    EnemyAI = stateEnemyAI.idlePause;
                else
                {
                    //follow the player
                    MoveTowards(target.transform.position);
                }

                break;
            }
            
		    //==========Attack State==========
            case stateEnemyAI.attack://TODO: Finish attack state
            {
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
	//=======================================================================================

	bool PersonWithinRange(float Range) // Circle equatioooon!
	{
		return (Mathf.Pow((target.transform.position.x - transform.position.x), 2) +
		        Mathf.Pow((target.transform.position.y - transform.position.y), 2)) <= Mathf.Pow(Range, 2);
	}

	bool AIWithinRange(float Range) // Circle equatioooon!
	{
		return (Mathf.Pow((transform.position.x - enemyInitialSpawn.x), 2) +
		        Mathf.Pow((transform.position.y - enemyInitialSpawn.y), 2)) <= Mathf.Pow(Range, 2);
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
        /*enemyDirection = destination - transform.position;
        enemyDirection.Normalize();
        enemyDirection *= enemySpeed;
        rigidbody.velocity = new Vector3(enemyDirection.x, rigidbody.velocity.y, enemyDirection.z);
        */
    }
}

