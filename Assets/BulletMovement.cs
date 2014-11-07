using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {

	float speed = 20f;
	float yAngle;
	int damage = 3;
	public Transform target;
	Vector3 initTargetPos;
	Vector3 initPos;


	// Use this for initialization
	void Start () {
		initPos = transform.position;
		initTargetPos = target.position;
		//rigidbody.AddForce (Vector3.up*15, ForceMode.VelocityChange);
		Vector3 direction = target.position - transform.position;
		float yAngle = (-Physics.gravity.y/(speed*speed) > 1) ? 45f : 
			(Mathf.Asin (-Physics.gravity.y*Mathf.Sqrt(direction.x*direction.x+direction.z*direction.z)/(speed*speed))/2f * 180.0f / Mathf.PI);

		float xAngle = Mathf.Atan (direction.x / direction.z) * 180.0f / Mathf.PI;
		float zAngle = 0;
		Debug.Log (new Vector3(xAngle, yAngle, zAngle));
		Debug.Log (Quaternion.Euler (yAngle, xAngle, zAngle) * Vector3.forward * speed);
		rigidbody.AddForce(Quaternion.Euler (-yAngle, xAngle, zAngle) * Vector3.forward * speed, ForceMode.VelocityChange);

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target.position - transform.position;
		transform.rotation = Quaternion.LookRotation (rigidbody.velocity);
/*
		float xSpeed = Mathf.Sqrt (((speed * speed) - (ySpeed * ySpeed)) / (1.0f + (direction.z * direction.z) / (direction.x * direction.x)));
		float zSpeed = xSpeed/(direction.x / direction.z);
		Debug.Log ("xSpeed: " + xSpeed);
		rigidbody.AddForce(new Vector3(xSpeed, rigidbody.velocity.y, zSpeed) - rigidbody.velocity, ForceMode.VelocityChange);
*/
		/*transform.rotation = Quaternion.LookRotation (direction, Vector3.up);*/
		if (direction.magnitude > speed/120f) {
			//transform.Translate (direction.normalized * speed);
		}else{
			target.GetComponent<EnemyAI>().TakeDamage(3);
			//Destroy (transform);
			transform.position = initPos;
			initTargetPos = target.position;
		}
	}

}
