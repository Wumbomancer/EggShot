
// Writed by ALIyerEdon in Winter 2017
// Attach this script to wall and set "Wall" tag to wall

using UnityEngine;
using System.Collections;

public class DriftCollision : MonoBehaviour {

	DriftManager manager;

	public string cornerTag = "Wall";


	void Start () {
	
		manager = GameObject.FindObjectOfType<DriftManager> ();

	}
	

	void OnCollisionEnter (Collision col) {


		if (col.collider.CompareTag (cornerTag))
			manager.driftScore = 0;
	}
}
