using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float zOffset;

	void Start() {
		zOffset = -transform.position.z;
	}

	void Update() {
        transform.position = transform.parent.position + new Vector3(0.0f, 0.0f, -zOffset);
        transform.rotation = Quaternion.identity;
    }
}
