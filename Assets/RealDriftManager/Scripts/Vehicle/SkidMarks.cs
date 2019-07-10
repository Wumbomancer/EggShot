//--------------------------------------------------------------
//
//                    Truck Parking kit
//        
//           Contact me : aliyeredon@gmail.com
//
//--------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SkidMarks : MonoBehaviour {

	public WheelCollider CorrespondingCollider ;
	public TrailRenderer skidMarkPrefab ;
	public ParticleSystem smoke;

	VehicleController2017 vehicleController;
	VehicleAudio2017 vehicleAudio;

	void Start() {
		manager = GameObject.FindObjectOfType<DriftManager> ();
		vehicleController = GetComponentInParent<VehicleController2017> ();
		vehicleAudio = GetComponentInParent<VehicleAudio2017> ();
	}

	DriftManager manager;

	bool isDrifting;


	void Update () {


		manager.isDriftingTemp = isDrifting;
		manager.speed = vehicleController.currentSpeed;

		// define a hit point for the raycast collision
		RaycastHit hit  ;
		// Find the collider's center point, you need to do this because the center of the collider might not actually be
		// the real position if the transform's off.
		Vector3 ColliderCenterPoint   = CorrespondingCollider.transform.TransformPoint( CorrespondingCollider.center );

		// now cast a ray out from the wheel collider's center the distance of the suspension, if it hit something, then use the "hit"
		// variable's data to find where the wheel hit, if it didn't, then se tthe wheel to be fully extended along the suspension.
		if ( Physics.Raycast( ColliderCenterPoint, -CorrespondingCollider.transform.up,out hit, CorrespondingCollider.suspensionDistance + CorrespondingCollider.radius ) ) {
			transform.position = hit.point + (CorrespondingCollider.transform.up * CorrespondingCollider.radius);
		}else{
			transform.position = ColliderCenterPoint - (CorrespondingCollider.transform.up * CorrespondingCollider.suspensionDistance);
		}

		// define a wheelhit object, this stores all of the data from the wheel collider and will allow us to determine
		// the slip of the tire.
		WheelHit CorrespondingGroundHit;
		CorrespondingCollider.GetGroundHit(out CorrespondingGroundHit );

		// if the slip of the tire is greater than 2.0, and the slip prefab exists, create an instance of it on the ground at
		// a zero rotation.
		if ( Mathf.Abs( CorrespondingGroundHit.sidewaysSlip ) > .14 )
		{
			skidMarkPrefab.enabled = true;
			smoke.enableEmission = true;
			isDrifting = true;
			manager.StartDrfit ();
			vehicleAudio.pitchMultiplier = Mathf.Lerp(vehicleAudio.pitchMultiplier,1.4f,Time.deltaTime*10);
		}
		else if ( Mathf.Abs( CorrespondingGroundHit.sidewaysSlip ) <= .14 )
		{
			skidMarkPrefab.enabled = false;
			isDrifting = false;
			manager.StopDrift ();
			smoke.enableEmission = false;
			vehicleAudio.pitchMultiplier = Mathf.Lerp(vehicleAudio.pitchMultiplier,1f,Time.deltaTime*10);

		}
	}
}
