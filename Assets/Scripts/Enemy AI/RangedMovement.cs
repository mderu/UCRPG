using UnityEngine;
using System.Collections;

public class RangedMovement : EnemyMovement {

	// AI states~!
	public enum stateEnemyAI { init, patrol, idle, aggro, attack, kite, idlePause, returnToPost };
	public float attackRange = 15;

	public float enemyMinAttackRange = 5;
	public float enemyMaxAttackRange = 10;

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

			case stateEnemyAI.attack:
			{
				attackState();
				break;
			}
			case stateEnemyAI.kite:{}
			case stateEnemyAI.idlePause:{}
			case stateEnemyAI.returnToPost:{}

			default:
				break;
		}
	}



	//===========================================~~~ Lazy Inheritance Classes 2! ~~~===========================================//
	//========================================~~~ Lazier & Cooler than Ever Before! ~~~========================================//


	void aggroState()
	{

		if (!AIWithinRange(enemyPatrolRadius))
			//if not in range then go back to idle
			EnemyAI = stateEnemyAI.idlePause;
		else
		{
			if (!AIWithinRange(enemyMaxAttackRange))			// Enemy too far away to attack, move closer
			{
				Debug.Log ("Moving Closer");
				MoveTowards(target.transform.position);
			}
			else if (AIWithinRange(enemyMinAttackRadius))		// Enemy is close enough to attack
			{
				// if in attack range, switch to attack state
				EnemyAI = stateEnemyAI.attack;
			}
			else if (PersonWIthinRange( enemyMinAttackRange ))	// Player too close!
			{
				MoveAway();
			}
		}
	}

	void attackState()
	{

		if (!AIWithinRange(enemyPatrolRadius))
			//if not in range then go back to idle
			EnemyAI = stateEnemyAI.idlePause;
		else
		{
			if (PersonWIthinRange( enemyMinAttackRange ))	// Player too close! Fly you fools! ...actually, the if else might need to swap
			{
				Debug.Log(AIWIthinRange( enemyMinAttackRange ));
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
				while(idleTimer <= 3)
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
