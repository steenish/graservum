using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Bounds cameraBounds { get; private set; }

	void Start() {
		Vector3 center = Vector3.zero;
		Vector3 size = new Vector3(Screen.width, Screen.height, 10.0f);
		cameraBounds = new Bounds(center, size);
	}
}
