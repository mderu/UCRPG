using UnityEngine;
using System.Collections;

public class RangedWeapon : MonoBehaviour {
    public GameObject projectile;
    public GameObject target;

    public float reloadSpeed = 10;
    public float projectileSpeed = 30;
    public float accuracyRadius = 1;
    public bool HighArcTrajectory = false;
    public bool followTarget = false;

    //Turns on and off the shooting of the bullet.
    public bool fire = false;

    float reloadCounter = 0;

    /* 
     * This variable is in charge of determining whether or
     * not the animation of the bullet has gone off yet.
     * It is set to false when the animation is begun,
     * And set to true the first time the bullet is fired.
     * Then animation WILL call AnimationCalledBulletFire
     * multiple times, and setting bulletfired to true
     * prevents it from creating more than one bullet.
     */
    bool bulletFired = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        reloadCounter += Time.deltaTime;
        if (reloadCounter >= reloadSpeed && fire){
            bulletFired = false;
            transform.animation.Play();
            //Call Animation of firing the bullet.
            reloadCounter = 0;
        }
	}

    //The animation itself must call this 
    void AnimationCalledBulletFire()
    {
        if (!bulletFired)
        {
            GameObject ball = (GameObject)GameObject.Instantiate(projectile, transform.position, Quaternion.LookRotation(Vector3.up));
            ball.GetComponent<Projectile>().speed = projectileSpeed;
            ball.GetComponent<Projectile>().highArc = HighArcTrajectory;
            ball.GetComponent<Projectile>().targetPosition = target.transform.position + Vector3.up + Random.insideUnitSphere * accuracyRadius;
            ball.GetComponent<Projectile>().from = gameObject;
            if (followTarget)
            {
                ball.GetComponent<Projectile>().target = target;
            }
            bulletFired = true;
        }
    }
}
