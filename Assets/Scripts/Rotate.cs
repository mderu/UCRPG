using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	void Start () {
		transform.Rotate (Vector3.right, 50f);
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up, 1f);
	}
}
