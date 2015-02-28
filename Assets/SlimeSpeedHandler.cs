using UnityEngine;
using System.Collections;

public class SlimeSpeedHandler : MonoBehaviour {

    public float speed = 0f;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
    void Update()
    {
        transform.parent.GetComponent<NavMeshAgent>().speed = speed;
	}
}
