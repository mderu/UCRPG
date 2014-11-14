using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public GameObject target;
	float speed = 0.07f;	//speed of the mob
	bool isFollowing = false;
	float fRange = 25f;// follow range
	float aRange = 2.25f;//attack range
	float outRange = 29f;//out of distance from original position range
	float inRange = 10f;//in range of aggro
	float dmg_time = 11; //time since taken damage
	Vector3 pos;
	// Use this for initialization
	void Start () {
		pos = transform.position;	//initial starting position of mob
	}

	// Update is called once per frame
	void Update () {

		//gets the distance between the position of the cube and the position fo the player
		Vector3 direction = target.transform.position - transform.position;

		//gets the distance of how far away the cube is from the initial "spawn point"
		Vector3 direction2 = pos - transform.position;

		//if player is within range then cube will start following player
		if ((isFollowing && direction.magnitude <= fRange) || (direction.magnitude <= inRange
		     && direction2.magnitude <= outRange)) 
		{
			isFollowing = true;

			if( direction.magnitude >= aRange)	//keep going towards player until in range for attack
			{
				direction.Normalize ();
				direction *= speed;
				transform.position += direction;
			}
			else if( direction.magnitude < aRange)	//if player is within range, then attack
			{
				//attack
				isFollowing = false;
			}
		}
		else
		{
			isFollowing = false;

			if( direction2.magnitude > 1 )	//if not following then head back to initial "spawn point"
			{
				direction2.Normalize ();
				direction2 *= speed;
				transform.position += direction2;
			}
		}

	}
	
}
