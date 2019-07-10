
// Writed by ALIyerEdon in Winter 2017
// Use this script to calculated drifting value and save scores in PlayerPrefs.SetInt("TotalScores")

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Normal => Easy  |  Power=> Medium  |  Delay=> Standard  |  DelayPower=>Hard   
public enum DriftMode
{
	Normal,
	Power,
	Delay
	,DelayPower
}
public class DriftManager : MonoBehaviour {


	[Header("Select Drift Mode")]
	// Select drift mode | Normal => Easy  |  Power=> Medium  |  Delay=> Standard  |  DelayPower=>Hard    
	public DriftMode driftMode = DriftMode.Normal;

	// Current score and totalscore
	[HideInInspector]public float driftScore,totalDriftScore;

	[Header("Informations")]
	// Display current and total drift scores
	public Text driftScoreText;
	public Text totalDriftScoreText;

	[Header("Drift Score Menu")]
	// Drift menu  appear when drifting is started and disappear when finished
	public GameObject driftScoreMenu;

	// Internal usage
	[HideInInspector]public bool isDrifting,saved;


	// Set this on car controller scripts (this speed = vehicle speed)
	[HideInInspector]public float speed;

	[Header("Power Mode Visuals")]
	// Heat slider (engine heat in poer modes)
	public Slider healthSlider;

	// Heat slider image (used for changing color to ed and green)
	public Image healthSliderImage;

	public float maxHealth = 300f;

	// Display engine over heat alarm image
	public GameObject damageIcon;

	[Header("Delay Mode Visuals")]
	// delay slider
	public Image delaySliderImage;

	bool canDrift;

	public float driftDelaySpeed = 10f;


	// Internal usage
	[HideInInspector]public   bool isDriftingTemp;
	bool isRunningDrift;

	[Header("Between Drifts Time")]
	// Delay between drifts pause
	public float driftTime = 1f;



	[Header("Drift Factor Visuals")]
	// Delay and increase drift factor
	public float driftFactorDelay = 1f;

	// Display Drift Factor
	public Text driftFactorText;

	// Drift factor (1X 2X,3X,4X ...)
	float driftFactor;



	/// <summary>
	/// T/////////////////////////////////////////////////////////////////	/// </summary>
	/// 
	/// Raycast system (Ground - Road Detector)
	/// 
	RaycastHit hit;

	[Header("Only On Road Drift")]
	public bool showDebug = false;

	Color color = Color.green; 

	// Give drift score only when car is on the road
	public bool roadDetection;

	// Road tag
	public string roadTag = "Road";

	bool isOnRoad;

	// Cast ray from this pont to road
	public Transform raycastSpot;


	/// <summary>
	/// S///////////////////////////////////////////	/// </summary>
	/// 
	/// 
	/// 
	void Start()
	{
/*		RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Keyboard;

		RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Drift;*/

		if(roadDetection)
			StartCoroutine ("RayscastUpdate");
		    
		// Read total scores first
		totalDriftScore = (float)PlayerPrefs.GetInt ("TotalScores");

		healthSlider.maxValue = maxHealth;

		// Save scores to player prfs in each 3 minutes
		StartCoroutine (saveScore ());

		if (driftMode == DriftMode.Normal) {

			healthSlider.gameObject.SetActive (false);
			damageIcon.SetActive (false);
			delaySliderImage.gameObject.SetActive (false);
		}
		if(driftMode == DriftMode.Power)
		{
			healthSlider.value = 0;
			healthSlider.gameObject.SetActive (true);
			damageIcon.SetActive (false);
			delaySliderImage.gameObject.SetActive (false);
		}
		if(driftMode == DriftMode.DelayPower)
		{

			healthSlider.value = 0; 

			healthSlider.gameObject.SetActive (true);
			damageIcon.SetActive (false);
			delaySliderImage.gameObject.SetActive (true);
		}
		if(driftMode == DriftMode.Delay)
		{

			healthSlider.gameObject.SetActive (false);
			damageIcon.SetActive (false);
			delaySliderImage.gameObject.SetActive (true);
		}

	}

	// Save score coroutine
	IEnumerator saveScore()
	{

		while (true) 
		{
			// Save in each 3 minutes
			yield return new WaitForSeconds (3);
			PlayerPrefs.SetInt ("TotalScores", (int)Mathf.Floor( totalDriftScore));

		}
	}

	void Update()
	{

		if(driftMode == DriftMode.Normal)
			DriftNormal ();
		if(driftMode == DriftMode.Power)
			DriftPower ();
		if(driftMode == DriftMode.DelayPower)
			DriftDelayPower ();
		if(driftMode == DriftMode.Delay)
			DriftDelay ();

		// Delete dringting score for test it agin
		if (Input.GetKeyDown (KeyCode.H))
			PlayerPrefs.DeleteAll ();
	}

	void DriftNormal()
	{

		// Calculate drifting scores based on vehicle speed
		if (isDrifting) 
		{
			if (isDriftingTemp) {
			
				if (roadDetection) {
					if (isOnRoad)
						driftScore = driftScore + speed / 100;
				} else {
					driftScore = driftScore + speed / 100;
				}
			}

			if (!driftScoreMenu.activeSelf) {
				driftScoreMenu.SetActive (true);
				StartCoroutine ("DriftFactor");

			}

			driftScoreText.text = ((int)Mathf.Floor(driftScore)).ToString ();

			if (saved)
				saved = false;
		}
		else
		{
			if (!saved) 
			{
				totalDriftScore += driftScore*driftFactor;
				saved = true;
				driftScore = 0;
				totalDriftScoreText.text = ((int)Mathf.Floor(totalDriftScore)).ToString("###,###,###,###");
			}

			if (driftScoreMenu.activeSelf) {
				driftScoreMenu.SetActive (false);
				StopCoroutine ("DriftFactor");
				driftFactor = 1;
			}
		}
	}

	void DriftPower()
	{

		if ((healthSlider.value * 100 / maxHealth) > 90f) {
			healthSliderImage.color = Color.red;
			damageIcon.SetActive (true);
		}
		else if ((healthSlider.value * 100 / maxHealth) > 70f) 
			healthSliderImage.color = Color.yellow;
		else {
			healthSliderImage.color = Color.green;
			damageIcon.SetActive(false);

		}

		// Calculate drifting scores based on vehicle speed
		if (isDrifting) 
		{
			if (isDriftingTemp) {

				if (roadDetection) {
					if (isOnRoad)
						driftScore = driftScore + speed / 100;
				} else {
					driftScore = driftScore + speed / 100;
				}
			}

			if (!driftScoreMenu.activeSelf) {
				driftScoreMenu.SetActive (true);
				StartCoroutine ("DriftFactor");

			}

			if (isDriftingTemp) {
				if (healthSlider.value < maxHealth)
					healthSlider.value += 1;
			} else {

				if(healthSlider.value>0)
					healthSlider.value -= 1;
			}

			driftScoreText.text = ((int)Mathf.Floor(driftScore)).ToString ();

			if (saved)
				saved = false;
		}
		else
		{
			if (!saved) 
			{
				totalDriftScore += driftScore*driftFactor;
				saved = true;
				driftScore = 0;
				totalDriftScoreText.text = ((int)Mathf.Floor(totalDriftScore)).ToString("###,###,###,###");
			}

			if (driftScoreMenu.activeSelf) {
				driftScoreMenu.SetActive (false);
				StopCoroutine ("DriftFactor");
				driftFactor = 1;
			}

			if (!isDriftingTemp)
			{
				if(healthSlider.value>0)
					healthSlider.value -= 1;
			}
		}
	}

	void DriftDelay()
	{

		// Calculate drifting scores based on vehicle speed
		if (isDrifting) 
		{

			delaySliderImage.gameObject.SetActive (true);
			
			if (!canDrift) {

				delaySliderImage.fillAmount -= 0.1f*Time.deltaTime*driftDelaySpeed*(speed/100)*2;
				if (delaySliderImage.fillAmount == 0)
					canDrift = true;
			} else {
				
				if (isDriftingTemp) {

					if (roadDetection) {
						if (isOnRoad)
							driftScore = driftScore + speed / 100;
					} else {
						driftScore = driftScore + speed / 100;
					}
				}

				if (!driftScoreMenu.activeSelf) {
					driftScoreMenu.SetActive (true);
					StartCoroutine ("DriftFactor");

				}

				driftScoreText.text = ((int)Mathf.Floor (driftScore)).ToString ();

				if (saved)
					saved = false;
			}
		}
		else
		{
			canDrift = false;
			delaySliderImage.fillAmount = 1f;
			delaySliderImage.gameObject.SetActive (false);

			if (!saved) 
			{
				totalDriftScore += driftScore*driftFactor;
				saved = true;
				driftScore = 0;
				totalDriftScoreText.text = ((int)Mathf.Floor(totalDriftScore)).ToString("###,###,###,###");
			}

			if (driftScoreMenu.activeSelf) {
				driftScoreMenu.SetActive (false);
				StopCoroutine ("DriftFactor");
				driftFactor = 1;
			}
		}
	}

	void DriftDelayPower()
	{

		if ((healthSlider.value * 100 / maxHealth) > 90f) {
			healthSliderImage.color = Color.red;
			damageIcon.SetActive (true);
		} else if ((healthSlider.value * 100 / maxHealth) > 70f)
			healthSliderImage.color = Color.yellow;
		else {
			healthSliderImage.color = Color.green;
			damageIcon.SetActive (false);
		}
		
		// Calculate drifting scores based on vehicle speed
		if (isDrifting) 
		{
			
			delaySliderImage.gameObject.SetActive (true);

			if (!canDrift) {

				delaySliderImage.fillAmount -= 0.1f*Time.deltaTime*driftDelaySpeed*(speed/100)*2;
				if (delaySliderImage.fillAmount == 0)
					canDrift = true;
			} else {

				if (isDriftingTemp) {

					if (roadDetection) {
						if (isOnRoad)
							driftScore = driftScore + speed / 100;
					} else {
						driftScore = driftScore + speed / 100;
					}
				}

				if (!driftScoreMenu.activeSelf) {
					driftScoreMenu.SetActive (true);
					StartCoroutine ("DriftFactor");

				}

				if (isDriftingTemp) {
					if (healthSlider.value < maxHealth)
						healthSlider.value += 1;
				} else {

					if(healthSlider.value>0)
						healthSlider.value -= 1;
				}

				driftScoreText.text = ((int)Mathf.Floor (driftScore)).ToString ();

				if (saved)
					saved = false;
			}
		}
		else
		{
			canDrift = false;
			delaySliderImage.fillAmount = 1f;
			delaySliderImage.gameObject.SetActive (false);

			if (!saved) 
			{
				totalDriftScore += driftScore*driftFactor;
				saved = true;
				driftScore = 0;
				totalDriftScoreText.text = ((int)Mathf.Floor(totalDriftScore)).ToString("###,###,###,###");
			}

			if (driftScoreMenu.activeSelf) {
				driftScoreMenu.SetActive (false);
				StopCoroutine ("DriftFactor");
				driftFactor = 1;
			}

			if (!isDriftingTemp)
			{
				if(healthSlider.value>0)
					healthSlider.value -= 1;
			}

		}
	}

	IEnumerator DriftFactor()
	{

		while (true) {

			driftFactorText.text = driftFactor.ToString ()+"x";
			yield return new WaitForSeconds (driftFactorDelay);
			driftFactor += 1;

		}

	}

	public void StartDrfit()
	{

		StopCoroutine ("stopDriftNow");
		isDrifting = true;
	}

	public void StopDrift()
	{
		if(!isRunningDrift)
			StartCoroutine ("stopDriftNow");
	}

	IEnumerator stopDriftNow()
	{

		yield return new WaitForSeconds (driftTime);
		if(!isDriftingTemp)
			isDrifting = false;

		isRunningDrift = false;
	}

	IEnumerator RayscastUpdate () {

		#if !UNITY_EDITOR
		showDebug = false;
		#endif

		while (true)
		{
			yield return new WaitForSeconds(0.1f);

			Raycast();
		}
	}

	void Raycast()
	{
		var up = raycastSpot.TransformDirection(Vector3.up);

		if(showDebug)
			Debug.DrawRay(raycastSpot.position, -up * 100f, color);


		if (Physics.Raycast(raycastSpot.position, -up, out hit, 100f))
		{
			if(showDebug)
				Debug.Log("Ground Name : "+hit.collider.name);

			if (hit.collider.tag == roadTag)
			{
				color = Color.red;
				isOnRoad = true;
			}
			else
			{
				color = Color.green;
				isOnRoad = false;
			}
		}
	}

}
