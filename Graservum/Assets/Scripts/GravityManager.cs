using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour {

    GameObject[] gravityObjects;

    void Start() {
        InvokeRepeating(updateList(), 0, 1);
    }

    void Update() {
        
    }

    void updateList() {
        gravityObjects = GameObject.FindGameObjectsWithTag("GravityObject");
    }
}
