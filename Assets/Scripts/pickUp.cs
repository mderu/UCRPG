using UnityEngine;
using System.Collections;

public class pickUp : MonoBehaviour {
	void OnTriggerEnter (Collider other){
		if (other.gameObject.layer == Layers.Player) {
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
