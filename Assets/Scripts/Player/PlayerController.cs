using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// Use this for initialization

    public GameObject lockedOnTarget;
	public GameObject lookingAtTarget;
    public List<GameObject> lockedOnHL = new List<GameObject>();
	public List<GameObject> lookingAtHL = new List<GameObject>();

	void Start () {
		Materials.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        HighlightTarget();
        highlightFollowTarget();
	}

	void HighlightTarget(){
		RaycastHit target = CamRayHit();
        if (target.collider == null)
        {
            ClearLookingAt();
            lookingAtTarget = null;
        }
        else
        {
			if(target.distance > 1f){
                if (Input.GetMouseButtonDown(1))
                {
                    lockedOnTarget = target.collider.gameObject;
                    if (lockedOnTarget == lookingAtTarget)
                    {
                        ClearLookingAt();
                        lookingAtTarget = null;
                    }
                    setHighLight(target.collider.gameObject, true);
                }
                else
                {
                    if (lookingAtTarget != target.collider.gameObject && target.collider.gameObject != lockedOnTarget)
                    {
                        lookingAtTarget = target.collider.gameObject;
                        setHighLight(target.collider.gameObject, false);
                    }
                }
			}
		}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockedOnTarget = null;
            ClearLockOn();
        }
	}
    void setHighLight(GameObject target, bool lockedOn)
    {
        List<GameObject> objHighlights = new List<GameObject>();
        Material hlColor;

        if ((target.layer == Layers.Enemies)){
            if (lockedOn){
                hlColor = Materials.OutlineEnemyLock;
            }else{
                hlColor = Materials.OutlineEnemy;
            }
        }else if (target.layer == Layers.Allies){
            hlColor = Materials.OutlineAlly;
        }
        else {
            hlColor = Materials.OutlineTarget; 
        }

        _setHighLight(target, objHighlights, hlColor);
        if (lockedOn) {
            ClearLockOn();
            lockedOnHL = objHighlights; 
        }
        else 
        {
            ClearLookingAt();
            lookingAtHL = objHighlights; 
        }
    }
	void _setHighLight(GameObject target, List<GameObject> objHighlights, Material hlColor)
    {
		Renderer rend = target.GetComponent<Renderer> ();
        if (rend)
        {
            GameObject highlight = new GameObject("Highlight");
            highlight.transform.localScale = target.transform.lossyScale;

            //Copys the component, and then gets the list of materials.
            if (rend is SkinnedMeshRenderer)
            {
                Utilities.CopyComponent((SkinnedMeshRenderer)rend, highlight);
                ((SkinnedMeshRenderer)highlight.renderer).sharedMesh = target.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                ((SkinnedMeshRenderer)highlight.renderer).bones = target.GetComponent<SkinnedMeshRenderer>().bones;
            }
            else
            {
                Utilities.CopyComponent(rend, highlight);
                Utilities.CopyComponent(target.GetComponent<MeshFilter>(), highlight);
                highlight.GetComponent<MeshFilter>().mesh = target.GetComponent<MeshFilter>().mesh;
            }
            Material[] mats = new Material[rend.materials.Length];
            for (int j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = hlColor;
            }
            highlight.renderer.materials = mats;

            objHighlights.Add(highlight);
        }

		//Recursively find a child to highlight
        if (target.transform.childCount > 0)
        {
			for (int i = 0; i < target.transform.childCount; i++) {
                _setHighLight(target.transform.GetChild(i).gameObject, objHighlights, hlColor);
			}
		}
	}

    //TODO: Make it so child objects follow movement as well.
	void highlightFollowTarget(){
		if (lockedOnTarget != null)
        {
            _followTarget(lockedOnTarget, lockedOnHL, 0);
		}
        if (lookingAtTarget != null)
        {
            _followTarget(lookingAtTarget, lookingAtHL, 0);
        }
	}

    void _followTarget(GameObject target, List<GameObject> list, int index)
    {
        if (!target.transform.renderer) { return; }
        list[index].transform.position = target.transform.position;
        list[index].transform.rotation = target.transform.rotation;
        list[index].transform.localScale = target.transform.lossyScale;
        if (target.transform.childCount > 0)
        {
			for (int i = 0; i < target.transform.childCount; i++)
            {
                index += 1;
                _followTarget(target.transform.GetChild(i).gameObject, list, index);
            }
		}
    }

	void ClearLockOn(){
        if (lockedOnHL.Count > 0)
        {
            for (int i = 0; i < lockedOnHL.Count; i++)
            {
                Destroy(lockedOnHL[i]);
            }
            lockedOnHL.Clear();
        }
	}

    void ClearLookingAt()
    {
        if (lookingAtHL.Count > 0)
        {
            for (int i = 0; i < lookingAtHL.Count; i++)
            {
                Destroy(lookingAtHL[i]);
            }
            lookingAtHL.Clear();
        }
    }

	RaycastHit CamRayHit(float raycastRange = 50f, int layerMask = Layers.TargetableMask){
		RaycastHit camRayHit;
		Transform camera = transform.parent.GetChild(1).GetChild(0);
        Physics.Raycast(camera.position, camera.rotation * (Vector3.forward), out camRayHit, raycastRange, layerMask);
		return camRayHit;
	}
}
