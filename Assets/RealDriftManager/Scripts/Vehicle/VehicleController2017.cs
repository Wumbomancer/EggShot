using UnityEngine;
using System.Collections;

public enum InputType
{
	Keyboard,
	Mobile
}

public class VehicleController2017 : MonoBehaviour {

	public bool canControll;

	[Header("Wheels")]
	public WheelCollider[] Wheel_Colliders;

	public Transform[] Wheel_Transforms;

	// public Transform centerOfMass;


	public float currentSpeed;
	[Header("Vehicle Setup")]
	public float enginePower = 1400f ;

	public float maxSteer = 43f;

	// Input values
	float throttleInput;
	float steerInput;
	bool handBrake;
	float brakePower;

	// Used for detecting reverse mode (if localVel.z <0 => reversing, if localVel.z>0 => is not reversing)    
	Vector3 velocity;
	Vector3 localVel;
	[HideInInspector]public bool reversing;

	// Catch rigidbody
	Rigidbody rigid;

	// Gear values to control engine sound based on gears    
	public int numberOfGears = 10;
	int currentGear ;
	float GearFactor;
	[HideInInspector]public float Revs;

	public float GearShiftDelay = 0.3f;
	VehicleAudio2017 audioTruck;

	[Header("Lights")]
	// Vehicle lights
	public Light[] brakeLights;
	public Light[] reverseLights;

	public Transform COM;

	void Start()
	{

		StartCoroutine (GearChanging ());

		// I see this in unity car sample script, i don't khow what is do this
		Wheel_Colliders [0].attachedRigidbody.centerOfMass = new Vector3(0,0,0);

		rigid = GetComponent<Rigidbody> ();

		// used to smoothing smooth follow camera movement behind vehicle
		rigid.interpolation = RigidbodyInterpolation.Interpolate;

		// Set center of mass to center of mass transform localposition
		rigid.centerOfMass =  transform.InverseTransformPoint(COM.position);

		audioTruck = GetComponent<VehicleAudio2017> ();

	}


	void Update () 
	{


		InputSystem ();

		VehicleEngine ();

		// Update current speed and multiply to 3 for better understand
		currentSpeed = rigid.velocity.magnitude * 2.23693629f;

		// Find vehicle reversing state
		velocity = rigid.velocity;
		localVel = transform.InverseTransformDirection(velocity);

		if (localVel.z < 0)
			reversing = true;
		else
			reversing = false;

		// Align wheel mesh across wheel collider rotation and position
		for (int i = 0; i < Wheel_Colliders.Length; i++) 
		{
			Quaternion quat;
			Vector3 position;
			Wheel_Colliders [i].GetWorldPose (out position, out quat);
			Wheel_Transforms [i].transform.position = position;
			Wheel_Transforms [i].transform.rotation = quat;
		}

	}

	public void VehicleEngine()
	{
		CalculateRevs ();

		if (canControll) 
		{

			Wheel_Colliders [2].motorTorque = enginePower*throttleInput;
			Wheel_Colliders [3].motorTorque = enginePower*throttleInput;

			Wheel_Colliders [2].motorTorque = Mathf.Clamp (Wheel_Colliders [2].motorTorque, -enginePower/2, enginePower);
			Wheel_Colliders [3].motorTorque = Mathf.Clamp (Wheel_Colliders [3].motorTorque, -enginePower/2, enginePower);

			Wheel_Colliders [0].steerAngle = maxSteer * steerInput;
			Wheel_Colliders [1].steerAngle = maxSteer * steerInput;

			Wheel_Colliders [1].steerAngle = Mathf.Clamp (Wheel_Colliders [1].steerAngle, - (maxSteer/(currentSpeed/10)), (maxSteer/(currentSpeed/10)));
			Wheel_Colliders [0].steerAngle = Mathf.Clamp (Wheel_Colliders [0].steerAngle, - (maxSteer/(currentSpeed/10)), (maxSteer/(currentSpeed/10)));

			if (handBrake) { // Hand brake state

				Wheel_Colliders [2].brakeTorque = enginePower;
				Wheel_Colliders [3].brakeTorque = enginePower;

				LightIntensity (0, 1f);
				LightIntensity (1, 0);

				//Debug.Log ("1");
			} 
			else 
			{
				// note: We used enginePower as brake power
				// Speed decreasing when motor input value is  0
				if (throttleInput <= 0.07  && throttleInput >= -0.07f) 
				{//Debug.Log ("2");
					Wheel_Colliders [2].brakeTorque = enginePower / 5;
					Wheel_Colliders [3].brakeTorque = enginePower / 5;
					Wheel_Colliders [0].brakeTorque = enginePower / 5;
					Wheel_Colliders [1].brakeTorque = enginePower / 5;
					LightIntensity (0, 0);
					LightIntensity (1, 0);
				} 
				// Brake in forward moving
				else if (throttleInput < 0 && !reversing) 
				{//Debug.Log ("3");
					Wheel_Colliders [0].brakeTorque = enginePower * Mathf.Abs (throttleInput);
					Wheel_Colliders [1].brakeTorque = enginePower * Mathf.Abs (throttleInput);
					Wheel_Colliders [2].brakeTorque = enginePower * Mathf.Abs (throttleInput / 2);
					Wheel_Colliders [3].brakeTorque = enginePower * Mathf.Abs (throttleInput / 2);
					LightIntensity (0, 1f);
					LightIntensity (1, 0);
				} 
				// Brake in backward moving
				else if (throttleInput > 0 && reversing) 
				{//Debug.Log ("4");
					Wheel_Colliders [0].brakeTorque = enginePower * Mathf.Abs (throttleInput);
					Wheel_Colliders [1].brakeTorque = enginePower * Mathf.Abs (throttleInput);
					Wheel_Colliders [2].brakeTorque = enginePower * Mathf.Abs (throttleInput / 2);
					Wheel_Colliders [3].brakeTorque = enginePower * Mathf.Abs (throttleInput / 2);
					LightIntensity (0, 1f);
					LightIntensity (1, 0);
				} 
				//  Release brake ( is now driving)    
				else {//Debug.Log ("5");
					Wheel_Colliders [2].brakeTorque = 0;
					Wheel_Colliders [3].brakeTorque = 0;
					Wheel_Colliders [0].brakeTorque = 0;
					Wheel_Colliders [1].brakeTorque = 0;
					LightIntensity (0, 0);
					LightIntensity (1, 0);
				}

			}
			if (reversing && throttleInput < 0) {

				LightIntensity (0, 0);
				LightIntensity (1, 1f);
			}
		}
	}

	public void InputSystem()
	{

		throttleInput = Input.GetAxis ("Vertical");

			steerInput = Input.GetAxis ("Horizontal");

			handBrake = Input.GetKey(KeyCode.Space);

	}

	void LightIntensity(int type,float value)
	{
		if (type == 0) {
			for (int a = 0; a < brakeLights.Length; a++)
				brakeLights [a].intensity = value;
		} else {
			for (int a = 0; a < reverseLights.Length; a++)
				reverseLights [a].intensity = value;
		}
	}


	// Engine sound system calculation
	// Gear changing only used for sound system      

	IEnumerator GearChanging ()
	{
		while (true) 
		{
			yield return new WaitForSeconds (0.01f);
			if (!reversing) {
				float f = Mathf.Abs (currentSpeed / nextGearSpeed);
				float upgearlimit = (1 / (float)numberOfGears) * (currentGear + 1);
				float downgearlimit = (1 / (float)numberOfGears) * currentGear;

				// Changinbg gear down
				if (currentGear > 0 && f < downgearlimit) {
					// Reduce engine audio volume when changing gear
					audioTruck.audioSource.volume = 0.7f;
					//audioTruck.ChangeGear ();
					// Delay time for changing gear down
					yield return new WaitForSeconds (0);
					audioTruck.audioSource.volume = 1f;


					currentGear--;
				}

				// Changing gear Up
				if (f > upgearlimit && (currentGear < (numberOfGears - 1))) {
					// Reduce engine audio volume when changing gear
					audioTruck.audioSource.volume = 0.3f;
					//audioTruck.ChangeGear ();
					// Delay before changing gear up
					yield return new WaitForSeconds (GearShiftDelay);
					audioTruck.audioSource.volume = 1f;
					currentGear++;
				}
			} else {

				if (reversing)
					currentGear = 0;
			}
		}
	}

	// simple function to add a curved bias towards 1 for a value in the 0-1 range
	private static float CurveFactor (float factor)
	{
		return 1 - (1 - factor) * (1 - factor);
	}

	// unclamped version of Lerp, to allow value to exceed the from-to range
	private static float ULerp (float from, float to, float value)
	{
		return (1.0f - value) * from + value * to;
	}
	public float nextGearSpeed = 300f;
	// Used for engine sound system    
	private void CalculateGearFactor ()
	{
		float f = (1 / (float)numberOfGears);
		// gear factor is a normalised representation of the current speed within the current gear's range of speeds.
		// We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
		var targetGearFactor = Mathf.InverseLerp (f * currentGear, f * (currentGear + 1), Mathf.Abs (currentSpeed / nextGearSpeed));
		GearFactor = Mathf.Lerp (GearFactor, targetGearFactor, Time.deltaTime * 5f);
	}   

	// Used for engine sound system
	private void CalculateRevs ()
	{
		// calculate engine revs (for display / sound)
		// (this is done in retrospect - revs are not used in force/power calculations)
		CalculateGearFactor ();
		var gearNumFactor = currentGear / (float)numberOfGears;
		var revsRangeMin = ULerp (0f, 1f, CurveFactor (gearNumFactor));
		var revsRangeMax = ULerp (1f, 1f, gearNumFactor);
		Revs = ULerp (revsRangeMin, revsRangeMax, GearFactor);
	}


	public bool isDrifting;

	void Drift()
	{

		if (Input.GetKey (KeyCode.LeftShift)) {

			WheelFrictionCurve fc = new WheelFrictionCurve ();

			fc.extremumSlip = 0.3f;
			fc.extremumValue = 1f;
			fc.asymptoteSlip = 0.1f;
			fc.asymptoteValue = 10f;
			fc.stiffness = 1.4f;


			Wheel_Colliders [2].sidewaysFriction = fc;

			Wheel_Colliders [3].sidewaysFriction = fc;

		}
		else
		{


			WheelFrictionCurve fc1 = new WheelFrictionCurve ();

			fc1.extremumSlip = 0.3f;
			fc1.extremumValue = 1f;
			fc1.asymptoteSlip = 0.1f;
			fc1.asymptoteValue = 0.75f;
			fc1.stiffness = 1.3f;

			Wheel_Colliders [2].sidewaysFriction = fc1;

			Wheel_Colliders [3].sidewaysFriction = fc1;

		}
	}
}




