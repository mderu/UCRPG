using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == Layers.Player)
        {
            animation.Play("chestOpen");
            Object.Destroy(collider);
        }
    }
}
