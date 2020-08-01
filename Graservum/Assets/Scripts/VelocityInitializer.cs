using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityInitializer : MonoBehaviour {

    public GameObject[] objects;
    public Vector3[] velocities;
    public Vector3[] angularVelocities;
    
    void Start() {
        for (int i = 0; i < objects.Length; i++) {
            objects[i].GetComponent<Rigidbody>().velocity = velocities[i];
            objects[i].GetComponent<Rigidbody>().angularVelocity = angularVelocities[i];
        }
    }
}
