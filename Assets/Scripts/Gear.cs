using UnityEngine;
using System.Collections;

public class Gear : MonoBehaviour {

	public float strength = 0f;

	public float rotSpeed = 0f;
	public float numTeeth = 8;
	private bool decay = true;
	private const float decayRate = .99f;

	// Use this for initialization
	void Start () {
		//rigidbody.angularVelocity = new Vector3 (0, 0, rotSpeed);
        if (rotSpeed != 0 && strength == 0) { strength = 1; }
        if (strength > 0) { decay = false; }
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = transform.rotation * Quaternion.Euler (0, 0, rotSpeed);
        if (decay) { rotSpeed *= decayRate; strength *= .99f; if (Mathf.Abs(rotSpeed) < .001) { rotSpeed = 0; strength = 0; } }
	}
	void OnTriggerExit(Collider other){
		Gear gear = other.GetComponent<Gear> ();
		if (gear != null) {
			if(Mathf.Abs(gear.strength) > Mathf.Abs(strength)){strength = -Mathf.Abs(strength); decay = true;}
			if(Mathf.Abs(strength) > Mathf.Abs(gear.strength)){gear.strength = -Mathf.Abs(gear.strength); gear.decay = true;}
		}
	}
	void OnTriggerStay(Collider other){
		Gear gear = other.GetComponent<Gear> ();
		if(gear != null){
			//Handle gears spinning properly
			if(strength > 0){
				if(Mathf.Abs(gear.strength) <= Mathf.Abs(strength/2f)){
					gear.strength = strength/2f;
					gear.rotSpeed = -rotSpeed * numTeeth/gear.numTeeth;
				}else if (Mathf.Abs(gear.strength) >= Mathf.Abs(strength*2f)){
                    gear.strength = Mathf.Abs(gear.strength);
					decay = false;
				}
            }
            //Handle gears disconnecting
            if (gear.decay && Mathf.Abs(gear.strength) > strength) { strength = -Mathf.Abs(strength); decay = true; }

		}
	}
}
