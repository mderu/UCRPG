using UnityEngine;
using System.Collections;

//This script is used to place the camera in a position such that it does not look through objects, 
//while still being rotated around an object.

public class CamRaycastFromPivot : MonoBehaviour {
	private Vector3 initPos;
	public float distance = 3.5f;
	public float minCamDist = 1f;
	public float maxCamDist = 5f;
	public Transform follow;
	// Use this for initialization
	void Start () {
		if (follow == null) {
			follow = transform.parent.parent.GetChild (0);
		}
		initPos = transform.parent.position - follow.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.parent.position = follow.position + initPos;
		RaycastHit hit;

		//If the Raycast hits a target, move the camera to that point, so it doesn't pass through, else, keep the same distance.
		if (Physics.Raycast(transform.parent.position, transform.rotation * (-Vector3.forward), out hit, distance, ~Layers.Player)) {
			transform.localPosition = (-Vector3.forward).normalized * hit.distance;
		}else{
			transform.localPosition = (-Vector3.forward) * distance;
		}
		if (Input.GetMouseButtonUp(0)) {
			Screen.lockCursor = !Screen.lockCursor;
		}

		//Allow the player to scroll to zoom
		if ((minCamDist < distance && -Input.mouseScrollDelta.y < 0) || (distance < maxCamDist && -Input.mouseScrollDelta.y > 0)) {
			distance += -Input.mouseScrollDelta.y * .25f;
		}

		//Rotate camera based off of mouse movement
		if (Screen.lockCursor) {
			transform.parent.Rotate( new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f), Space.Self);
			transform.parent.localRotation = Quaternion.Euler (Mathf.Clamp((transform.parent.localRotation.eulerAngles.x > 180f) ? 
			                                                               (transform.parent.localRotation.eulerAngles.x - 360f) : 
			                                                               (transform.parent.localRotation.eulerAngles.x), -20f,30f),
			                                                   Mathf.Clamp((transform.parent.localRotation.eulerAngles.y > 180f) ? 
			            												   (transform.parent.localRotation.eulerAngles.y - 360) : 
			            												   (transform.parent.localRotation.eulerAngles.y), -180f,180f),
			                                                   0);
		}
	}
}
