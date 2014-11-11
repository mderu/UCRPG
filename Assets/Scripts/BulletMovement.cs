using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {

	float speed = 30f;
	float yAngle;
	int damage = 3;
	int targetLayer = Layers.Enemies;
	Vector3 initPos;
	
	public Transform target;
	public bool followTarget = true;

	Vector3 lastTargPos = Vector3.zero;

	float speedTimesFrames = 0;

	// Use this for initialization
	void Start () {
		initPos = transform.position;
		calculateTrajectory ();
	}

	void calculateTrajectory(){
		Vector3 direction = target.position - transform.position;
		//For help on understanding this equation see the wiki page:
		//http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
		
		float yAngle = Mathf.Atan ((Mathf.Pow (speed, 2) - Mathf.Sqrt (Mathf.Pow (speed, 4) - 
		                                                               -Physics.gravity.y * (
			-Physics.gravity.y * (direction.x * direction.x + direction.z * direction.z) + 2 * direction.y * speed * speed)
		                                                               )) / 
		                           (-Physics.gravity.y * Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z))) * 180.0f / Mathf.PI;
		float xAngle = Mathf.Atan (direction.x / direction.z) * 180.0f / Mathf.PI;
		float zAngle = 0;
		if (float.IsNaN (yAngle)) {yAngle = 45f;}
		rigidbody.velocity = Quaternion.Euler (-yAngle, xAngle, zAngle) * Vector3.forward * speed;

		lastTargPos = target.position;
	}
	
	void calculateTrajectory(float iSpeed){
		calculateTrajectory (iSpeed, iSpeed * iSpeed);
	}
	void calculateTrajectory(float iSpeed, float iSpeedSquared){
		Vector3 direction = target.position - transform.position;
		//For help on understanding this equation see the wiki page:
		//http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
		
		float yAngle = Mathf.Atan ((iSpeedSquared - Mathf.Sqrt (iSpeedSquared * iSpeedSquared - 
		                                                               -Physics.gravity.y * (
			-Physics.gravity.y * (direction.x * direction.x + direction.z * direction.z) + 2 * direction.y * iSpeedSquared)
		                                                               )) / 
		                           (-Physics.gravity.y * Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z))) * 180.0f / Mathf.PI;
		float xAngle = Mathf.Atan (direction.x / direction.z) * 180.0f / Mathf.PI;
		if (target.position.z < transform.position.z) {xAngle += 180;}
		float zAngle = 0;
		if (float.IsNaN (yAngle)) {yAngle = 45f;}
		if (float.IsNaN (xAngle)) {xAngle = 0;}
		rigidbody.velocity = Quaternion.Euler (-yAngle, xAngle, zAngle) * Vector3.forward * iSpeed;
		
		lastTargPos = target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 direction = target.position - transform.position;
		transform.rotation = Quaternion.LookRotation (rigidbody.velocity);

		if (followTarget) {
			calculateTrajectory (rigidbody.velocity.magnitude, rigidbody.velocity.sqrMagnitude);
		}
			//if (followTarget) {transform.position += (target.position - lastTargPos);lastTargPos = target.position;}

		RaycastHit rch;
		if (Physics.Raycast (transform.position, rigidbody.velocity, out rch, rigidbody.velocity.magnitude*Time.fixedDeltaTime, targetLayer)){
			RaycastHit rch2;
			if (Physics.Raycast (transform.position, rigidbody.velocity, out rch2, rigidbody.velocity.magnitude*Time.fixedDeltaTime, ~targetLayer)){
				if(rch2.distance > rch.distance){target.GetComponent<EnemyAI>().TakeDamage(3);}
			}else{
				target.GetComponent<EnemyAI>().TakeDamage(3);
			}
			killBullet();
		}
		if (Physics.Raycast (transform.position, rigidbody.velocity, out rch, rigidbody.velocity.magnitude*Time.fixedDeltaTime, ~targetLayer)){
			killBullet();
		}
		
		speedTimesFrames += speed * Time.fixedDeltaTime;
		if(speedTimesFrames > 200){
			killBullet();
		}
	}

	//For now, kill bullet just resets it's location
	void killBullet(){
		speedTimesFrames = 0;
		transform.position = initPos;
		rigidbody.velocity = Vector3.zero;
		calculateTrajectory();
	}

	void OnTriggerEnter(Collider other)
	{
		if ( (other.gameObject.layer & targetLayer) != 0) {
			other.transform.GetComponent<EnemyAI>().TakeDamage(3);
		}
		killBullet();
	}
}
