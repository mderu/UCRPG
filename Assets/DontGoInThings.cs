using UnityEngine;
using System.Collections;

public class DontGoInThings : MonoBehaviour {

	private Vector3 initPos;
	public float distance = 3.5f;
	private Vector3 centerOfScreen;
	public Transform follow;
	public RaycastHit camRayHit;
	public float raycastRange = 50f;
	// Use this for initialization
	void Start () {
		initPos = transform.parent.position - follow.position;
		
		centerOfScreen = new Vector3(Screen.width/2f, Screen.height/2f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.parent.position = follow.position + initPos;
		//Debug.DrawLine(transform.parent.position,transform.parent.position + transform.rotation * (-Vector3.forward));
		RaycastHit hit;

		if (Physics.Raycast(transform.parent.position, transform.rotation * (-Vector3.forward), out hit, distance, ~Layers.Player)) {
			//Debug.DrawLine(target.position,hit.point);
			//Debug.Log (hit.collider.name);
			transform.localPosition = (-Vector3.forward).normalized * hit.distance;
		}else{
			transform.localPosition = (-Vector3.forward) * distance;
		}


		/*if (Screen.lockCursor) {
			transform.GetChild(0).Rotate( new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f), Space.Self);
			transform.GetChild(0).rotation = Quaternion.Euler (Mathf.Clamp((transform.GetChild(0).rotation.eulerAngles.x > 180f) ? 
			                                                               (transform.GetChild(0).rotation.eulerAngles.x - 360) : 
			 															   (transform.GetChild(0).rotation.eulerAngles.x), -60f,60f),
			                                                   Mathf.Clamp((transform.GetChild(0).rotation.eulerAngles.y > 180f) ? 
			                                                               (transform.GetChild(0).rotation.eulerAngles.y - 360) : 
			 															   (transform.GetChild(0).rotation.eulerAngles.y), -60f,60f),
			                                                   0);
		}*/
		if (Input.GetMouseButtonUp(0)) {

			Screen.lockCursor = !Screen.lockCursor;
		}

		if((1f < distance && -Input.mouseScrollDelta.y < 0) || (distance < 5f && -Input.mouseScrollDelta.y > 0))
			distance += -Input.mouseScrollDelta.y *.25f;
			//Debug.Log(Input.mouseScrollDelta);

		if (Screen.lockCursor) {
			transform.parent.Rotate( new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f), Space.Self);
			transform.parent.localRotation = Quaternion.Euler (Mathf.Clamp((transform.parent.localRotation.eulerAngles.x > 180f) ? 
			                                                               (transform.parent.localRotation.eulerAngles.x - 360) : 
			                                                               (transform.parent.localRotation.eulerAngles.x), -20f,20f),
			                                                   Mathf.Clamp((transform.parent.localRotation.eulerAngles.y > 180f) ? 
			            												   (transform.parent.localRotation.eulerAngles.y - 360) : 
			            												   (transform.parent.localRotation.eulerAngles.y), -180f,180f),
			                                                   0);
			//Screen.lockCursor = true;
		}
		Physics.Raycast (transform.position, transform.rotation * (Vector3.forward), out camRayHit, raycastRange, ~Layers.Player & ~Layers.Floor);
	}
}
