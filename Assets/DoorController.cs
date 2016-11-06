using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public GameObject gate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OpenDoor() {
		gate.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			OpenDoor ();
			this.gameObject.SetActive (false);
		}
	}
}

