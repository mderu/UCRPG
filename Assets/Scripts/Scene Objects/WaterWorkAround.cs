using UnityEngine;
using System.Collections;

public class WaterWorkAround : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BoxCollider[] colliders = gameObject.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].isTrigger = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
