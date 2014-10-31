using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	RaycastHit CamRayHit(float raycastRange = 50f, int ignoreList = (~Layers.Environment & ~Layers.Player)){
		RaycastHit camRayHit;
		Transform camera = transform.parent.GetChild(1).GetChild(0);
		Physics.Raycast (camera.position, camera.rotation * (Vector3.forward), out camRayHit, raycastRange, ignoreList);
		return camRayHit;
	}

}
