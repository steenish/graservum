using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityInitializer : MonoBehaviour {

    public GameObject[] objects;
    public Vector3[] velocities;
    
    void Start() {
        for (int i = 0; i < objects.Length; i++) {
            objects[i].GetComponent<Rigidbody>().velocity = velocities[i];
        }
    }
}
