using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float zOffset = 10.0f;

    void Update() {
        transform.position = transform.parent.position + new Vector3(0.0f, 0.0f, -zOffset);
        transform.rotation = Quaternion.identity;
    }
}
