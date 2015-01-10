using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    float initSpeed = 0;
	public float speed = 25f;
    public bool highArc = false;

    //Change in accleration

    public float jerk = 0;
    float accel = 0f;
    //Random Z value for speed of rotation of the projectile
    float zRot = 0;

	Vector3 initPos;
	
	public Vector3 targetPosition;
    public GameObject target;
    public GameObject from;

	Vector3 lastTargPos = Vector3.zero;

	float totalDist = 0;

	// Use this for initialization
	void Start () {
        initSpeed = speed;
		initPos = transform.position;
		calculateTrajectory (speed, speed*speed);
        zRot = Random.value;
	}

	void calculateTrajectory(){
        Vector3 direction = targetPosition - transform.position;
		//For help on understanding this equation see the wiki page:
		//http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29

        float yAngle = Mathf.Atan((Mathf.Pow(speed, 2) - Mathf.Sqrt(Mathf.Pow(speed, 4) - 
		                                                               -Physics.gravity.y * (
			-Physics.gravity.y * (direction.x * direction.x + direction.z * direction.z) + 2 * direction.y * speed * speed)
		                                                               )) / 
		                           (-Physics.gravity.y * Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z))) * 180.0f / Mathf.PI;
        if (highArc) { yAngle = 90 - yAngle; }
        float xAngle = Mathf.Atan (direction.x / direction.z) * 180.0f / Mathf.PI;
		float zAngle = 0;
		if (float.IsNaN (yAngle)) {yAngle = 45f;}
		rigidbody.velocity = Quaternion.Euler (-yAngle, xAngle, zAngle) * Vector3.forward * speed;

		lastTargPos = targetPosition;
	}
	
	void calculateTrajectory(float iSpeed){
		calculateTrajectory (iSpeed, iSpeed * iSpeed);
	}
	void calculateTrajectory(float iSpeed, float iSpeedSquared){
		Vector3 direction = targetPosition - transform.position;
		//For help on understanding this equation see the wiki page:
		//http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
		
		float yAngle = Mathf.Atan ((iSpeedSquared - Mathf.Sqrt (iSpeedSquared * iSpeedSquared - 
		                                                               -Physics.gravity.y * (
			-Physics.gravity.y * (direction.x * direction.x + direction.z * direction.z) + 2 * direction.y * iSpeedSquared)
		                                                               )) / 
		                           (-Physics.gravity.y * Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z))) * 180.0f / Mathf.PI;
        if (highArc) { yAngle = 90 - yAngle; }
        float xAngle = Mathf.Atan (direction.x / direction.z) * 180.0f / Mathf.PI;
		if (targetPosition.z < transform.position.z) {xAngle += 180;}
		float zAngle = 0;
		if (float.IsNaN (yAngle)) {yAngle = 45f;}
		if (float.IsNaN (xAngle)) {xAngle = 0;}
		rigidbody.velocity = Quaternion.Euler (-yAngle, xAngle, zAngle) * Vector3.forward * iSpeed;
		lastTargPos = targetPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 direction = targetPosition - transform.position;
        Vector3 toInit = transform.position - initPos;
        //transform.localScale = transform.localScale.normalized * (5 * Mathf.Min(direction.magnitude / toInit.magnitude, toInit.magnitude / direction.magnitude));
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = transform.GetChild(i).localPosition.normalized * (3 * Mathf.Max(.01f, Mathf.Min(direction.magnitude / toInit.magnitude, toInit.magnitude / direction.magnitude)));
        }
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity) * Quaternion.Euler(0, 0, zRot);
        zRot += 7.5f + accel;
        if (target != null)
        {
            targetPosition = target.transform.position;
            calculateTrajectory(speed, speed*speed);
        }
		
		totalDist += rigidbody.velocity.magnitude * Time.fixedDeltaTime;
		if(totalDist > 500){
			killBullet();
		}
	}

	void killBullet(){
        Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == Layers.Player){
            PlayerController pc = other.transform.GetComponent<PlayerController>();
            //If the bullet hit the player
            if (pc != null)
            {
                //pc.TakeDamage();
            }
        }
        if (other.gameObject != from)
        {
            killBullet();
        }
	}
}
