using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	private Transform cam;

	// Use this for initialization
	void Start() {
		cam = Camera.main.transform;
	}

	// Update is called once per frame
	void Update() {
		
		transform.localRotation = Quaternion.LookRotation(cam.forward);
		//Vector3 eulers = transform.localEulerAngles;
		//transform.localEulerAngles = new Vector3(0, eulers.y, 0);
	}
}
