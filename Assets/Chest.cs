using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

    bool chestOpened = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!chestOpened)
        {
            transform.GetChild(0).particleSystem.renderer.enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == Layers.Player)
        {
            chestOpened = true;
            transform.GetChild(0).particleSystem.renderer.enabled = true;
            transform.GetChild(1).gameObject.SetActive(true);
            animation.Play("chestOpen");
            Object.Destroy(collider);
        }
    }
}
