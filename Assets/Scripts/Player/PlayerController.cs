using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// Use this for initialization

	GameObject lockedOnTarget;
	GameObject highlight;

	void Start () {
		Materials.Initialize();
		highlight = new GameObject("Highlight");
	}
	
	// Update is called once per frame
	void Update () {
		HighlightTarget ();
	}

	void HighlightTarget(){
		RaycastHit target = CamRayHit();
		if (target.collider == null) {
			remHighLights();
			lockedOnTarget = null;
		}else if (lockedOnTarget == null) {
			if(target.distance > 1f){
				Debug.Log ("Looking at " + target.collider.name);
				lockedOnTarget = target.collider.gameObject;
				addHighLight(target.collider.gameObject, true);
			}
		}
		highlightFollowTarget ();
	}
	void addHighLight(GameObject target, bool isEnemy){
		Renderer[] rends = target.GetComponents<Renderer> ();
		highlight.transform.localScale = target.transform.lossyScale;
		for (int i = 0; i < rends.Length; i++) {
			//Copys the component, and then gets the list of materials.
			if(rends[i] is SkinnedMeshRenderer){
				Utilities.CopyComponent((SkinnedMeshRenderer)rends[i], highlight);
				((SkinnedMeshRenderer)highlight.renderer).sharedMesh = target.GetComponent<SkinnedMeshRenderer>().sharedMesh;
				((SkinnedMeshRenderer)highlight.renderer).bones = target.GetComponent<SkinnedMeshRenderer>().bones;

			}else{
				Utilities.CopyComponent(rends[i], highlight);
				Utilities.CopyComponent(target.GetComponent<MeshFilter>(), highlight);
				highlight.GetComponent<MeshFilter>().mesh = target.GetComponent<MeshFilter>().mesh;
			}
			Material[] mats = new Material[rends[i].materials.Length];
			for(int j = 0; j < rends[i].materials.Length; j++){
				mats[j] = isEnemy ? Materials.OutlineEnemy : Materials.OutlineTarget;
			}
			highlight.renderer.materials = mats;

			//If we want the entire object light up, we'll make highlight an array of highlights,
			//But for now 1 is probably okay.
			break;
		}

		//Uncommenting will recursively find a child to highlight
		if (rends.Length == 0) {
			for (int i = 0; i < target.transform.childCount; i++) {
				if(highlight.GetComponents<Renderer>().Length == 0){
					addHighLight(target.transform.GetChild(i).gameObject, isEnemy);
				}else{
					break;
				}
			}
		}

	}
	void highlightFollowTarget(){
		if (lockedOnTarget != null) {
			highlight.transform.position = lockedOnTarget.transform.position;
			highlight.transform.rotation = lockedOnTarget.transform.rotation;
		}
	}

	void remHighLights(){
		if (highlight.renderer != null) {
			Destroy (highlight.renderer);
		}
		if (highlight.GetComponent<MeshFilter>()) {
			Destroy (highlight.GetComponent<MeshFilter>());
		}
		transform.localScale = Vector3.one;
	}

	RaycastHit CamRayHit(float raycastRange = 50f, int ignoreList = (~Layers.Environment & ~Layers.Player)){
		RaycastHit camRayHit;
		Transform camera = transform.parent.GetChild(1).GetChild(0);
		Physics.Raycast (camera.position, camera.rotation * (Vector3.forward), out camRayHit, raycastRange, ignoreList);
		return camRayHit;
	}
}
