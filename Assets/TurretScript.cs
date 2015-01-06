using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
    public float reloadSpeed = 10;
    public float cannonballSpeed = 30;
    public float accuracyRadius = 1;
    public bool followTarget = false;
    bool fireBullet = false;
    public bool HighArcTrajectory = false;
    float reloadCounter = 0;

    public GameObject cannonBall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        reloadCounter += Time.deltaTime;
        if (reloadCounter >= reloadSpeed && fireBullet){
            GameObject ball = (GameObject)GameObject.Instantiate(cannonBall, transform.position, Quaternion.LookRotation(Vector3.up));
            ball.GetComponent<BulletMovement>().speed = cannonballSpeed;
            ball.GetComponent<BulletMovement>().highArc = HighArcTrajectory;
            ball.GetComponent<BulletMovement>().targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.up + Random.insideUnitSphere * accuracyRadius;
            ball.GetComponent<BulletMovement>().followTarget = followTarget;
            reloadCounter = 0;
        }
	}
}
