using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	float speed = .0f;
	public int health = 1000;
	int nextWaypoint = 0;

	public Transform waypoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = waypoints.GetChild(nextWaypoint).position - transform.position;
		if (direction.magnitude >= speed) {
			transform.Translate (direction.normalized * speed);
		}else{
			transform.position = waypoints.GetChild(nextWaypoint).position;
			nextWaypoint = (nextWaypoint + 1) % waypoints.childCount;
		}
	}

	public void TakeDamage(int damage){
		health -= damage;
	}
}
