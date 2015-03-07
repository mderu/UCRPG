using UnityEngine;
using System.Collections;

public class RangedMovement : EnemyMovement {

	// AI states~!
	protected enum stateEnemyAI { init, patrol, idle, aggro, attack, kite, idlePause, returnToPost };
	protected stateEnemyAI EnemyAI = stateEnemyAI.init;
	public float attackRange = 15;

	public float minAttackRange = 5;
	public float maxAttackRange = 10;

	void Start () 
	{
		enemyPatrolRadius = Random.Range(enemyPatrolRadiusLowerBound, enemyPatrolRadiusUpperBound);
		enemyInitialSpawn = transform.position;
		enemyPatrolNext.Set (0, 0, 0);
		enemyDirection.Set (0, 0, 0);
	}

	void enemyAIStateMachine()
	{
		switch (EnemyAI)
		{
			case stateEnemyAI.init:
			{
				EnemyAI = stateEnemyAI.idle;
				Debug.Log("Enemy Patrol Radius: " + enemyPatrolRadius);
				Debug.Log("Going to Idle.");
				break;
			}

			case stateEnemyAI.idle:
			{
				idleState();
				break;
			}

			case stateEnemyAI.patrol:
			{	
				patrolState();
				break;
			}

			case stateEnemyAI.aggro:		// might want to make this same as kite ...t.t.t...
			{
				aggroState();
				break;
			}

			case stateEnemyAI.attack:		// kites away from player and tries to attack
			{
				attackState();
				break;
			}
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
			case stateEnemyAI.returnToPost:
			{
				break;
			}

			default:
				break;
		}
	}



	//===========================================~~~ Lazy Inheritance Classes 2! ~~~===========================================//
	//========================================~~~ Lazier & Cooler than Ever Before! ~~~========================================//


	void aggroState()
	{
		Debug.Log ("Aggro State");
		if (!AIWithinRange(enemyPatrolRadius))
			//if not in range then go back to idle
			EnemyAI = stateEnemyAI.idlePause;
		else
		{
			if (!AIWithinRange(maxAttackRange))			// Enemy too far away to attack, move closer
			{
				Debug.Log ("Moving Closer");
				MoveTowards(target.transform.position);
			}
			else if (AIWithinRange(minAttackRange))		// Enemy is close enough to attack
			{
				// if in attack range, switch to attack state
				EnemyAI = stateEnemyAI.attack;
			}
			else if (PersonWithinRange( minAttackRange ))	// Player too close!
			{
				Debug.Log ("Moving Farther");
				MoveAway();
			}
		}
	}

	void attackState()
	{
		Debug.Log ("Attack State");
		if (!AIWithinRange(enemyPatrolRadius))
			//if not in range then go back to idle
			EnemyAI = stateEnemyAI.idlePause;
		else
		{
			if (PersonWithinRange( minAttackRange ))	// Player too close! Fly you fools! ...actually, the if else might need to swap
			{
				Debug.Log(AIWithinRange( minAttackRange ));
				Debug.Log ("Attack going to Aggro!");
				// fancy math in hurrrrr
				EnemyAI = stateEnemyAI.aggro;
				MoveAway();
			}
			else
			{
				Debug.Log("Attack State! Grrrr");
				// todo: implement attack animation, damage calc, etc
				// more math, more fun!. 
				//MoveAway(); // Just to do something for now
				idleTimer = 0;
				while(idleTimer <= 2)
				{
					Debug.Log ("Pretend I'm totally attacking /s");
					idleTimer++;
				}
				idleTimer = 0;
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
}
