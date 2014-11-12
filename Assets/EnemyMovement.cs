using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public GameObject target;
	float speed = 0.07f;
	bool isFollowing = false;
	float fRange = 25f;// follow range
	float aRange = 2.25f;//attack range
	float outRange = 29f;//out of distance from original position range
	float inRange = 10f;//in range of aggro
	float dmg_time = 11; //time since taken damage
	Vector3 pos;
	// Use this for initialization
	void Start () {
		pos = transform.position;
	}

	// Update is called once per frame
	void Update () {

		Vector3 direction = target.transform.position - transform.position;
		Vector3 direction2 = pos - transform.position;
		if ((isFollowing && direction.magnitude <= fRange) || (direction.magnitude <= inRange
		     && direction2.magnitude <= outRange)) {
			isFollowing = true;
			if( direction.magnitude >= aRange)
			{
				direction.Normalize ();
				direction *= speed;
				transform.position += direction;
			}
			else if( direction.magnitude < aRange)
			{
				//attack
				isFollowing = false;
			}
		}
		else{
			isFollowing = false;
			if( direction2.magnitude > 1 ){

				direction2.Normalize ();
				direction2 *= speed;
				transform.position += direction2;
			}
		}

	}
	
}
